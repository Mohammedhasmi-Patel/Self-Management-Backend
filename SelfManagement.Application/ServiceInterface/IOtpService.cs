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
        Task<(bool Success, string Message)> VerifyOtpAsync(string email, int otp);

    }
}
