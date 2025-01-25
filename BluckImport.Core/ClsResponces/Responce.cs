using BluckImport.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluckImport.Core.ClsResponce
{
    public class Response
    {

        public string? Message { get; set; }
        public bool? Status { get; set; }
        public string? Data { get; set; }
        public List<string> Errors { get; set; }
    }

    public class Response<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public List<T>? Data { get; set; }
        public long? TotalRecords { get; set; } = 0;
        public long recordsFiltered { get; set; }
        public string draw { get; set; } = "0";
        public List<string>? Errors { get; set; } 
    }

    public class ErrorResponse<T>
    {
        public List<string>? Errors { get; set; }
        public string? Message { get; set; }
        public bool? Status { get; set; }
    }
    public class ResponseModel
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public List<InsertQuestion>? Data { get; set; } // Optional: if you want to return the inserted data
    }
}
