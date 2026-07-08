
using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.DTO.Common;

namespace SelfManagement.Application.ServiceInterface
{
    public interface IAuthService
    {
        public Task<ApiResponse<object>> RegisterUser(RegisterUserRequest request);
    }
}
