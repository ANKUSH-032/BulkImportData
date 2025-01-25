using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluckImport.Core.Model;
using System.Drawing;

namespace BulkImport.Core.Common.Import
{
    public static class ImportExcel
    {
        public static async Task<ExcelWorksheet> EmployeeErrorGetExelFIle(List<EmpoyeeInsertList> data, ExcelPackage package, string excelname)
        {
            // add a new worksheet to the empty workbook
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(excelname);


            int counter = 1;

            int colHeaderCounter = 1;

            worksheet.Cells[counter, colHeaderCounter++].Value = "First Name";
            worksheet.Cells[counter, colHeaderCounter++].Value = "Middle Name";
            worksheet.Cells[counter, colHeaderCounter++].Value = "Last Name";
            worksheet.Cells[counter, colHeaderCounter++].Value = "Age";
            worksheet.Cells[counter, colHeaderCounter++].Value = "DOB";
            worksheet.Cells[counter, colHeaderCounter++].Value = "EmailID";
            worksheet.Cells[counter, colHeaderCounter++].Value = "Aderess";
            worksheet.Cells[counter, colHeaderCounter++].Value = "RoleID";
            worksheet.Cells[counter, colHeaderCounter++].Value = "Gender";
            worksheet.Cells[counter, colHeaderCounter++].Value = "Skill";
            worksheet.Cells[counter, colHeaderCounter++].Value = "IsDuplicate";


            worksheet.Cells[1, 1, 1, colHeaderCounter + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            worksheet.Cells[1, 1, 1, colHeaderCounter + 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            using (var colorrange = worksheet.Cells[counter, 1, counter, colHeaderCounter - 1])
            {
                colorrange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                colorrange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
            }


            int i = counter;
            foreach (var row in data)
            {
                i = i + 1;
                int colDataCounter = 1;

                worksheet.Cells[i, colDataCounter++].Value = row.FirstName;
                worksheet.Cells[i, colDataCounter++].Value = row.MiddleName;
                worksheet.Cells[i, colDataCounter++].Value = row.LastName;
                worksheet.Cells[i, colDataCounter++].Value = row.Age;
                worksheet.Cells[i, colDataCounter++].Value = row.DOB;
                worksheet.Cells[i, colDataCounter++].Value = row.EmailID;
                worksheet.Cells[i, colDataCounter++].Value = row.Aderess;
                worksheet.Cells[i, colDataCounter++].Value = row.RoleID;
                worksheet.Cells[i, colDataCounter++].Value = row.Gender;
                worksheet.Cells[i, colDataCounter++].Value = row.Skill;
                if (row.IsDuplicate.ToLower() == "yes")
                {
                    //var cell = worksheet.Cells[i, colDataCounter++];
                    //cell.Value = row.IsDuplicate;
                    //cell.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    var cell = worksheet.Cells[i, colDataCounter++];
                    cell.Value = row.IsDuplicate;
                    cell.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                }
                else
                {
                    worksheet.Cells[i, colDataCounter++].Value = row.IsDuplicate;
                }


            }

            using (var range = worksheet.Cells[counter, 1, counter, 8]) { range.Style.Font.Bold = true; }

            // Change the sheet view to show it in page layout mode
            worksheet.View.PageLayoutView = false;

            return worksheet;
        }
        public static async Task<ExcelWorksheet> CreateDynamicExcelFile<T>(
                                            List<T> data,
                                            ExcelPackage package,
                                            string sheetName,
                                            Dictionary<string, string> columnMappings,
                                            System.Drawing.Color headerBackgroundColor,
                                            System.Drawing.Color highlightColor,
                                            Func<T, bool>? highlightCondition = null, // Optional custom condition for highlighting
                                            bool pageLayoutView = false)
        {
            // Add a new worksheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

            int rowCounter = 1; // Start with the first row
            int colCounter = 1; // Start with the first column

            // Create column headers dynamically based on columnMappings
            foreach (var header in columnMappings.Values)
            {
                worksheet.Cells[rowCounter, colCounter++].Value = header;
            }

            // Style headers
            using (var headerRange = worksheet.Cells[rowCounter, 1, rowCounter, columnMappings.Count])
            {
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(headerBackgroundColor);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }

            // Populate data rows dynamically
            foreach (var item in data)
            {
                rowCounter++;
                colCounter = 1;

                foreach (var property in columnMappings.Keys)
                {
                    var propertyValue = typeof(T).GetProperty(property)?.GetValue(item, null);
                    worksheet.Cells[rowCounter, colCounter++].Value = propertyValue;
                }

                // Apply highlighting conditionally
                if (highlightCondition != null && highlightCondition(item))
                {
                    using (var highlightRange = worksheet.Cells[rowCounter, 1, rowCounter, columnMappings.Count])
                    {
                        highlightRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        highlightRange.Style.Fill.BackgroundColor.SetColor(highlightColor);
                    }
                }
            }

            // Optional: Set worksheet view
            worksheet.View.PageLayoutView = pageLayoutView;

            // Return the worksheet for further customization
            return worksheet;
        }

    }
}
