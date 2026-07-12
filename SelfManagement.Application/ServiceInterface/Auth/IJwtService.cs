using SelfManagement.Application.DTO.Common;
using SelfManagement.Domain.Entities;

namespace SelfManagement.Application.ServiceInterface.Auth
{
    public interface IJwtService
    {
        public Task<JwtConfigurationResponse> GenerateJwtToken(ApplicationUser user);
    }
}
