using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Infrastructure;

namespace TQ.UseCases.TrevalQuest.Queries.Main.Patten
{
    internal class ExcelPatternNewRunHandler : IRequestHandler<ExcelPatternNewRunQuery, Result<(MemoryStream, string)>>
    {
        private readonly IMapper _mapper;
        public ExcelPatternNewRunHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<Result<(MemoryStream, string)>> Handle(ExcelPatternNewRunQuery request, CancellationToken cancellationToken)
        {
            var _excelParser = new ExcelWorking();
            var stream = new MemoryStream();

            //Пояснение
            var (title, data, sheetName) = await ExcelDataInfo();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName, "horizontal", 0, 0);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 150 }
                    },
                false
                ));
            stream = _excelParser.StyleExcelFile(stream);

            //Run пример
            (title, data, sheetName) = await ExcelDataPatternRun();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 30 },
                        { 2, 17 },
                        { 3, 30 },
                        { 4, 15 },
                        { 5, 30 },
                        { 6, 10 }
                    },
                true
                ));
            stream = _excelParser.StyleExcelFile(stream);

            //CP пример
            (title, data, sheetName) = await ExcelDataPatternCP();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 10 },
                        { 2, 30 },
                        { 3, 10 },
                        { 4, 20 },
                        { 5, 10 },
                        { 6, 10 },
                        { 7, 30 },
                        { 8, 15 },
                        { 9, 10 }
                    },
                true
                ));
            stream = _excelParser.StyleExcelFile(stream);

            //Run
            (title, data, sheetName) = await ExcelDataRun();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 15 },
                        { 2, 17 },
                        { 3, 30 },
                        { 4, 15 },
                        { 5, 30 },
                        { 6, 10 }
                    },
                true
                ));
            stream = _excelParser.StyleExcelFile(stream);

            //CP
            (title, data, sheetName) = await ExcelDataCP();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 10 },
                        { 2, 10 },
                        { 3, 10 },
                        { 4, 20 },
                        { 5, 10 },
                        { 6, 10 },
                        { 7, 10 },
                        { 8, 15 },
                        { 9, 10 }
                    },
                true
                ));
            stream = _excelParser.StyleExcelFile(stream);

            return Result.Success((stream, _excelParser.SanitizeFileName("pattern-new-run")));
        }

        /// <summary>
        /// Страница пояснения
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataInfo()
        {
            List<string> title = new List<string>() {
                "Поля id язвляются необязательными к заполнению (заполняются автомтически). Все остальные кроме удалить обязательны",
                "Поля, что заполняются автоматически можно изменить все параметры.", 
                "Поле со временем старта имеет следующий формат: 2025-08-13T14:00:00+05:00",
                "Где: 2025 - год, 08 - месяц, 13 - день.", 
                "T - обязательный символ разделения даты и времени",
                "14:00:00 обязательный формат времени старта - часы:минуты:секунды",
                "Также +05:00 является временной зоной в котрой СЕЙЧАС находится комьютер в формате часы:минуты (может быть как - так и +)",
                "ВАЖНО!",
                "Вычитайте временную зону из времени старта!",
                "Пример:",
                "Старт в 19:00:00, а вы находитесь в Перми (+5). Значит мы пишем  19-5=14 (T14:00:00+05:00)"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "", "", "", "", "", "", "", "", "", "", ""
            });

            return (title, data, $"Пояснения");
        }

        /// <summary>
        /// Страница run пример
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataPatternRun()
        {
            List<string> title = new List<string>() {
                "Код", "Название забега", "Время старта", "Время забега", "Описание", "Удалить"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "Шифр английскими буквами и цифрами", "Что угодно", "2025-08-13T14:00:00+05:00", "04:00:00", "Что угодно", "Да/Нет"
            });


            return (title, data, $"Run Пример");
        }

        /// <summary>
        /// Страница CP пример
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataPatternCP()
        {
            List<string> title = new List<string>() {
                "Код", "Номер КП", "Легенда", "Широта", "Долгота", "Код забега", "Очки за взятие", "Удалить"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "Англискими буквами и цифрами", "21", "Что угодно", "00.00000", "00.00000", "Шифр английскими буквами и цифрами", "5", ""
            });
            data.Add(new List<string>()
            {
                "Англискими буквами и цифрами", "31", "Что угодно", "00.00000", "00.00000", "Шифр английскими буквами и цифрами", "7", ""
            });
            data.Add(new List<string>()
            {
                "Англискими буквами и цифрами", "22", "Что угодно", "00.00000", "00.00000", "Шифр английскими буквами и цифрами", "10", "Да/Нет"
            });
            data.Add(new List<string>()
            {
                "Англискими буквами и цифрами", "32", "Что угодно", "00.00000", "00.00000", "Шифр английскими буквами и цифрами", "2", ""
            });


            return (title, data, $"CP Пример");
        }

        /// <summary>
        /// Страница run
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataRun()
        {
            List<string> title = new List<string>() {
                "Код", "Название забега", "Время старта", "Время забега", "Описание", "Удалить"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "", "", "", "", "", ""
            });

            return (title, data, $"Run");
        }

        /// <summary>
        /// Страница CP
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataCP()
        {
            List<string> title = new List<string>() {
                "Код", "Номер КП", "Легенда", "Широта", "Долгота", "Код забега", "Очки за взятие", "Удалить"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "", "", "", "", "", "", "", ""
            });

            return (title, data, $"CP");
        }
    }
}
