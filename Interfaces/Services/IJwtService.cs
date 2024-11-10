using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using server.Enums;
using server.Models;

namespace server.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(AppUser user, UserRole role);
        string GenerateRefreshToken(Account account);
        string GenerateResetPasswordToken(Guest guest);
        bool VerifyRefreshToken(string refreshToken, out ClaimsPrincipal? principal);
        bool VerifyResetPasswordToken(string resetPasswordToken, out ClaimsPrincipal? principal);
    }
}
