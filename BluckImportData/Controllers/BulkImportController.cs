using BluckImport.Core.ClsResponce;
using BluckImport.Core.Interface;
using BluckImport.Core.Model;
using BulkImport.Core.Common.Import;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace BluckImportData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkImportController : ControllerBase
    {
        private readonly IBulkImportRepository _bulkImportRepository;
        private readonly IConfiguration _configuration;
        public BulkImportController(IBulkImportRepository bulkImportRepository, IConfiguration configuration)
        {
            _bulkImportRepository = bulkImportRepository;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> InsertBulkData([FromForm] BulkImportData bulkImport)
        {

            string fileName = Path.GetFileName(bulkImport.ImportFile.FileName);
            string fileExtension = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
            {
                if (fileExtension.ToLower() !=".xlsx")
                {
                    return BadRequest(new
                    {
                        Status = false,
                        Message = "Only Accept .xlsx file"


                    });
                }
            }
            var response = await _bulkImportRepository.Add(bulkImport);
            if (response.Data == null && response.Errors != null && response.Errors.Count > 0)
            {
                var errorResponse = new ErrorResponse<EmpoyeeInsertList>
                {
                    Status = false,
                    Message = "Validation Errors",
                    Errors = response.Errors
                };

                return BadRequest(errorResponse);
            }
            else
            {
                var dataForExcel = response.Data;
                if (dataForExcel != null && dataForExcel.Count() == 0)
                {
                    return BadRequest(new
                    {
                        Status = false,
                        Message = "No Data Found"
                    });
                }
                if (response != null && !(bool)response.Status)
                {
                    byte[] byteArrayForFileConversion;
                    string fileType = string.Empty, documentName = string.Empty;
                    string exportType = "EXCEL";
                    switch (exportType) 
                    {
                        case "EXCEL":
                            ExcelPackage.LicenseContext = LicenseContext.Commercial;
                            using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
                            {
                                await ImportExcel.EmployeeErrorGetExelFIle(dataForExcel, package, fileName).ConfigureAwait(false);
                                byteArrayForFileConversion = package.GetAsByteArray();
                                fileType = "application/ms-excel";
                                documentName = "EmployeeError.xlsx";
                            }
                            break;

                        default:
                            return BadRequest(new
                            {
                                Status = false,
                                Message = "Download type can only be EXCEL OR CSV!"
                            });
                    }
                    return File(byteArrayForFileConversion, fileType, documentName);
                }
            }
            return Ok(response);
        }
    }
}
