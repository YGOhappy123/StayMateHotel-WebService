using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using server.Dtos.Auth;
using server.Dtos.Response;
using server.Enums;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Utilities;

namespace server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepo;
        private readonly IGuestRepository _guestRepo;
        private readonly IAdminRepository _adminRepo;
        private readonly IJwtService _jwtService;
        private readonly IMailerService _mailerService;

        public AuthService(
            IConfiguration configuration,
            IAccountRepository accountRepo,
            IGuestRepository guestRepo,
            IAdminRepository adminRepo,
            IJwtService jwtService,
            IMailerService mailerService
        )
        {
            _configuration = configuration;
            _accountRepo = accountRepo;
            _guestRepo = guestRepo;
            _adminRepo = adminRepo;
            _jwtService = jwtService;
            _mailerService = mailerService;
        }

        public async Task<ServiceResponse<AppUser>> SignIn(SignInDto signInDto)
        {
            var existedAccount = await _accountRepo.GetAccountByUsername(signInDto.Username);
            if (existedAccount == null || !BCrypt.Net.BCrypt.Verify(signInDto.Password, existedAccount.Password))
            {
                return new ServiceResponse<AppUser>
                {
                    Status = ResStatusCode.UNAUTHORIZED,
                    Success = false,
                    Message = ErrorMessage.INCORRECT_USERNAME_OR_PASSWORD,
                };
            }

            AppUser? userData =
                (existedAccount.Role == UserRole.Guest)
                    ? await _guestRepo.GetGuestByAccountId(existedAccount.Id)
                    : await _adminRepo.GetAdminByAccountId(existedAccount.Id);

            return new ServiceResponse<AppUser>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.SIGN_IN_SUCCESSFULLY,
                Data = userData,
                AccessToken = _jwtService.GenerateAccessToken(userData!, existedAccount.Role),
                RefreshToken = _jwtService.GenerateRefreshToken(existedAccount),
            };
        }

        public async Task<ServiceResponse<Guest>> SignUpGuestAccount(SignUpDto signUpDto)
        {
            var existedAccount = await _accountRepo.GetAccountByUsername(signUpDto.Username);
            if (existedAccount != null)
            {
                return new ServiceResponse<Guest>
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.USERNAME_EXISTED,
                };
            }

            var newAccount = new Account { Username = signUpDto.Username, Password = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password) };
            await _accountRepo.AddAccount(newAccount);

            var newGuest = new Guest
            {
                FirstName = signUpDto.FirstName,
                LastName = signUpDto.LastName,
                AccountId = newAccount.Id,
                Avatar = _configuration["Application:DefaultUserAvatar"],
            };
            await _guestRepo.AddGuest(newGuest);

            return new ServiceResponse<Guest>
            {
                Status = ResStatusCode.CREATED,
                Success = true,
                Message = SuccessMessage.SIGN_UP_SUCCESSFULLY,
                Data = newGuest,
                AccessToken = _jwtService.GenerateAccessToken(newGuest!, UserRole.Guest),
                RefreshToken = _jwtService.GenerateRefreshToken(newAccount),
            };
        }

        public async Task<ServiceResponse> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            if (_jwtService.VerifyRefreshToken(refreshTokenDto.RefreshToken, out var principal))
            {
                var accountId = principal!.FindFirst(ClaimTypes.Name)!.Value;
                var account = await _accountRepo.GetAccountById(int.Parse(accountId));

                if (account == null)
                {
                    return new ServiceResponse
                    {
                        Status = ResStatusCode.UNAUTHORIZED,
                        Success = false,
                        Message = ErrorMessage.INVALID_CREDENTIALS,
                    };
                }

                AppUser? userData =
                    (account.Role == UserRole.Guest)
                        ? await _guestRepo.GetGuestByAccountId(account.Id)
                        : await _adminRepo.GetAdminByAccountId(account.Id);

                return new ServiceResponse
                {
                    Status = ResStatusCode.OK,
                    Success = true,
                    Message = SuccessMessage.REFRESH_TOKEN_SUCCESSFULLY,
                    AccessToken = _jwtService.GenerateAccessToken(userData!, account.Role),
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.UNAUTHORIZED,
                    Success = false,
                    Message = ErrorMessage.INVALID_CREDENTIALS,
                };
            }
        }

        public async Task<ServiceResponse> ChangePassword(ChangePasswordDto changePasswordDto, int authUserId, string authUserRole)
        {
            var targetAccount = await _accountRepo.GetAccountByUserIdAndRole(authUserId, authUserRole);
            if (targetAccount == null || !BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, targetAccount.Password))
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.UNAUTHORIZED,
                    Success = false,
                    Message = ErrorMessage.INCORRECT_USERNAME_OR_PASSWORD,
                };
            }

            targetAccount.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            await _accountRepo.UpdateAccount(targetAccount);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.CHANGE_PASSWORD_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var existedGuest = await _guestRepo.GetGuestByEmail(forgotPasswordDto.Email);
            if (existedGuest == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.USER_NOT_FOUND,
                };
            }

            await _mailerService.SendResetPasswordEmail(
                forgotPasswordDto.Email,
                $"{existedGuest.LastName} {existedGuest.FirstName}",
                $"{_configuration["Application:ClientUrl"]}/auth?type=reset&token={_jwtService.GenerateResetPasswordToken(existedGuest)}"
            );

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.RESET_PASSWORD_EMAIL_SENT,
            };
        }

        public async Task<ServiceResponse> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (_jwtService.VerifyResetPasswordToken(resetPasswordDto.ResetPasswordToken, out var principal))
            {
                var email = principal!.FindFirst(ClaimTypes.Email)!.Value;
                var account = await _accountRepo.GetGuestAccountByEmail(email);

                if (account == null)
                {
                    return new ServiceResponse
                    {
                        Status = ResStatusCode.UNAUTHORIZED,
                        Success = false,
                        Message = ErrorMessage.INVALID_CREDENTIALS,
                    };
                }

                account.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.Password);
                await _accountRepo.UpdateAccount(account);

                return new ServiceResponse
                {
                    Status = ResStatusCode.OK,
                    Success = true,
                    Message = SuccessMessage.RESET_PASSWORD_SUCCESSFULLY,
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.UNAUTHORIZED,
                    Success = false,
                    Message = ErrorMessage.INVALID_CREDENTIALS,
                };
            }
        }

        public async Task<ServiceResponse<Guest>> GoogleAuthentication(GoogleAuthDto googleAuthDto)
        {
            var googleUserInfo = await FetchGoogleUserInfoAsync(googleAuthDto.GoogleAccessToken);
            if (googleUserInfo == null || !googleUserInfo.EmailVerified)
            {
                return new ServiceResponse<Guest>
                {
                    Status = ResStatusCode.UNAUTHORIZED,
                    Success = false,
                    Message = ErrorMessage.GOOGLE_AUTH_FAILED,
                };
            }

            var existedAccount = await _accountRepo.GetGuestAccountByEmail(googleUserInfo.Email);
            if (existedAccount == null)
            {
                string randomUsername = RandomStringGenerator.GenerateRandomString(16);
                string randomPassword = RandomStringGenerator.GenerateRandomString(16);

                var newAccount = new Account { Username = randomUsername, Password = BCrypt.Net.BCrypt.HashPassword(randomPassword) };
                await _accountRepo.AddAccount(newAccount);

                var newGuest = new Guest
                {
                    FirstName = googleUserInfo.FirstName,
                    LastName = googleUserInfo.LastName,
                    AccountId = newAccount.Id,
                    Avatar = googleUserInfo.Picture ?? _configuration["Application:DefaultUserAvatar"],
                    Email = googleUserInfo.Email,
                };
                await _guestRepo.AddGuest(newGuest);

                await _mailerService.SendGoogleRegistrationSuccessEmail(
                    googleUserInfo.Email,
                    $"{newGuest.LastName} {newGuest.FirstName}",
                    randomUsername,
                    randomPassword,
                    $"{_configuration["Application:ClientUrl"]}/profile/change-password"
                );

                return new ServiceResponse<Guest>
                {
                    Status = ResStatusCode.CREATED,
                    Success = true,
                    Message = SuccessMessage.GOOGLE_AUTH_SUCCESSFULLY,
                    Data = newGuest,
                    AccessToken = _jwtService.GenerateAccessToken(newGuest!, UserRole.Guest),
                    RefreshToken = _jwtService.GenerateRefreshToken(newAccount),
                };
            }
            else
            {
                var guestData = await _guestRepo.GetGuestByAccountId(existedAccount.Id);

                return new ServiceResponse<Guest>
                {
                    Status = ResStatusCode.OK,
                    Success = true,
                    Message = SuccessMessage.GOOGLE_AUTH_SUCCESSFULLY,
                    Data = guestData,
                    AccessToken = _jwtService.GenerateAccessToken(guestData!, UserRole.Guest),
                    RefreshToken = _jwtService.GenerateRefreshToken(existedAccount),
                };
            }
        }

        private async Task<GoogleUserInfoDto?> FetchGoogleUserInfoAsync(string googleAccessToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", googleAccessToken);

            var response = await httpClient.GetAsync(_configuration["GoogleApi:OAuthEndPoint"]);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GoogleUserInfoDto>(json);
        }

        public async Task<ServiceResponse> DeactivateAccount(DeactivateAccountDto deactivateAccountDto, int authUserId, string authUserRole)
        {
            var targetAccount = await _accountRepo.GetAccountByUserIdAndRole(
                deactivateAccountDto.TargetUserId,
                deactivateAccountDto.TargetUserRole
            );
            if (targetAccount == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.UNAUTHORIZED,
                    Success = false,
                    Message = ErrorMessage.USER_NOT_FOUND,
                };
            }

            // Guest: can only deactivate his/her account
            // Admin: can deactivate any account
            if (
                authUserRole == UserRole.Guest.ToString()
                && (authUserRole != targetAccount.Role.ToString() || authUserId != targetAccount.Id)
            )
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.FORBIDDEN,
                    Success = false,
                    Message = ErrorMessage.NO_PERMISSION,
                };
            }

            targetAccount.IsActive = false;
            await _accountRepo.UpdateAccount(targetAccount);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.DEACTIVATE_ACCOUNT_SUCCESSFULLY,
            };
        }
    }
}
