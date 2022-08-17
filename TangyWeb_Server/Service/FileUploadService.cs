using Microsoft.AspNetCore.Components.Forms;
using TangyWeb_Server.Service.IService;

namespace TangyWeb_Server.Service
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public bool DeleteFile(string filePath)
        {
            string fullFilePath = _webHostEnvironment.WebRootPath + filePath;
            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
                return true;
            }
            return false;
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            FileInfo fileInfo = new FileInfo(file.Name);
            var filename = Guid.NewGuid().ToString() + fileInfo.Extension;
            var folderDirectory = @$"{_webHostEnvironment.WebRootPath}/images/product";
            if (!Directory.Exists(folderDirectory))
            {
                Directory.CreateDirectory(folderDirectory);
            }
            var filePath = Path.Combine(folderDirectory, filename);
            await using var fs = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream(51200000).CopyToAsync(fs);
            var fullPath = @$"/images/product/{filename}";
            return fullPath;
        }
    }
}
