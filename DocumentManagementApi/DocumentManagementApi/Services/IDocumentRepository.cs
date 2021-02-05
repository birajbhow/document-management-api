using DocumentManagementApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementApi.Services
{
    public interface IDocumentRepository
    {
        Task<bool> Process(IFormFile formFile);
        Task<IEnumerable<Document>> GetAll();
        Task Delete(AppFile appFile);
        Task<AppFile> Find(string documentId);
    }
}
