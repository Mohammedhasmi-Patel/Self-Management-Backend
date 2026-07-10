using SelfManagement.Application.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfManagement.Application.ServiceInterface
{
    public interface IOtpService
    {
        Task<int> GenerateAndSendOtpAsync(string email,Guid? userId);
        Task<ApiResponse<object>> VerifyOtpAsync(string email, int otp);

    }
}
