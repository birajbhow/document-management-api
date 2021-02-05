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
    public class FileProcessor : IFileProcessor
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FileProcessor> _logger;

        public FileProcessor(AppDbContext context, ILogger<FileProcessor> logger)
        {
            _context = context;
            _logger = logger;
        }

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
                    $"{nameof(FileProcessor)}: Error occurred processing the file {formFile.FileName}");                
            }

            return false;
        }
    }
}
