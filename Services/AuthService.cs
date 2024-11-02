using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Account;
using server.Dtos.Auth;
using server.Dtos.Response;
using server.Enums;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Mappers;
using server.Models;
using server.Utilities;

namespace server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IGuestRepository _guestRepo;
        private readonly IAdminRepository _adminRepo;

        public AuthService(
            IAccountRepository accountRepo,
            IGuestRepository guestRepo,
            IAdminRepository adminRepo
        )
        {
            _accountRepo = accountRepo;
            _guestRepo = guestRepo;
            _adminRepo = adminRepo;
        }

        public async Task<ServiceResponse<AppUser>> SignIn(SignInDto signInDto)
        {
            var existedAccount = await _accountRepo.GetAccountByUsername(signInDto.Username);

            if (
                existedAccount == null
                || !existedAccount.IsActive
                || !BCrypt.Net.BCrypt.Verify(signInDto.Password, existedAccount.Password)
            )
            {
                return new ServiceResponse<AppUser>
                {
                    Status = ResStatusCode.UNAUTHORIZED,
                    Success = false,
                    Message = ErrorMessage.INVALID_CREDENTIALS,
                };
            }
            else
            {
                if (existedAccount.Role == UserRole.Guest)
                {
                    var guestData = await _guestRepo.GetGuestByAccountId(existedAccount.Id);
                    return new ServiceResponse<AppUser>
                    {
                        Status = ResStatusCode.OK,
                        Success = true,
                        Message = SuccessMessage.SIGN_IN_SUCCESSFULLY,
                        Data = guestData,
                    };
                }
                else
                {
                    var adminData = await _adminRepo.GetAdminByAccountId(existedAccount.Id);
                    return new ServiceResponse<AppUser>
                    {
                        Status = ResStatusCode.OK,
                        Success = true,
                        Message = SuccessMessage.SIGN_IN_SUCCESSFULLY,
                        Data = adminData,
                    };
                }
            }
        }

        public async Task<ServiceResponse<Guest>> SignUpGuestAccount(SignUpDto signUpDto)
        {
            var existedAccount = await _accountRepo.GetAccountByUsername(signUpDto.Username);

            if (existedAccount == null)
            {
                var newAccount = new Account
                {
                    Username = signUpDto.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password),
                };

                await _accountRepo.AddAccount(newAccount);

                var newGuest = new Guest
                {
                    FirstName = signUpDto.FirstName,
                    LastName = signUpDto.LastName,
                    AccountId = newAccount.Id,
                };

                await _guestRepo.AddGuest(newGuest);

                return new ServiceResponse<Guest>
                {
                    Status = ResStatusCode.CREATED,
                    Success = true,
                    Message = SuccessMessage.SIGN_UP_SUCCESSFULLY,
                    Data = newGuest,
                };
            }
            else
            {
                if (existedAccount.IsActive)
                {
                    return new ServiceResponse<Guest>
                    {
                        Status = ResStatusCode.CONFLICT,
                        Success = false,
                        Message = ErrorMessage.USERNAME_EXISTED,
                    };
                }
                else
                {
                    existedAccount.IsActive = true;
                    existedAccount.Password = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password);
                    await _accountRepo.UpdateAccount(existedAccount);

                    var guestData = await _guestRepo.GetGuestByAccountId(existedAccount.Id);
                    guestData!.FirstName = signUpDto.FirstName;
                    guestData!.LastName = signUpDto.LastName;

                    await _guestRepo.UpdateGuest(guestData);

                    return new ServiceResponse<Guest>
                    {
                        Status = ResStatusCode.OK,
                        Success = true,
                        Message = SuccessMessage.REACTIVATE_ACCOUNT_SUCCESSFULLY,
                        Data = guestData,
                    };
                }
            }
        }
    }
}