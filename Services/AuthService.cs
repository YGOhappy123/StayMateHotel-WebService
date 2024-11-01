using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Account;
using server.Dtos.Auth;
using server.Dtos.Response;
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

        public AuthService(IAccountRepository accountRepo, IGuestRepository guestRepo)
        {
            _accountRepo = accountRepo;
            _guestRepo = guestRepo;
        }

        public Task<ServiceResponse<AppUser>> SignIn(SignInDto signInDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<GuestDto>> SignUpGuestAccount(SignUpDto signUpDto)
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

                return new ServiceResponse<GuestDto>
                {
                    Success = true,
                    Message = SuccessMessage.SIGN_UP_SUCCESSFULLY,
                    Data = newGuest.ToGuestDto(),
                };
            }
            else
            {
                if (existedAccount.IsActive)
                {
                    return new ServiceResponse<GuestDto>
                    {
                        Status = StatusCodes.Status409Conflict,
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

                    return new ServiceResponse<GuestDto>
                    {
                        Status = StatusCodes.Status200OK,
                        Success = true,
                        Message = SuccessMessage.REACTIVATE_ACCOUNT_SUCCESSFULLY,
                        Data = guestData.ToGuestDto(),
                    };
                }
            }
        }
    }
}
