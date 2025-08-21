using Ardalis.Result;
using AutoMapper;
using MediatR;
using System.Globalization;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.Infrastructure;
using TQ.SharedKernel;

namespace TQ.UseCases.TrevalQuest.Queries.Main.Add
{
    class ExcelNewRunHandler : IRequestHandler<ExcelNewRunCommand, Result<(MemoryStream, string)>>
    {
        private readonly IMapper _mapper;
        private readonly IRunService _runService;
        private readonly ICPService _cpService;
        private readonly ICPRunService _cpRunService;
        public ExcelNewRunHandler(IMapper mapper, IRunService runService, ICPService cpService, ICPRunService cpRunService)
        {
            _mapper = mapper;
            _runService = runService;
            _cpService = cpService;
            _cpRunService = cpRunService;
        }

        // ===== Helpers =====

        private static string GetCell(IReadOnlyList<string> row, int idx) =>
            (idx >= 0 && idx < row.Count) ? (row[idx] ?? string.Empty).Trim() : string.Empty;

        private static bool TryParseDoubleMulti(string? s, out double value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(s)) return false;

            // Пытаемся: инвариант, текущая культура, ru-RU
            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value))
                return true;

            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out value))
                return true;

            var ru = CultureInfo.GetCultureInfo("ru-RU");
            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, ru, out value))
                return true;

            return false;
        }

        // Модели исхода по строкам (для Excel)
        private class RunOutcome
        {
            public int Row { get; set; }
            public List<string> Original { get; set; } = new(); // Ровно как в исходном Excel
            public string Status { get; set; } = "";            // Create OK / Update OK / Deleted OK / Validation Error / NotFound / Skipped / Error: ...
            public string Error { get; set; } = "";             // Детали ошибки
        }

        private class CPOutcome
        {
            public int Row { get; set; }
            public List<string> Original { get; set; } = new(); // Ровно как в исходном Excel
            public string StatusCP { get; set; } = "";          // Статус именно CP
            public string StatusCPRun { get; set; } = "";       // Статус связи (Run-CP)
            public string Error { get; set; } = "";             // Детали ошибки
        }

        public async Task<Result<(MemoryStream, string)>> Handle(ExcelNewRunCommand request, CancellationToken cancellationToken)
        {
            var excel = new ExcelWorking();

            using var stream = new MemoryStream();
            using var streamRes = new MemoryStream();
            var globalErrors = new List<string>();

            using (var fileStream = request.File.OpenReadStream())
            {
                await fileStream.CopyToAsync(stream, cancellationToken);
                stream.Position = 0;

                // Проверяем наличие листов
                var sheets = excel.GetSheets(stream);
                if (sheets == null || !sheets.Contains("Run"))
                    return Result.Error("Отсутствует страница Run");
                if (!sheets.Contains("CP"))
                    return Result.Error("Отсутствует страница CP");

                // Парсим листы
                var (titleRun, dataRun) = excel.ParseSheet(stream, "Run");
                stream.Position = 0;
                var (titleCP, dataCP) = excel.ParseSheet(stream, "CP");

                // ==== Обработка Runs (построчно, с сохранением исходных значений и статусов) ====
                var runOutcomes = new List<RunOutcome>();

                for (int i = 0; i < dataRun.Count; i++)
                {
                    var row = dataRun[i];
                    var outcome = new RunOutcome
                    {
                        Row = i + 2, // с учётом заголовка
                        Original = row.ToList()
                    };

                    try
                    {
                        string code = GetCell(row, 0);
                        string name = GetCell(row, 1);
                        string startStr = GetCell(row, 2);
                        string durStr = GetCell(row, 3);
                        string desc = GetCell(row, 4);
                        string del = GetCell(row, 5).ToLowerInvariant();

                        if (string.IsNullOrWhiteSpace(code))
                        {
                            outcome.Status = "Validation Error";
                            outcome.Error = "Пустой код забега";
                            runOutcomes.Add(outcome);
                            continue;
                        }

                        if (del == "да")
                        {
                            // Удаление
                            var delRes = await _runService.DeleteAsync(new RunId(code));
                            if (delRes.IsSuccess)
                            {
                                outcome.Status = "Deleted OK";
                            }
                            else
                            {
                                // Если в системе не найден — обозначим это явно
                                outcome.Status = delRes.Status == ResultStatus.NotFound ? "NotFound" : "Error";
                                outcome.Error = string.Join("; ", delRes.Errors ?? new List<string>()) switch
                                {
                                    "" => "Не удалось удалить Run",
                                    var s => s
                                };
                            }
                            runOutcomes.Add(outcome);
                            continue;
                        }

                        // Create/Update
                        if (!DateTime.TryParse(startStr, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var start)
                            && !DateTime.TryParse(startStr, out start))
                        {
                            outcome.Status = "Validation Error";
                            outcome.Error = "Неверный формат Время старта";
                            runOutcomes.Add(outcome);
                            continue;
                        }

                        if (!TimeSpan.TryParse(durStr, out var duration))
                        {
                            outcome.Status = "Validation Error";
                            outcome.Error = "Неверный формат Время забега";
                            runOutcomes.Add(outcome);
                            continue;
                        }

                        var existing = await _runService.GetAsync(new RunId(code));
                        if (existing == null)
                        {
                            var create = await _runService.CreateAsync(new Run(new RunId(code), name, start.ToUniversalTime(), duration, desc));
                            if (create.IsSuccess)
                            {
                                outcome.Status = "Create OK";
                            }
                            else
                            {
                                outcome.Status = "Error";
                                outcome.Error = string.Join("; ", create.Errors ?? new List<string>()) switch
                                {
                                    "" => "Не удалось создать Run",
                                    var s => s
                                };
                            }
                        }
                        else
                        {
                            existing.UpdateName(name);
                            existing.UpdateRunStart(start.ToUniversalTime());
                            existing.UpdateDuration(duration);
                            existing.UpdateDescription(desc);

                            var update = await _runService.UpdateAsync(existing);
                            if (update.IsSuccess)
                            {
                                outcome.Status = "Update OK";
                            }
                            else
                            {
                                outcome.Status = "Error";
                                outcome.Error = string.Join("; ", update.Errors ?? new List<string>()) switch
                                {
                                    "" => "Не удалось обновить Run",
                                    var s => s
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        outcome.Status = "Error";
                        outcome.Error = ex.Message;
                    }

                    runOutcomes.Add(outcome);
                }

                // ==== Подготовка данных о всех CPRun заранее (кеш), чтобы не дёргать БД в цикле ====
                var cpRunCache = new Dictionary<string, CPRun>(); // key: runId|cpId
                try
                {
                    var listRes = await _cpRunService.GetListAsync();
                    if (listRes.IsSuccess && listRes.Value != null)
                    {
                        foreach (var cpr in listRes.Value)
                            cpRunCache[$"{cpr.RunId.Value}|{cpr.CPId.Value}"] = cpr;
                    }
                }
                catch (Exception ex)
                {
                    globalErrors.Add($"Не удалось загрузить список CPRun: {ex.Message}");
                }

                // ==== Обработка CP (каждая строка формирует исход — и для CP, и для CPRun) ====
                var cpOutcomes = new List<CPOutcome>();

                for (int i = 0; i < dataCP.Count; i++)
                {
                    var row = dataCP[i];
                    var outcome = new CPOutcome
                    {
                        Row = i + 2,
                        Original = row.ToList(),
                        StatusCP = "",
                        StatusCPRun = "",
                        Error = ""
                    };

                    try
                    {
                        string cpCode = GetCell(row, 0);
                        string numberStr = GetCell(row, 1);
                        string legend = GetCell(row, 2);
                        string latStr = GetCell(row, 3);
                        string lonStr = GetCell(row, 4);
                        string runCode = GetCell(row, 5);
                        string scoreStr = GetCell(row, 6);
                        string del = GetCell(row, 7).ToLowerInvariant();

                        if (string.IsNullOrWhiteSpace(cpCode))
                        {
                            outcome.StatusCP = "Validation Error";
                            outcome.StatusCPRun = "Skipped";
                            outcome.Error = AppendError(outcome.Error, "Пустой код КП");
                            cpOutcomes.Add(outcome);
                            continue;
                        }

                        // === Блок удаления CP ===
                        if (del == "да")
                        {
                            var delRes = await _cpService.DeleteAsync(new CPId(cpCode));
                            if (delRes.IsSuccess)
                            {
                                outcome.StatusCP = "Deleted OK";
                            }
                            else
                            {
                                outcome.StatusCP = delRes.Status == ResultStatus.NotFound ? "NotFound" : "Error";
                                outcome.Error = AppendError(outcome.Error,
                                    string.Join("; ", delRes.Errors ?? new List<string>()).Length == 0
                                        ? "Не удалось удалить CP"
                                        : string.Join("; ", delRes.Errors));
                            }

                            // Для CPRun — пропускаем любые операции (предполагаем каскад/ручное удаление)
                            outcome.StatusCPRun = "Skipped (CP marked delete)";
                            cpOutcomes.Add(outcome);
                            continue;
                        }

                        // === Upsert CP (при наличии валидных данных), НО CPRun обрабатываем независимо ===
                        CP? cpEntity = null;
                        double lat = 0;
                        double lon = 0;
                        bool cpValid =
                            int.TryParse(numberStr, out int number) &&
                            TryParseDoubleMulti(latStr, out lat) &&
                            TryParseDoubleMulti(lonStr, out lon);

                        if (!cpValid)
                        {
                            // Не валиден — не создаём/обновляем CP, но дальше попробуем CPRun (возможно CP уже существует)
                            outcome.StatusCP = "Validation Error";

                            var existingCP = await _cpService.GetAsync(new CPId(cpCode));
                            if (existingCP != null)
                            {
                                // Пометим, что CP существует, но строка с некорректными данными.
                                cpEntity = existingCP;
                                outcome.Error = AppendError(outcome.Error, "Некорректные поля CP (номер/координаты)");
                            }
                            else
                            {
                                outcome.Error = AppendError(outcome.Error, "Некорректные поля CP (номер/координаты), CP не найден");
                            }
                        }
                        else
                        {
                            // Валидные данные — создаём/обновляем CP
                            var existing = await _cpService.GetAsync(new CPId(cpCode));
                            if (existing == null)
                            {
                                var create = await _cpService.CreateAsync(new CP(new CPId(cpCode), number, legend, lat, lon));
                                if (create.IsSuccess)
                                {
                                    cpEntity = create.Value;
                                    outcome.StatusCP = "Create OK";
                                }
                                else
                                {
                                    outcome.StatusCP = "Error";
                                    outcome.Error = AppendError(outcome.Error,
                                        string.Join("; ", create.Errors ?? new List<string>()).Length == 0
                                            ? "Не удалось создать CP"
                                            : string.Join("; ", create.Errors));
                                }
                            }
                            else
                            {
                                existing.UpdateNumber(number);
                                existing.UpdateLegend(legend);
                                existing.UpdateLatitude(lat);
                                existing.UpdateLongitude(lon);

                                var update = await _cpService.UpdateAsync(existing);
                                if (update.IsSuccess)
                                {
                                    cpEntity = update.Value;
                                    outcome.StatusCP = "Update OK";
                                }
                                else
                                {
                                    outcome.StatusCP = "Error";
                                    outcome.Error = AppendError(outcome.Error,
                                        string.Join("; ", update.Errors ?? new List<string>()).Length == 0
                                            ? "Не удалось обновить CP"
                                            : string.Join("; ", update.Errors));
                                }
                            }
                        }

                        // === CPRun — независимо от валидности CP-части строки ===
                        bool hasRunCode = !string.IsNullOrWhiteSpace(runCode);
                        bool hasScore = int.TryParse(scoreStr, out int score);

                        if (!hasRunCode && !hasScore)
                        {
                            outcome.StatusCPRun = "No CPRun data";
                        }
                        else if (!hasRunCode)
                        {
                            outcome.StatusCPRun = "Validation Error";
                            outcome.Error = AppendError(outcome.Error, "Пустой Код забега для CPRun");
                        }
                        else if (!hasScore)
                        {
                            outcome.StatusCPRun = "Validation Error";
                            outcome.Error = AppendError(outcome.Error, "Неверные очки для CPRun");
                        }
                        else
                        {
                            // Проверим наличие Run и CP в системе (CP — либо только что апсёртили, либо уже был)
                            var run = await _runService.GetAsync(new RunId(runCode));
                            if (run == null)
                            {
                                outcome.StatusCPRun = "NotFound (Run)";
                            }
                            else
                            {
                                CP? cpForLink = cpEntity ?? await _cpService.GetAsync(new CPId(cpCode));
                                if (cpForLink == null)
                                {
                                    outcome.StatusCPRun = "NotFound (CP)";
                                }
                                else
                                {
                                    var key = $"{run.Id.Value}|{cpForLink.Id.Value}";
                                    if (!cpRunCache.TryGetValue(key, out var link))
                                    {
                                        // Create
                                        var create = await _cpRunService.CreateAsync(new CPRun(run.Id, cpForLink.Id, score));
                                        if (create.IsSuccess)
                                        {
                                            outcome.StatusCPRun = "Create OK";
                                            cpRunCache[key] = create.Value;
                                        }
                                        else
                                        {
                                            outcome.StatusCPRun = "Error";
                                            outcome.Error = AppendError(outcome.Error,
                                                string.Join("; ", create.Errors ?? new List<string>()).Length == 0
                                                    ? "Не удалось создать CPRun"
                                                    : string.Join("; ", create.Errors));
                                        }
                                    }
                                    else
                                    {
                                        // Update
                                        link.UpdateScores(score);
                                        var update = await _cpRunService.UpdateAsync(link);
                                        if (update.IsSuccess)
                                        {
                                            outcome.StatusCPRun = "Update OK";
                                            cpRunCache[key] = update.Value;
                                        }
                                        else
                                        {
                                            outcome.StatusCPRun = "Error";
                                            outcome.Error = AppendError(outcome.Error,
                                                string.Join("; ", update.Errors ?? new List<string>()).Length == 0
                                                    ? "Не удалось обновить CPRun"
                                                    : string.Join("; ", update.Errors));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(outcome.StatusCP)) outcome.StatusCP = "Error";
                        if (string.IsNullOrEmpty(outcome.StatusCPRun)) outcome.StatusCPRun = "Error";
                        outcome.Error = AppendError(outcome.Error, ex.Message);
                    }

                    cpOutcomes.Add(outcome);
                }

                // ==== Формируем ответный Excel: те же колонки + статусы/ошибки ====

                // Run: заголовок как в исходном + "Статус", "Ошибка"
                var runTitle = new List<string>(titleRun);
                if (!runTitle.Contains("Статус")) runTitle.Add("Статус");
                if (!runTitle.Contains("Ошибка")) runTitle.Add("Ошибка");

                var runData = new List<List<string>>();
                foreach (var r in runOutcomes)
                {
                    var row = new List<string>(r.Original);
                    // добиваем до ширины заголовка (если исходник короче)
                    while (row.Count < runTitle.Count - 2) row.Add("");
                    row.Add(r.Status);
                    row.Add(r.Error);
                    runData.Add(row);
                }

                var (tRun, dRun, sRun) = (runTitle, runData, "Run");
                var outStream = excel.CreateUpdateExcelFile(new MemoryStream(), tRun, dRun, sRun);

                excel.AddSheetStyle(new StyleExcel(
                    excel.CheckAndChangeListName(sRun),
                    new Dictionary<int, double>
                    {
                        { 1, 15 }, // Код
                        { 2, 25 }, // Название
                        { 3, 28 }, // Время старта
                        { 4, 18 }, // Время забега
                        { 5, 30 }, // Описание
                        { 6, 10 }, // Удалить
                        { 7, 18 }, // Статус
                        { 8, 40 }, // Ошибка
                    },
                    true
                ));
                outStream = excel.StyleExcelFile(outStream);

                // CP: заголовок как в исходном + "Статус CP", "Статус CPRun", "Ошибка"
                var cpTitle = new List<string>(titleCP);
                if (!cpTitle.Contains("Статус CP")) cpTitle.Add("Статус CP");
                if (!cpTitle.Contains("Статус CPRun")) cpTitle.Add("Статус CPRun");
                if (!cpTitle.Contains("Ошибка")) cpTitle.Add("Ошибка");

                var cpData = new List<List<string>>();
                foreach (var c in cpOutcomes)
                {
                    var row = new List<string>(c.Original);
                    while (row.Count < cpTitle.Count - 3) row.Add("");
                    row.Add(c.StatusCP);
                    row.Add(c.StatusCPRun);
                    row.Add(c.Error);
                    cpData.Add(row);
                }

                var (tCP, dCP, sCP) = (cpTitle, cpData, "CP");
                outStream = excel.CreateUpdateExcelFile(outStream, tCP, dCP, sCP);

                excel.AddSheetStyle(new StyleExcel(
                    excel.CheckAndChangeListName(sCP),
                    new Dictionary<int, double>
                    {
                        { 1, 14 }, // Код
                        { 2, 12 }, // Номер КП
                        { 3, 24 }, // Легенда
                        { 4, 16 }, // Широта
                        { 5, 16 }, // Долгота
                        { 6, 16 }, // Код забега
                        { 7, 16 }, // Очки
                        { 8, 12 }, // Удалить
                        { 9, 16 }, // Статус CP
                        { 10, 18 }, // Статус CPRun
                        { 11, 40 }, // Ошибка
                    },
                    true
                ));
                outStream = excel.StyleExcelFile(outStream);

                // Итог

                return new Result<(MemoryStream, string)>((outStream, "result-add-run"));
            }
        }
        private static string AppendError(string existing, string add) =>
            string.IsNullOrWhiteSpace(existing) ? add : $"{existing}; {add}";
    }
}