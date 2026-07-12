using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.DTO.Common;

namespace SelfManagement.Application.ServiceInterface.Auth
{
    public interface IAuthService
    {
        public Task<ApiResponse<object>> RegisterUser(RegisterUserRequest request);
        public Task<ApiResponse<LoginUserResponse>> LoginUser(LoginUserRequest request);
    }
}
