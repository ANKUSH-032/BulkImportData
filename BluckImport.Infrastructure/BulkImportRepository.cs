using BluckImport.Core.ClsResponce;
using BluckImport.Core.Interface;
using BluckImport.Core.Model;
using BulkImport.Core.Common;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
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
        public async Task<Responce> Add(BulkImportData bulkImport)
        {
            Responce responce = new Responce();
            string json;
            if (bulkImport.ImportFile == null || bulkImport.ImportFile.Length == 0)
            {
                // Handle the case where no file is provided.
                responce.Message = "No file provided";
                return responce;
            }

            using (var stream = bulkImport.ImportFile.OpenReadStream())
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet.
                var data = new DataTable();
                // Assuming the first row contains column headers.
                foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    data.Columns.Add(firstRowCell.Text);
                }

                // Load the data from the Excel sheet into the DataTable.
                for (var rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
                {
                    var worksheetRow = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                    var dataRow = data.NewRow();
                    foreach (var cell in worksheetRow)
                    {
                        dataRow[cell.Start.Column - 1] = cell.Text;
                    }
                    data.Rows.Add(dataRow);
                }

                // Convert the DataTable to JSON.
                json = JsonConvert.SerializeObject(data, Formatting.Indented);

                // Write the JSON to a file.
                File.WriteAllText("outer.json", json);
                Console.WriteLine("Data has been converted to JSON and saved to outer.json");
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
                tvp.Columns.Add("DOB", typeof(DateTime)); // Use DateTime for DOB
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
                responce = affectedRows.Read<Responce>().FirstOrDefault();
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
