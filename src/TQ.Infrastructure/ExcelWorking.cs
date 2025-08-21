using Ardalis.Result;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TQ.Infrastructure
{
    public class ExcelWorking
    {
        /// <summary>
        /// Переменная стилей для каждой страницы
        /// </summary>
        public List<StyleExcel> SheetStyles { get; private set; }

        public ExcelWorking()
        {
            SheetStyles = new List<StyleExcel>();
        }
        public ExcelWorking(List<StyleExcel> sheetStyles)
        {
            SheetStyles = sheetStyles;
        }

        public void AddSheetStyle(StyleExcel sheetStyle)
        {
            SheetStyles.Add(sheetStyle);
        }

        /// <summary>
        /// Функция для применения заданых стилей к заданым страницам excel
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public MemoryStream StyleExcelFile(MemoryStream stream)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;

                // Инициализация WorkbookStylesPart, если он отсутствует
                if (workbookPart.WorkbookStylesPart == null)
                {
                    workbookPart.AddNewPart<WorkbookStylesPart>();
                    InitializeStyles(workbookPart.WorkbookStylesPart);
                }

                foreach (var sheetStyle in SheetStyles)
                {
                    Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetStyle.SheetName);

                    if (sheet == null)
                    {
                        throw new ArgumentException($"Sheet with name '{sheetStyle.SheetName}' does not exist.");
                    }

                    WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id));
                    Columns columns = worksheetPart.Worksheet.GetFirstChild<Columns>();
                    if (columns == null)
                    {
                        columns = new Columns();
                        worksheetPart.Worksheet.InsertAt(columns, 0);
                    }

                    // Установка ширины колонок
                    if (sheetStyle.ColumnWidths != null)
                    {
                        foreach (var columnWidth in sheetStyle.ColumnWidths)
                        {
                            Column column = new Column()
                            {
                                Min = (uint)columnWidth.Key,
                                Max = (uint)columnWidth.Key,
                                Width = columnWidth.Value,
                                CustomWidth = true
                            };
                            columns.Append(column);
                        }
                    }

                    // Создание стиля с переносом текста
                    if (sheetStyle.WrapText)
                    {
                        var stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;

                        CellFormat cellFormat = new CellFormat()
                        {
                            Alignment = new Alignment()
                            {
                                WrapText = sheetStyle.WrapText
                            }
                        };

                        stylesheet.CellFormats.Append(cellFormat);
                        stylesheet.Save();



                        uint styleIndex = (uint)(stylesheet.CellFormats.Count() - 1);

                        // Применение стиля ко всем ячейкам на листе
                        foreach (Row row in worksheetPart.Worksheet.GetFirstChild<SheetData>().Descendants<Row>())
                        {
                            foreach (Cell cell in row.Descendants<Cell>())
                            {
                                cell.StyleIndex = styleIndex;
                            }
                        }
                    }
                    worksheetPart.Worksheet.Save();
                }
            }
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Создание основного стиля для Excel. Нужен в том случае, если стили в документе не были заданы.
        /// </summary>
        /// <param name="workbookStylesPart"></param>
        private void InitializeStyles(WorkbookStylesPart workbookStylesPart)
        {
            Stylesheet stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = stylesheet;

            // Создание основного стиля
            stylesheet.NumberingFormats = new NumberingFormats();
            stylesheet.Fonts = new Fonts(new Font());
            stylesheet.Fills = new Fills(new Fill());
            stylesheet.Borders = new Borders(new Border());
            stylesheet.CellStyleFormats = new CellStyleFormats(new CellFormat());
            stylesheet.CellFormats = new CellFormats(new CellFormat()); // По умолчанию пустой стиль

            workbookStylesPart.Stylesheet.Save();
        }

        /// <summary>
        /// Возвращает список всех листов (названия) в Excel-документе.
        /// </summary>
        /// <param name="excel">Поток с Excel-файлом</param>
        /// <returns>Список названий страниц</returns>
        public List<string> GetSheets(Stream excel)
        {
            var result = new List<string>();
            using (var document = SpreadsheetDocument.Open(excel, false))
            {
                var workbookPart = document.WorkbookPart;
                if (workbookPart?.Workbook?.Sheets != null)
                {
                    foreach (Sheet sheet in workbookPart.Workbook.Sheets.OfType<Sheet>())
                    {
                        result.Add(sheet.Name);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Читает данные с конкретного листа Excel и возвращает титульник отдельно от данных.
        /// Пустые ячейки не пропускаются, а вставляются как пустая строка.
        /// </summary>
        /// <param name="excel">Поток с Excel-файлом</param>
        /// <param name="sheetName">Имя страницы для парсинга</param>
        /// <returns>Кортеж: титульник и список строк с данными</returns>
        public (List<string> Title, List<List<string>> Data) ParseSheet(MemoryStream excel, string sheetName)
        {
            var data = new List<List<string>>();
            var title = new List<string>();

            using (var document = SpreadsheetDocument.Open(excel, false))
            {
                var workbookPart = document.WorkbookPart!;
                var sheet = workbookPart.Workbook.Sheets.OfType<Sheet>().FirstOrDefault(s => s.Name == sheetName);

                if (sheet == null)
                    throw new ArgumentException($"Лист с именем {sheetName} не найден.");

                var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                if (sheetData == null)
                    return (title, data);

                // Определяем границы прямоугольника (макс. строка и столбец)
                uint maxRow = sheetData.Elements<Row>().Max(r => r.RowIndex!.Value);
                uint maxCol = 0;
                foreach (var row in sheetData.Elements<Row>())
                {
                    foreach (var cell in row.Elements<Cell>())
                    {
                        var colIndex = GetColumnIndex(cell.CellReference!);
                        if (colIndex > maxCol)
                            maxCol = colIndex;
                    }
                }

                // Формируем прямоугольник
                for (uint rowIndex = 1; rowIndex <= maxRow; rowIndex++)
                {
                    var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
                    var rowValues = new List<string>();

                    for (uint colIndex = 1; colIndex <= maxCol; colIndex++)
                    {
                        string cellRef = GetCellReference((int)colIndex, (int)rowIndex);
                        var cell = row?.Elements<Cell>().FirstOrDefault(c => c.CellReference == cellRef);
                        string value = GetCellText(document, cell);
                        rowValues.Add(value);
                    }

                    if (rowIndex == 1)
                        title = rowValues; // титульник
                    else
                        data.Add(rowValues); // остальные данные
                }
            }

            return (title, data);
        }

        /// <summary>
        /// Функция генерации excel таблицы и заполнения ее страниц данными. 
        /// Способна сама сгенерировать начальные данные для документа и заполнить колонки со смещением. Также заполнять разные страницы по имени.
        /// </summary>
        /// <param name="stream">Основной stream документа с которым работаем</param>
        /// <param name="titleData">Титульник будующей таблицы</param>
        /// <param name="data">Данные которые будут занесены в таблицу. Внутренний List должен соответствовать размеру титульника.</param>
        /// <param name="sheetName">Имя страницы</param>
        /// <param name="orientation">Ориентация страницы: standard — стандартная ориентация, horizontal — горизонтальная ориентация.</param>
        /// <param name="x">Смещение таблицы по столбцам (начиная с 0)</param>
        /// <param name="y">Смещение таблицы по строкам (начиная с 0)</param>
        /// <returns></returns>
        public Result<MemoryStream> CreateUpdateExcelFile(MemoryStream stream, List<string> titleData, List<List<string>> data, string sheetName = null, string orientation = "vertical", int x = 0, int y = 0)
        {
            sheetName = sheetName != null ? CheckAndChangeListName(sheetName) : null;
            switch (orientation)
            {
                case "vertical":
                    return CreateExcelFileStandart(stream, titleData, data, sheetName, x + 1, y + 1);
                case "horizontal":
                    return CreateExcelFileHorizontal(stream, titleData, data, sheetName, x + 1, y + 1);
                default:
                    return Result.Error("Неизвестная ориентация документа");

            }
        }

        private Result<MemoryStream> CreateExcelFileStandart(MemoryStream stream, List<string> titleData, List<List<string>> data, string sheetName, int x, int y)
        {
            if (data[0].Count != titleData.Count)
            {
                return Result.Error("Длина титульника и строчки значения не сходится");
            }

            bool isNewDocument = stream.Length == 0;
            using (var spreadsheetDocument = isNewDocument ? SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook) : SpreadsheetDocument.Open(stream, true))
            {
                WorkbookPart workbookPart;
                Sheets sheets;
                SheetData sheetData;
                WorksheetPart worksheetPart;

                if (isNewDocument)
                {
                    workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                    sheets = workbookPart.Workbook.AppendChild(new Sheets());

                    worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());
                    sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    var sheetId = sheets.Elements<Sheet>().Count() + 1;
                    var newSheetName = sheetName ?? $"Лист{sheetId}";

                    var sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = (uint)sheetId,
                        Name = newSheetName
                    };
                    sheets.Append(sheet);
                }
                else
                {
                    workbookPart = spreadsheetDocument.WorkbookPart;
                    sheets = workbookPart.Workbook.GetFirstChild<Sheets>();

                    Sheet sheet = sheets.Elements<Sheet>().FirstOrDefault(s => s.Name == sheetName);
                    if (sheet != null)
                    {
                        worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(sheet.Id);
                        sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                    }
                    else
                    {
                        worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet(new SheetData());
                        sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                        var sheetId = sheets.Elements<Sheet>().Count() + 1;
                        var newSheetName = sheetName ?? $"Лист{sheetId}";

                        sheet = new Sheet()
                        {
                            Id = workbookPart.GetIdOfPart(worksheetPart),
                            SheetId = (uint)sheetId,
                            Name = newSheetName
                        };
                        sheets.Append(sheet);
                    }
                }

                // Calculate boundaries for the full rectangle to be filled
                int endY = y + data.Count;
                int endX = x + data[0].Count;

                // Fill the area to the left of the data block
                for (int rowIdx = 1; rowIdx <= endY; rowIdx++)
                {
                    Row row = GetOrCreateRow(sheetData, (uint)rowIdx);
                    for (int colIdx = 1; colIdx < x; colIdx++)
                    {
                        FillEmptyCell(row, colIdx, rowIdx);
                    }
                }

                // Fill the area above the data block
                for (int rowIdx = 1; rowIdx < y; rowIdx++)
                {
                    Row row = GetOrCreateRow(sheetData, (uint)rowIdx);
                    for (int colIdx = 1; colIdx < endX; colIdx++)
                    {
                        FillEmptyCell(row, colIdx, rowIdx);
                    }
                }

                // Fill the top-left rectangle
                for (int rowIdx = 1; rowIdx < y; rowIdx++)
                {
                    Row row = GetOrCreateRow(sheetData, (uint)rowIdx);
                    for (int colIdx = 1; colIdx < x; colIdx++)
                    {
                        FillEmptyCell(row, colIdx, rowIdx);
                    }
                }

                // Add column headers starting from position (x, y)
                var headerRowIndex = (uint)y;
                var headerRow = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == headerRowIndex);
                if (headerRow == null)
                {
                    headerRow = new Row() { RowIndex = headerRowIndex };
                    sheetData.Append(headerRow);
                }

                int headerColumnIndex = x;
                foreach (var key in titleData)
                {
                    Cell cell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(key),
                        CellReference = GetCellReference(headerColumnIndex, y)
                    };

                    headerRow.InsertAt(cell, headerColumnIndex - 1);
                    headerColumnIndex++;
                }

                // Add rows from data starting from position (x, y + 1)
                uint rowIndex = (uint)(y + 1);
                foreach (var item in data)
                {
                    var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
                    if (row == null)
                    {
                        row = new Row() { RowIndex = rowIndex };
                        sheetData.Append(row);
                    }

                    for (int keyId = 0; keyId < titleData.Count; keyId++)
                    {
                        string value = item[keyId];
                        int columnIndex = x + keyId;
                        Cell cell = new Cell
                        {
                            CellReference = GetCellReference(columnIndex, (int)rowIndex)
                        };

                        if (double.TryParse(value, out double numberValue))
                        {
                            cell.DataType = CellValues.Number;
                            cell.CellValue = new CellValue(numberValue);
                        }
                        else
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(value);
                        }

                        row.InsertAt(cell, columnIndex - 1);
                    }

                    rowIndex++;
                    if (rowIndex == 276)
                    {
                    }
                }

                worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();
            }

            stream.Position = 0;
            return Result.Success(stream);
        }

        private Result<MemoryStream> CreateExcelFileHorizontal(MemoryStream stream, List<string> titleData, List<List<string>> data, string sheetName, int x, int y)
        {
            if (data[0].Count != titleData.Count)
            {
                return Result.Error("Длина титульника и строчки значения не сходится");
            }

            bool isNewDocument = stream.Length == 0;
            using (var spreadsheetDocument = isNewDocument ? SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook) : SpreadsheetDocument.Open(stream, true))
            {
                WorkbookPart workbookPart;
                Sheets sheets;
                SheetData sheetData;
                WorksheetPart worksheetPart;

                if (isNewDocument)
                {
                    workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                    sheets = workbookPart.Workbook.AppendChild(new Sheets());

                    worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());
                    sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    var sheetId = sheets.Elements<Sheet>().Count() + 1;
                    var newSheetName = sheetName ?? $"Лист{sheetId}";

                    var sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = (uint)sheetId,
                        Name = newSheetName
                    };
                    sheets.Append(sheet);
                }
                else
                {
                    workbookPart = spreadsheetDocument.WorkbookPart;
                    sheets = workbookPart.Workbook.GetFirstChild<Sheets>();

                    Sheet sheet = sheets.Elements<Sheet>().FirstOrDefault(s => s.Name == sheetName);
                    if (sheet != null)
                    {
                        worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(sheet.Id);
                        sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                    }
                    else
                    {
                        worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet(new SheetData());
                        sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                        var sheetId = sheets.Elements<Sheet>().Count() + 1;
                        var newSheetName = sheetName ?? $"Лист{sheetId}";

                        sheet = new Sheet()
                        {
                            Id = workbookPart.GetIdOfPart(worksheetPart),
                            SheetId = (uint)sheetId,
                            Name = newSheetName
                        };
                        sheets.Append(sheet);
                    }
                }

                // Calculate boundaries for the full rectangle to be filled
                int endY = y + titleData.Count - 1;
                int endX = x + data.Count;

                // Fill the area to the left of the data block
                for (int rowIdx = 1; rowIdx <= endY; rowIdx++)
                {
                    Row row = GetOrCreateRow(sheetData, (uint)rowIdx);
                    for (int colIdx = 1; colIdx < x; colIdx++)
                    {
                        FillEmptyCell(row, colIdx, rowIdx);
                    }
                }

                // Fill the area above the data block
                for (int rowIdx = 1; rowIdx < y; rowIdx++)
                {
                    Row row = GetOrCreateRow(sheetData, (uint)rowIdx);
                    for (int colIdx = 1; colIdx <= endX; colIdx++)
                    {
                        FillEmptyCell(row, colIdx, rowIdx);
                    }
                }

                // Fill the top-left rectangle
                for (int rowIdx = 1; rowIdx < y; rowIdx++)
                {
                    Row row = GetOrCreateRow(sheetData, (uint)rowIdx);
                    for (int colIdx = 1; colIdx < x; colIdx++)
                    {
                        FillEmptyCell(row, colIdx, rowIdx);
                    }
                }

                for (int i = 0; i < titleData.Count; i++)
                {
                    var rowIndex = y + i;
                    var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == (uint)rowIndex && r.RowIndex is not null);
                    //var row = sheetData!.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).First();
                    if (row == null)
                    {
                        row = new Row() { RowIndex = (uint)rowIndex };
                        sheetData.Append(row);
                    }
                    var headerCell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(titleData[i]),
                        CellReference = GetCellReference(x, rowIndex)
                    };
                    row.InsertAt(headerCell, x - 1);

                    int j = 0;
                    foreach (var item in data)
                    {
                        var cellReference = GetCellReference(x + j + 1, rowIndex);
                        var cell = new Cell()
                        {
                            CellReference = cellReference
                        };
                        string cellValue = item[i];

                        if (double.TryParse(cellValue, out double numberValue))
                        {
                            cell.DataType = CellValues.Number;
                            cell.CellValue = new CellValue(numberValue);
                        }
                        else
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(cellValue);
                        }
                        row.InsertAt(cell, x + j);
                        j++;
                    }
                    //sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();
            }

            stream.Position = 0;
            return Result.Success(stream);
        }

        /// <summary>
        /// Получает названия всех листов в предоставленном Excel-файле.
        /// </summary>
        /// <param name="excel">Поток, содержащий Excel-файл.</param>
        /// <returns>Список строк с названиями листов.</returns>
        public List<string> GetSheetNames(Stream excel)
        {
            var sheetNames = new List<string>();

            // Открываем Excel-файл в режиме чтения
            using (var document = SpreadsheetDocument.Open(excel, false))
            {
                // Получаем WorkbookPart, содержащий информацию обо всех листах
                var workbookPart = document.WorkbookPart;

                if (workbookPart != null)
                {
                    // Проходим по всем листам в Workbook
                    foreach (Sheet sheet in workbookPart.Workbook.Sheets)
                    {
                        sheetNames.Add(sheet.Name);
                    }
                }
            }

            return sheetNames;
        }

        #region private
        /// <summary>
        /// Метод для извлечения текстового значения из ячейки Excel. Учитывает, что ячейки могут содержать значения из таблицы общих строк (SharedStringTable).
        /// </summary>
        /// <param name="document">Документ Excel, содержащий ячейку.</param>
        /// <param name="cell">Ячейка Excel, из которой нужно извлечь текст.</param>
        /// <returns>Возвращает текстовое значение ячейки. Если ячейка содержит ссылку на таблицу общих строк, извлекает значение из этой таблицы.</returns>
        private string GetCellText(SpreadsheetDocument document, Cell cell)
        {
            if (cell == null || cell.CellValue == null) return string.Empty;

            string value = cell.CellValue.InnerText;

            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                var stringTable = document.WorkbookPart!.SharedStringTablePart!.SharedStringTable;
                value = stringTable.ChildElements[int.Parse(value)].InnerText;
            }

            return value;
        }
        /// <summary>
        /// Метод для попытки преобразования текстового представления числа в значение типа double.
        /// Этот метод учитывает оба возможных разделителя дробной части: точку и запятую, а также поддерживает отрицательные значения.
        /// </summary>
        /// <param name="input">Текстовое представление числа, которое нужно преобразовать.</param>
        /// <param name="result">Результат преобразования. Если преобразование успешно, содержит значение типа double; в противном случае - 0.</param>
        /// <returns>Возвращает true, если преобразование прошло успешно; в противном случае - false.</returns>
        private bool TryParseDouble(string input, out double result)
        {
            // Комбинируем стили для разрешения десятичной точки и лидирующего знака
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

            // Пытаемся распознать число с использованием инвариантной культуры (разделитель - точка)
            if (double.TryParse(input, styles, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }

            // Если не получилось, пытаемся распознать число с использованием текущей культуры (разделитель - запятая)
            if (double.TryParse(input, styles, CultureInfo.CurrentCulture, out result))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Преобразует список объектов в список словарей, где ключом является название свойства, а значением - строковое представление значения этого свойства.
        /// </summary>
        /// <typeparam name="T">Тип объектов в списке.</typeparam>
        /// <param name="list">Список объектов для преобразования.</param>
        /// <returns>Список словарей с названиями и значениями свойств объектов.</returns>
        private static List<Dictionary<string, string>> ConvertListToDictionaryList<T>(List<T> list)
        {
            var result = new List<Dictionary<string, string>>();

            foreach (var item in list)
            {
                var dict = new Dictionary<string, string>();
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item)?.ToString() ?? string.Empty;
                    dict.Add(prop.Name, value);
                }

                result.Add(dict);
            }

            return result;
        }

        private static void RemoveKeyFromDictionaries(ref List<Dictionary<string, string>> dict, string keyToRemove)
        {
            foreach (var dictionary in dict)
            {
                if (dictionary.ContainsKey(keyToRemove))
                {
                    dictionary.Remove(keyToRemove);
                }
            }
        }

        /// <summary>
        /// Извлекает существующую строку из предоставленного объекта SheetData по указанному индексу строки, или создает и добавляет новую строку, если она еще не существует.
        /// </summary>
        /// <param name="sheetData">Объект SheetData, содержащий строки листа.</param>
        /// <param name="rowIndex">Индекс строки для извлечения или создания.</param>
        /// <returns>Существующая или вновь созданная строка Row.</returns>
        private Row GetOrCreateRow(SheetData sheetData, uint rowIndex)
        {
            var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }
            return row;
        }

        /// <summary>
        /// Заполняет ячейку в указанной строке по заданному индексу столбца и индексу строки пустым значением, если она еще не существует.
        /// </summary>
        /// <param name="row">Объект Row, в котором необходимо заполнить ячейку.</param>
        /// <param name="columnIndex">Индекс столбца, в котором должна быть размещена ячейка.</param>
        /// <param name="rowIndex">Индекс строки, в которой должна быть размещена ячейка.</param>
        private void FillEmptyCell(Row row, int columnIndex, int rowIndex)
        {
            string cellReference = GetCellReference(columnIndex, rowIndex);
            if (row.Elements<Cell>().All(c => c.CellReference != cellReference))
            {
                Cell emptyCell = new Cell()
                {
                    CellReference = cellReference,
                    DataType = CellValues.String,
                    CellValue = new CellValue(string.Empty)
                };
                row.Append(emptyCell);
            }
        }

        /// <summary>
        /// Генерирует строку ссылки на ячейку для заданных индексов столбца и строки.
        /// </summary>
        /// <param name="columnIndex">Индекс столбца (начинается с 1).</param>
        /// <param name="rowIndex">Индекс строки (начинается с 1).</param>
        /// <returns>Строка ссылки на ячейку в формате "A1", "B2" и т.д.</returns>
        private string GetCellReference(int columnIndex, int rowIndex)
        {
            string columnName = "";
            while (columnIndex > 0)
            {
                int modulo = (columnIndex - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnIndex = (columnIndex - modulo) / 26;
            }

            return columnName + rowIndex.ToString();
        }

        private uint GetColumnIndex(string cellReference)
        {
            string columnName = Regex.Replace(cellReference, @"\d", "");
            uint index = 0;
            foreach (char ch in columnName)
            {
                index = (index * 26) + (uint)(ch - 'A' + 1);
            }
            return index;
        }

        #endregion

        /// <summary>
        /// Метод для замены недопустимых символов
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SanitizeFileName(string fileName)
        {
            // Определяем недопустимые символы
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            // Создаем регулярное выражение для поиска недопустимых символов
            string pattern = "[" + Regex.Escape(invalidChars) + "]";

            // Заменяем недопустимые символы на _
            return Regex.Replace(fileName, pattern, "_");
        }

        /// <summary>
        /// Проверяет длину строки и обрезает её до 30 символов, если она превышает этот предел.
        /// </summary>
        /// <param name="name">Строка, которую нужно проверить и, при необходимости, изменить.</param>
        /// <returns>Исходная строка, если её длина меньше 30 символов, или обрезанная строка длиной 30 символов.</returns>
        public string CheckAndChangeListName(string name)
        {
            if (name.Count() < 30)
            {
                return name;
            }
            else
            {
                return name[0..30];
            }
        }
    }

    /// <summary>
    /// Стилистический класс
    /// </summary>
    public class StyleExcel
    {
        /// <summary>
        /// Страница, к которой присваиваются стили.
        /// </summary>
        public string SheetName { get; private set; }
        /// <summary>
        /// Словарь, где ключи — это столбцы, а значения — это его размеры.
        /// </summary>
        public Dictionary<int, double> ColumnWidths { get; private set; } = null;
        /// <summary>
        /// Перенос текста по словам
        /// </summary>
        public bool WrapText { get; private set; } = false;

        public StyleExcel() { }
        public StyleExcel(string sheetName, Dictionary<int, double> columnWidths, bool wrapText)
        {
            SheetName = sheetName;
            ColumnWidths = columnWidths;
            WrapText = wrapText;
        }
    }
}
