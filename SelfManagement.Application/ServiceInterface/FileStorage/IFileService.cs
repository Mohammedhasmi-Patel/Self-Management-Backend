

using Microsoft.AspNetCore.Http;
using SelfManagement.Application.DTO.Common.Filestorage;

namespace SelfManagement.Application.ServiceInterface.FileStorage
{
    public interface IFileService
    {
        Task<FileMetadataDto> SaveFileAsync(IFormFile file, string subDirectory, string[] allowedExtensions,long maxSizeBytes);
        void DeleteFile(string filePath);
    }
}
