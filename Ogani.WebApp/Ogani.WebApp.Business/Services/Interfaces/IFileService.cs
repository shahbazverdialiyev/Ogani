using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string folderName);
        Task DeleteAsync(string fileUrl);
    }
}
