using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluckImport.Core.Model
{
    public class BulkImportData
    {
        public IFormFile? ImportFile { get; set; }
    }
    public class EmpoyeeInsert
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? DOB { get; set; }
        public string? EmailID { get; set; }
        public string? Aderess { get; set; }
        public string? CreatedBy { get; set; }
       // public string? CreatedOn { get; set; }
    }
}
