using BluckImport.Core.ClsResponce;
using BulkImport.Core.Common.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkImport.Core.Interface
{
    public interface IQuestionRepository
    {
        Task<Response<QuestionClass>> QuestionList(QuestionTypeRequest questionType);
        Task<Response<QuestionClass>> QuestionGet();
    }
}
