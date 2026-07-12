
using Microsoft.AspNetCore.Http;
using SelfManagement.Application.DTO.Common.Filestorage;

namespace SelfManagement.Application.ServiceInterface.FileStorage
{
    public interface IFileService
    {
        public Task<FileUploadResult> UploadFileAsync(IFormFile file, string folderName, string[] allowedExtensions, int maxSizeInMb);
        Task<FileUploadResult> UpdateFileAsync(IFormFile file, string existingFilePath, string folderName, string[] allowedExtensions, int maxSizeInMb);
        Task<FileDeleteResult> DeleteFileAsync(string filePath);
    }
}
