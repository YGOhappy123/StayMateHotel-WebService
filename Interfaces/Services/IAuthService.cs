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
        Task<ServiceResponse> ChangePassword(ChangePasswordDto changePasswordDto, int authUserId, string authUserRole);
        Task<ServiceResponse> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task<ServiceResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ServiceResponse<Guest>> GoogleAuthentication(GoogleAuthDto googleAuthDto);
        Task<ServiceResponse> DeactivateAccount(DeactivateAccountDto deactivateAccountDto, int authUserId, string authUserRole);
    }
}
