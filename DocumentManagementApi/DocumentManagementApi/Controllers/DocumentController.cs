using DocumentManagementApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DocumentManagementApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IFileValidator _fileValidator;
        private readonly IFileProcessor _fileProcessor;
        
        public DocumentController(IFileValidator fileValidator, 
            IFileProcessor fileProcessor)
        {
            _fileValidator = fileValidator;
            _fileProcessor = fileProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            if (!_fileValidator.IsValid(formFile))
            {
                return BadRequest("Invalid file!");
            }

            var processed = await _fileProcessor.Process(formFile);
            return processed
                ? Created(nameof(DocumentController), "File uploaded successfully.")
                : Problem("Error occurred!", statusCode: 500);
        }
    }
}
