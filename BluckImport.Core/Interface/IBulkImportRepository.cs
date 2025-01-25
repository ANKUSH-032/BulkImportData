using BluckImport.Core.ClsResponce;
using BluckImport.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BluckImport.Core.Interface
{
    public interface IBulkImportRepository
    {
        Task<Response<EmpoyeeInsertList>> Add(BulkImportData bulkImport);
        Task<Response<InsertQuestion>> AddQuestion(BulkImportData bulkImport);

    }
}
