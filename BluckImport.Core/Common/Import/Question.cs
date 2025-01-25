using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkImport.Core.Common.Import
{
    public class QuestionClass
    {
        public long Id { get; set; }
        public string? Question {  get; set; }
        public string? QuestionType {  get; set; }

    }
    public class QuestionTypeRequest
    {
        public string? QuestionType { get; set; }
    }
}
