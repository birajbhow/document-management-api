using DocumentManagementApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagementApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IFileValidator _fileValidator;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(IFileValidator fileValidator, 
            IDocumentRepository documentRepository, ILogger<DocumentController> logger)
        {
            _fileValidator = fileValidator;
            _documentRepository = documentRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            if (!_fileValidator.IsValid(formFile))
            {
                return BadRequest("Invalid file!");
            }

            var processed = await _documentRepository.Process(formFile);
            return processed
                ? Created(nameof(DocumentController), "File uploaded successfully.")
                : Problem("Error occurred!", statusCode: 500);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var docs = await _documentRepository.GetAll();
                return Ok(docs);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred fetching list of documents");
            }
            return Problem("Error occurred fetching document list!", statusCode: 500);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string docuementId)
        {
            try
            {
                var appFile = await _documentRepository.Find(docuementId);
                if (appFile == null)
                {
                    return BadRequest("Document not found!");
                }

                await _documentRepository.Delete(appFile);
                return Ok("Document deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during deleting a document {docuementId}");
            }
            return Problem("Error occurred deleting a document!", statusCode: 500);
        }

        [HttpGet]
        public async Task<IActionResult> Download(string documentId)
        {
            try
            {
                var appFile = await _documentRepository.Find(documentId);
                if (appFile == null)
                {
                    return BadRequest("Document not found!");
                }
                return File(appFile.Content, appFile.ContentType, appFile.EncodedName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred downloading document {documentId}");
            }
            return Problem("Error occurred downloading document!", statusCode: 500);
        }
    }
}
