using BluckImport.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BulkImport.Core.Common
{
    public class Utility
    {
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

        public static bool IsValidEmail(string email)
        {
            // Implement your email validation logic here.
            return Regex.IsMatch(email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        public static bool IsValidFirstName(string firstName)
        {
            // Implement your email validation logic here.
            return Regex.IsMatch(firstName, @"^[A-Za-z]*$");
        }
        public static bool IsValidLastName(string lastName)
        {
            // Implement your email validation logic here.
            return Regex.IsMatch(lastName, @"^[A-Za-z]*$");
        }
        public static bool IsValidAderess(string address)
        {
            // Implement your email validation logic here.
            return Regex.IsMatch(address, @"^[0-9A-Za-z ,.-]*$");
        }
        public static bool IsValidGender(string gender)
        {
            // Implement your email validation logic here.
            return Regex.IsMatch(gender, @"^(Male|Female)$");
        }
        public static bool IsValidRoleId(string roleNme)
        {
            // Implement your email validation logic here.
            return Regex.IsMatch(roleNme, @"^(SuperAdmin|Admin)$");
        }

        public static void MapData(EmpoyeeInsert rowData, string columnName, string cellValue)
        {
            // Implement mapping logic from Excel columns to EmpoyeeInsert properties.
            // Example: 
            switch (columnName)
            {
                case "FirstName":
                    rowData.FirstName = cellValue;
                    break;
                case "MiddleName":
                    rowData.MiddleName = cellValue;
                    break;
                case "LastName":
                    rowData.LastName = cellValue;
                    break;
                case "RoleID":
                    rowData.RoleID = cellValue;
                    break;
                case "Gender":
                    rowData.Gender = cellValue;
                    break;
                case "Aderess":
                    rowData.Aderess = cellValue;
                    break;
                case "DOB":
                    rowData.DOB = cellValue;
                    break;
                case "EmailID":
                    rowData.EmailID = cellValue;
                    break;
                case "Age":
                    rowData.Age = int.Parse(cellValue);
                    break;
                case "Skill":
                    rowData.Skill = cellValue;
                    break;
                    // Add more cases for other columns.
            }
        }

        public static bool IsValidRow(EmpoyeeInsert rowData)
        {
            // Implement additional row-level validation logic if needed.
            // Example: return false if required fields are missing.
            // You can also add more specific validation checks here.
            return !string.IsNullOrWhiteSpace(rowData.FirstName) 
                && !string.IsNullOrWhiteSpace(rowData.LastName) 
                && !string.IsNullOrWhiteSpace(rowData.EmailID) 
                && !string.IsNullOrWhiteSpace(rowData.Aderess) 
                && !string.IsNullOrWhiteSpace(rowData.DOB) 
                && !string.IsNullOrWhiteSpace(rowData.RoleID)
                && !string.IsNullOrWhiteSpace(rowData.Gender);
        }


        public static void MapQuestionData(InsertQuestion rowData, string columnName, string cellValue)
        {
            // Implement mapping logic from Excel columns to EmpoyeeInsert properties.
            // Example: 
            switch (columnName)
            {
                case "Question":
                    rowData.Question = cellValue;
                    break;
                case "QuestionType":
                    rowData.QuestionType = cellValue;
                    break;
                //case "LastName":
                //    rowData.LastName = cellValue;
                //    break;
          
            }
        }

        public static bool IsValidQuestionRow(InsertQuestion rowData)
        {
            // Implement additional row-level validation logic if needed.
            // Example: return false if required fields are missing.
            // You can also add more specific validation checks here.
            return !string.IsNullOrWhiteSpace(rowData.Question)
                && !string.IsNullOrWhiteSpace(rowData.QuestionType);
      
        }
        public static bool IsValidQuestion(string question)
        {
            // Implement your email validation logic here.
            return Regex.IsMatch(question, @"^.*$");
        }
        public static bool IsValidQuestionType(string questionType)
        {
            // Implement your email validation logic here.
            return Regex.IsMatch(questionType, @"^.*$");
        }
    }
}
