
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SelfManagement.Application.DTO.Common.Filestorage;
using SelfManagement.Application.Exceptions;
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

        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

        }

        public async Task<FileMetadataDto> SaveFileAsync(IFormFile file, string subDirectory, string[] allowedExtensions,
        long maxSizeBytes)
        {
            if (file == null || file.Length == 0) throw new BadRequestException("Invalid file");

            if(file.Length > maxSizeBytes)
            {
                throw new BadRequestException($"File size exceeds the limit of {maxSizeBytes / 1024 / 1024} MB.");
            }


            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uniqueFilename = $"{Guid.NewGuid():N}{extension}";

            if (!allowedExtensions.Contains(extension))
            {
                throw new BadRequestException($"Extension {extension} is not allowed.");
            }

            string upload = "upload";

            var uploadsPath = Path.Combine(_env.WebRootPath, upload, subDirectory);

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var fullpath = Path.Combine(uploadsPath,uniqueFilename);

            using var stream = new FileStream(fullpath,FileMode.Create);
            await file.CopyToAsync(stream);
            return new FileMetadataDto
            {
                FileName = file.FileName,
                StoredFileName = uniqueFilename,
                Extension = extension,
                SizeBytes = file.Length,
                ContentType = file.ContentType,
                FilePath = Path.Combine(subDirectory, uniqueFilename)
            };

        }
    }
}
