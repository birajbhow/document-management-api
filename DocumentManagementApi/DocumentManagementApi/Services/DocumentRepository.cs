using DocumentManagementApi.Data;
using DocumentManagementApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DocumentManagementApi.Services
{
    /// <summary>
    /// This repository manages all the operations regarding document management
    /// </summary>
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DocumentRepository> _logger;

        public DocumentRepository(AppDbContext context, ILogger<DocumentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Delete a document object from In-Memory DB
        /// </summary>
        /// <param name="appFile"></param>
        /// <returns></returns>
        public async Task Delete(AppFile appFile)
        {
            _context.File.Remove(appFile);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Find a document object by id from In-Memory DB
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public async Task<AppFile> Find(string documentId)
        {
            return await Task.Run(() => _context.File.FirstOrDefault(f => f.Id == documentId));
        }

        /// <summary>
        /// Returns summary information about all the documents
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Document>> GetAll()
        {
            return await Task.Run(() => _context.File.Select(f => new Document
            {
                Id = f.Id,
                Name = f.EncodedName,
                Size = f.Size
            }).AsEnumerable());
        }

        /// <summary>
        /// Reads the file content and store it in an object in an In-Memory DB
        /// </summary>
        /// <param name="formFile">File content sent with Request</param>
        /// <returns>true, if processed successfully else false</returns>
        public async Task<bool> Process(IFormFile formFile)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await formFile.CopyToAsync(memoryStream);

                var file = new AppFile
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = memoryStream.ToArray(),
                    ContentType = formFile.ContentType,
                    EncodedName = WebUtility.HtmlEncode(formFile.FileName),
                    Size = formFile.Length,
                    UploadDate = DateTime.UtcNow
                };

                _context.File.Add(file);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    $"{nameof(DocumentRepository)}: Error occurred processing the file {formFile.FileName}");                
            }

            return false;
        }
    }
}
