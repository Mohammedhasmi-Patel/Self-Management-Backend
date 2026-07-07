using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfManagement.Domain.Entities
{
    public class EmailOtp
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Otp { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }



    }
}
