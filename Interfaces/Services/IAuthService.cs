using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Account;
using server.Dtos.Auth;
using server.Dtos.Response;
using server.Models;

namespace server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<AppUser>> SignIn(SignInDto signInDto);
        Task<ServiceResponse<GuestDto>> SignUpGuestAccount(SignUpDto signUpDto);
    }
}
