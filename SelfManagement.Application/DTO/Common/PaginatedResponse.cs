
namespace SelfManagement.Application.DTO.Common
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = System.Linq.Enumerable.Empty<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    } 
}
