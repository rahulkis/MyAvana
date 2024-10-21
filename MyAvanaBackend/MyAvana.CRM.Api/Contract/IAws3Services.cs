using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IAws3Services
    {
        Task<bool> UploadFileAsync(List<IFormFile> files, int ProductEntityId);
        bool DeleteFileAsync(string fileName, int id, string versionId = "");
    }
}
