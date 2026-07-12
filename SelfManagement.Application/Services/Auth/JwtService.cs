
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.ServiceInterface.Auth;
using SelfManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SelfManagement.Application.Services.Auth
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtService(UserManager<ApplicationUser> userManager, IOptions<JwtConfiguration> jwtConfiguration)
        {
            _userManager = userManager;
            _jwtConfiguration = jwtConfiguration.Value;
        }
        public async Task<JwtConfigurationResponse> GenerateJwtToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var singleRole = userRoles.FirstOrDefault();


            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,singleRole!)
            };

            var jwtSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));
            var credentials = new SigningCredentials(jwtSecretKey,SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityToken(
   
                issuer : _jwtConfiguration.Issuer,
                audience : _jwtConfiguration.Audience,
                claims : claims,
                expires : DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpireMinutes),
                signingCredentials : credentials
             );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenHandler);

            return new JwtConfigurationResponse()
            {
                Success = true,
                Token = token,
                ExpiredAt = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpireMinutes)
            };
        }
    }
}
