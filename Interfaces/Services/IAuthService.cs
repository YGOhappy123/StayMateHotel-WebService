using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Auth;
using server.Dtos.Response;
using server.Models;

namespace server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<AppUser>> SignIn(SignInDto signInDto);
        Task<ServiceResponse<Guest>> SignUpGuestAccount(SignUpDto signUpDto);
        Task<ServiceResponse> RefreshToken(RefreshTokenDto refreshTokenDto);
        Task<ServiceResponse> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task<ServiceResponse> ResetPassword(string resetPasswordToken, ResetPasswordDto resetPasswordDto);
    }
}
