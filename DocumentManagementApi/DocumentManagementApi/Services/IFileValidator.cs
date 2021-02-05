using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementApi.Services
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile formFile);
    }
}
