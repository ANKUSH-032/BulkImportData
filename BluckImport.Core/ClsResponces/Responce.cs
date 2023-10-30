using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluckImport.Core.ClsResponce
{
    public class Responce
    {

        public string? Message { get; set; }
        public bool? Status { get; set; }
        public string? Data { get; set; }

    }

    public class Response<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
        public long? TotalRecords { get; set; } = 0;
        public long recordsFiltered { get; set; }
        public string draw { get; set; } = "0";
        public List<string> Errors { get; set; } 
    }

    public class ErrorResponse<T>
    {
        public List<string> Errors { get; set; }
        public string? Message { get; set; }
        public bool? Status { get; set; }
    }
}
