using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Infrastructure;

namespace TQ.UseCases.TrevalQuest.Queries.Main.Patten
{
    internal class ExcelPatternNewTeamHandler : IRequestHandler<ExcelPatternNewTeamQuery, Result<(MemoryStream, string)>>
    {
        private readonly IMapper _mapper;
        public ExcelPatternNewTeamHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<Result<(MemoryStream, string)>> Handle(ExcelPatternNewTeamQuery request, CancellationToken cancellationToken)
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

            //Team пример
            (title, data, sheetName) = await ExcelDataPatternTeam();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 10 },
                        { 2, 30 },
                        { 3, 20 },
                        { 4, 20 },
                        { 5, 30 },
                        { 6, 15 },
                        { 7, 15 },
                        { 8, 10 }
                    },
                true
                ));
            stream = _excelParser.StyleExcelFile(stream);

            //CP пример
            (title, data, sheetName) = await ExcelDataPatternUser();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 10 },
                        { 2, 30 },
                        { 3, 15 },
                        { 4, 15 },
                        { 5, 15 },
                    },
                true
                ));
            stream = _excelParser.StyleExcelFile(stream);

            //Run
            (title, data, sheetName) = await ExcelDataTeam();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 10 },
                        { 2, 15 },
                        { 3, 20 },
                        { 4, 20 },
                        { 5, 15 },
                        { 6, 15 },
                        { 7, 15 },
                        { 8, 10 }
                    },
                true
                ));
            stream = _excelParser.StyleExcelFile(stream);

            //CP
            (title, data, sheetName) = await ExcelDataUser();
            stream = _excelParser.CreateUpdateExcelFile(stream, title, data, sheetName);

            _excelParser.AddSheetStyle(new StyleExcel(
                _excelParser.CheckAndChangeListName(sheetName),
                new Dictionary<int, double>()
                    {
                        { 1, 10 },
                        { 2, 10 },
                        { 3, 15 },
                        { 4, 15 },
                        { 5, 15 },
                    },
                true
                ));
            stream = _excelParser.StyleExcelFile(stream);

            return Result.Success((stream, _excelParser.SanitizeFileName("pattern-new-Team")));
        }

        /// <summary>
        /// Страница пояснения
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataInfo()
        {
            List<string> title = new List<string>() {
                "", "", "", "", "", "", "", "", "", "", ""
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "", "", "", "", "", "", "", "", "", "", ""
            });

            return (title, data, $"Пояснения");
        }

        /// <summary>
        /// Страница Team пример
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataPatternTeam()
        {
            List<string> title = new List<string>() {
                "id", "Код забега", "Дата регистрации", "Название", "Код команды", "Территория", "Группа", "Удалить"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "", "Шифр английскими буквами и цифрами", "Можно пустым", "Что угодно", "Шифр английскими буквами и цифрами", "Что угодно", "Что угодно", "да/нет"
            });


            return (title, data, $"Team Пример");
        }

        /// <summary>
        /// Страница User пример
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataPatternUser()
        {
            List<string> title = new List<string>() {
                "id", "Код команды", "Имя", "Фамилия", "Код с браслета"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "", "Шифр английскими буквами и цифрами", "Что угодно", "Что угодно", "Можно пустым"
            });


            return (title, data, $"User Пример");
        }

        /// <summary>
        /// Страница Team
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataTeam()
        {
            List<string> title = new List<string>() {
                "id", "Код забега", "Дата регистрации", "Название", "Код команды", "Территория", "Группа", "Удалить"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "", "", "", "", "", "", "", "" 
            });


            return (title, data, $"Team");
        }

        /// <summary>
        /// Страница User
        /// </summary>
        /// <returns></returns>
        async private Task<(List<string>, List<List<string>>, string)> ExcelDataUser()
        {
            List<string> title = new List<string>() {
                "id", "Код команды", "Имя", "Фамилия", "Код с браслета"
            };

            List<List<string>> data = new List<List<string>>();

            data.Add(new List<string>()
            {
                "", "", "", "", ""
            });


            return (title, data, $"User");
        }
    }
}
