using BluckImport.Core.Interface;
using BluckImport.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            return Ok(response);
        }
    }
}
