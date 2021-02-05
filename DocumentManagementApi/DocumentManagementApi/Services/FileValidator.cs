using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementApi.Services
{
    public class FileValidator : IFileValidator
    {
        private readonly int _sizeLimit;
        private readonly string _permittedExtensions;

        public FileValidator(IConfiguration configuration)
        {
            _sizeLimit = configuration.GetValue<int>("FileSizeLimitInBytes");
            _permittedExtensions = configuration.GetValue<string>("PermittedFileExtensioins");
        }
        public bool IsValid(IFormFile formFile)
        {
            return IsNotNull(formFile)
                && ValidateFileSize(formFile)
                && ValidateFileExtension(formFile);
        }

        private bool IsNotNull(IFormFile formFile) => formFile != null;

        private bool ValidateFileSize(IFormFile formFile)
            => formFile.Length > 0 && formFile.Length <= _sizeLimit;

        private bool ValidateFileExtension(IFormFile formFile)
        {
            var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            return !string.IsNullOrEmpty(ext) 
                && _permittedExtensions.Split(",").Contains(ext);
        }
    }
}
