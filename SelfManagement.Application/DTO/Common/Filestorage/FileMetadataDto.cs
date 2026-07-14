
namespace SelfManagement.Application.DTO.Common.Filestorage
{
    public class FileMetadataDto
    {
        public string FileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}
