
namespace SelfManagement.Application.DTO.Company
{
    public class CompanyListResponse
    {
       
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
