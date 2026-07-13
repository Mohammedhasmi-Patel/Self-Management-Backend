
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SelfManagement.Application.DTO.Common.Filestorage;
using SelfManagement.Application.ServiceInterface.FileStorage;

namespace SelfManagement.Application.Services.FileStorage
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folderName, string[] allowedExtensions, int maxSizeInMb)
        {
            var result = new FileUploadResult();

            if(file == null || file.Length == 0)
            {
                result.ErrorMessage = "File is empty or null.";
                return result;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                result.IsSuccess = false;
                result.ErrorMessage = $"Invalid file extension. Allowed extensions are: {string.Join(", ", allowedExtensions)}";
                return result;
            }

            long maxBytes = maxSizeInMb * 1024 * 1024;
            if (file.Length > maxBytes)
            {
                result.IsSuccess = false;
                result.ErrorMessage = $"File size exceeds the maximum limit of {maxSizeInMb} MB.";
                return result;
            }

            const string rootFolder = "uploads";
            var uploadsFolder = Path.Combine(_env.WebRootPath, rootFolder, folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save File 
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            result.IsSuccess = true;
            result.FileName = uniqueFileName;
            result.FilePath = Path.Combine(rootFolder, folderName, uniqueFileName).Replace("\\", "/"); result.Extension = extension;
            result.SizeInKb = Math.Round((double)file.Length / 1024, 2); // Convert to KB

            return result;
        }

        public async Task<FileDeleteResult> DeleteFileAsync(string filePath)
        {
            FileDeleteResult result = new FileDeleteResult();

            if (string.IsNullOrEmpty(filePath))
            {
                result.IsSuccess = false;
                result.Message = "File path not provided.";
            }

            var cleanPath = filePath.TrimStart('/', '\\');
            var fullPath = Path.Combine(_env.WebRootPath, cleanPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                result.IsSuccess = true;
                result.Message = "File deleted successfully.";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "File not found.";
            }

            return result;
        }

        public async Task<FileUploadResult> UpdateFileAsync(IFormFile file, string existingFilePath, string folderName, string[] allowedExtensions, int maxSizeInMb)
        {
            // First, delete the old file using its path if provided
            if (!string.IsNullOrWhiteSpace(existingFilePath))
            {
                await DeleteFileAsync(existingFilePath);
            }

            // Then, upload the new file
            return await UploadFileAsync(file, folderName, allowedExtensions, maxSizeInMb);
        }
    }
}
