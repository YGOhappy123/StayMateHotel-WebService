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
        bool VerifyRefreshToken(string refreshToken, out ClaimsPrincipal? principal);
    }
}
