using Microsoft.AspNetCore.Http;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string folderName);
        Task DeleteAsync(string fileUrl);
    }
}
