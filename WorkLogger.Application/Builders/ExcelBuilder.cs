using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace WorkLogger.Application.Builders;

public static class ExcelBuilder
{
    public static MemoryStream BuildExcelFile(Dictionary<string, List<string>> data)
    {
        var memoryStream = new MemoryStream();

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var workSheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            workSheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
            
            var sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(workSheetPart), SheetId = 1, Name = "Generated Sheet" };
            sheets.Append(sheet);
            
            var headerRow = new Row();
            foreach (var column in data.Keys)
            {
                var cell = new Cell()
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(column)
                };
                headerRow.Append(cell);
            }
            sheetData.Append(headerRow);

            var maxRowCount = FindMaxRowCount(data);
            for (var i = 0; i < maxRowCount; i++)
            {
                var row = new Row();
                foreach (var columnData in data.Values)
                {
                    var cellValue = (columnData.Count > i) ? columnData[i] : string.Empty;
                    Cell cell = new Cell()
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(cellValue)
                    };
                    row.Append(cell);
                }
                sheetData.Append(row);
            }
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
    
    private static int FindMaxRowCount(Dictionary<string, List<string>> data)
    {
        int maxRowCount = 0;
        foreach (var list in data.Values)
        {
            if (list.Count > maxRowCount)
            {
                maxRowCount = list.Count;
            }
        }
        return maxRowCount;
    }
}