using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfManagement.Application.ServiceInterface.Auth
{
    public interface IOtpService
    {
        Task<GenerateOtpResponse> GenerateAndSendOtpAsync(string email,Guid? userId);
        Task<ApiResponse<object>> VerifyOtpAsync(string email, int otp);

    }
}
