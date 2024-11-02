using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;
using server.Models;

namespace server.Interfaces.Services
{
    public interface IJwtService
    {
        public string GenerateAccessToken(AppUser user, UserRole role);
        public string GenerateRefreshToken(Account account);
    }
}
