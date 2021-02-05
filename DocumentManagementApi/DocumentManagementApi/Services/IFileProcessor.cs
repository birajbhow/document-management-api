using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementApi.Services
{
    public interface IFileProcessor
    {
        Task<bool> Process(IFormFile formFile);
    }
}
