using BluckImport.Core.ClsResponce;
using BluckImport.Core.Interface;
using BluckImport.Core.Model;
using BulkImport.Core.Common;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace BluckImport.Infrastructure
{
    public class BulkImportRepository : IBulkImportRepository
    {
        private readonly IConfiguration _configuration;
        private static string con = string.Empty;

        public BulkImportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            con = _configuration["ConnectionStrings:DataConnect"];
        }
        public static IDbConnection connection
        {
            get
            {
                return new SqlConnection(con);
            }
        }
        public async Task<Response<EmpoyeeInsertList>> Add(BulkImportData bulkImport)
        {
            Response<EmpoyeeInsertList> responce = new Response<EmpoyeeInsertList>();
            string json;
            if (bulkImport.ImportFile == null || bulkImport.ImportFile.Length == 0)
            {
                responce.Message = "No file provided";
                return responce;
            }

            using (var stream = bulkImport.ImportFile.OpenReadStream())
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet.
                var data = new DataTable();
                var validationErrors = new List<string>();
                // Assuming the first row contains column headers.
                foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    data.Columns.Add(firstRowCell.Text);
                }

                // Load the data from the Excel sheet into the DataTable.
                //for (var rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
                //{
                //    var worksheetRow = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                //    var dataRow = data.NewRow();
                //    var rowData = new EmpoyeeInsert();
                //    foreach (var cell in worksheetRow)
                //    {
                //        dataRow[cell.Start.Column - 1] = cell.Text;
                //    }
                //    data.Rows.Add(dataRow);
                //}
                for (var rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
                {
                    var worksheetRow = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                    var dataRow = data.NewRow();
                    var rowData = new EmpoyeeInsert();

                    for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        var cell = worksheet.Cells[rowNumber, col];
                        var columnName = worksheet.Cells[1, col].Text;
                        var cellValue = cell.Text;
                        dataRow[cell.Start.Column - 1] = cellValue;


                        #region Comment if code 

                        // Add validation logic for specific columns.
                        //if (columnName == "FirstName" && !Utility.IsValidFirstName(cellValue))
                        //{
                        //    validationErrors.Add($"Invalid First Name in row {rowNumber}: {cellValue}");
                        //}
                        //if (columnName == "LastName" && !Utility.IsValidLastName(cellValue))
                        //{
                        //    validationErrors.Add($"Invalid LastName in row {rowNumber}: {cellValue}");
                        //}
                        //if (columnName == "Aderess" && !Utility.IsValidAderess(cellValue))
                        //{
                        //    validationErrors.Add($"Invalid Address in row {rowNumber}: {cellValue}");
                        //}
                        //if (columnName == "Gender" && !Utility.IsValidGender(cellValue))
                        //{
                        //    validationErrors.Add($"Invalid Gender in row {rowNumber}: {cellValue}");
                        //}
                        //if (columnName == "RoleId" && !Utility.IsValidRoleId(cellValue))
                        //{
                        //    validationErrors.Add($"Invalid Role Name in row {rowNumber}: {cellValue}");
                        //}
                        //if (columnName == "Age" && !int.TryParse(cellValue, out int age))
                        //{
                        //    validationErrors.Add($"Invalid Age in row {rowNumber}: {cellValue}");
                        //}
                        //if (columnName == "DOB" && !DateTime.TryParseExact(cellValue, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                        //{
                        //    validationErrors.Add($"Invalid DOB in row {rowNumber}: {cellValue}");
                        //}
                        //if (columnName == "EmailID" && !Utility.IsValidEmail(cellValue))
                        //{
                        //    validationErrors.Add($"Invalid EmailID in row {rowNumber}: {cellValue}");
                        //}
                        #endregion

                        switch (columnName)
                        {
                            case "FirstName":
                                if (!Utility.IsValidFirstName(cellValue))
                                    validationErrors.Add($"Invalid First Name in row {rowNumber}: {cellValue}");
                                break;

                            case "LastName":
                                if (!Utility.IsValidLastName(cellValue))
                                    validationErrors.Add($"Invalid LastName in row {rowNumber}: {cellValue}");
                                break;

                            case "Aderess":
                                if (!Utility.IsValidAderess(cellValue))
                                    validationErrors.Add($"Invalid Address in row {rowNumber}: {cellValue}");
                                break;

                            case "Gender":
                                if (!Utility.IsValidGender(cellValue))
                                    validationErrors.Add($"Invalid Gender in row {rowNumber}: {cellValue}");
                                break;

                            case "RoleId":
                                if (!Utility.IsValidRoleId(cellValue))
                                    validationErrors.Add($"Invalid Role Name in row {rowNumber}: {cellValue}");
                                break;

                            case "Age":
                                if (!int.TryParse(cellValue, out int age))
                                    validationErrors.Add($"Invalid Age in row {rowNumber}: {cellValue}");
                                break;

                            case "DOB":
                                if (!DateTime.TryParseExact(cellValue, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                                    validationErrors.Add($"Invalid DOB in row {rowNumber}: {cellValue}");
                                break;

                            case "EmailID":
                                if (!Utility.IsValidEmail(cellValue))
                                    validationErrors.Add($"Invalid EmailID in row {rowNumber}: {cellValue}");
                                break;


                            default:
                               
                                break;
                        }


                        // Map data to the EmpoyeeInsert object for further use.
                        Utility.MapData(rowData, columnName, cellValue);
                    }

                    // Validate the entire row and add custom validation logic if needed.
                    if (Utility.IsValidRow(rowData))
                    {
                        data.Rows.Add(dataRow);
                    }
                    else
                    {
                        validationErrors.Add($"Invalid data in row {rowNumber}");
                    }
                }
                if (validationErrors.Count() > 0)
                {
                   var errorResponse = new Response<EmpoyeeInsertList>
                    {
                        Errors = validationErrors,
                    };

                    return errorResponse;
                }

                // Convert the DataTable to JSON.
                json = JsonConvert.SerializeObject(data, Formatting.Indented);

                
            }
            List<EmpoyeeInsert> personList = JsonConvert.DeserializeObject<List<EmpoyeeInsert>>(json);

            using (IDbConnection db = connection)
            {
                // Create a DataTable to match the TVP structure
                var tvp = new DataTable();
                tvp.Columns.Add("FirstName", typeof(string));
                tvp.Columns.Add("MiddleName", typeof(string));
                tvp.Columns.Add("LastName", typeof(string));
                tvp.Columns.Add("Age", typeof(int));
                tvp.Columns.Add("DOB", typeof(string)); // Use DateTime for DOB
                tvp.Columns.Add("EmailID", typeof(string));
                tvp.Columns.Add("Address", typeof(string));
                tvp.Columns.Add("RoleID", typeof(string));
                tvp.Columns.Add("Gender", typeof(string));
                tvp.Columns.Add("Skill", typeof(string));
                // Populate the DataTable with data from personList
                foreach (var person in personList)  
                {

                    tvp.Rows.Add(person.FirstName, person.MiddleName, person.LastName, person.Age, person.DOB, person.EmailID, person.Aderess, person.RoleID, person.Gender, person.Skill);
                }

                // Define a dynamic parameter to pass the TVP
                var parameters = new DynamicParameters();
                parameters.Add("BluckEmployeeData", tvp.AsTableValuedParameter("BluckEmployeeInsert"));
                parameters.Add("CreatedBy", "Ankush");
                // Call the stored procedure
                var affectedRows = await db.QueryMultipleAsync("[dbo].[uspEmployeeInsert]", parameters, commandType: CommandType.StoredProcedure);

                // Update response based on the result
                
                Response<EmpoyeeInsertList> response = affectedRows.Read<Response<EmpoyeeInsertList>>().FirstOrDefault();
                
                if (response != null && !response.Status)
                {
                    var data = affectedRows.Read<EmpoyeeInsertList>().ToList();
                    response.Data = data;
                    return response;
                }
            }

            return responce;
        }

        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                dt.Columns.Add(property.Name);
            }
            if (items != null && items.Count > 0)
            {
                foreach (T item in items)
                {
                    var values = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        values[i] = properties[i].GetValue(item, null);
                    }
                    dt.Rows.Add(values);
                }
            }
            return dt;
        }




    }

    
}
