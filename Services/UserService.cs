using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.User;
using server.Enums;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepo;
        private readonly IGuestRepository _guestRepo;
        private readonly IAdminRepository _adminRepo;
        private readonly IMailerService _mailerService;

        public UserService(
            IConfiguration configuration,
            IAccountRepository accountRepo,
            IGuestRepository guestRepo,
            IAdminRepository adminRepo,
            IMailerService mailerService
        )
        {
            _configuration = configuration;
            _accountRepo = accountRepo;
            _guestRepo = guestRepo;
            _adminRepo = adminRepo;
            _mailerService = mailerService;
        }

        public async Task<ServiceResponse<List<Guest>>> GetAllGuests(BaseQueryObject queryObject)
        {
            var (guests, total) = await _guestRepo.GetAllGuests(queryObject);

            return new ServiceResponse<List<Guest>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = guests,
                Total = total,
                Took = guests.Count,
            };
        }

        public async Task<ServiceResponse> UpdateGuestProfile(UpdateGuestDto updateGuestDto, int guestId)
        {
            var targetGuest = await _guestRepo.GetGuestById(guestId);
            if (targetGuest == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.USER_NOT_FOUND,
                };
            }

            targetGuest.FirstName = updateGuestDto.FirstName;
            targetGuest.LastName = updateGuestDto.LastName;

            if (!string.IsNullOrEmpty(updateGuestDto.Avatar))
            {
                targetGuest.Avatar = updateGuestDto.Avatar;
            }

            targetGuest.Address = string.IsNullOrEmpty(updateGuestDto.Address) ? null : updateGuestDto.Address;
            targetGuest.PhoneNumber = string.IsNullOrEmpty(updateGuestDto.PhoneNumber) ? null : updateGuestDto.PhoneNumber;

            if (!string.IsNullOrEmpty(updateGuestDto.Email))
            {
                var guestWithThisEmail = await _guestRepo.GetGuestByEmail(updateGuestDto.Email);
                if (guestWithThisEmail != null && guestWithThisEmail.Id != guestId)
                {
                    return new ServiceResponse
                    {
                        Status = ResStatusCode.CONFLICT,
                        Success = false,
                        Message = ErrorMessage.EMAIL_EXISTED,
                    };
                }

                targetGuest.Email = updateGuestDto.Email;
            }
            else
            {
                targetGuest.Email = null;
            }

            await _guestRepo.UpdateGuest(targetGuest);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_USER_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse<List<Admin>>> GetAllAdmins(BaseQueryObject queryObject)
        {
            var (admins, total) = await _adminRepo.GetAllAdmins(queryObject);

            return new ServiceResponse<List<Admin>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = admins,
                Total = total,
                Took = admins.Count,
            };
        }

        public async Task<ServiceResponse> CreateNewAdmin(CreateAdminDto createAdminDto, int authUserId)
        {
            var adminWithSameEmail = await _adminRepo.GetAdminByEmailIncludeInactive(createAdminDto.Email);
            if (adminWithSameEmail != null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.EMAIL_EXISTED,
                };
            }

            var adminWithSamePhone = await _adminRepo.GetAdminByPhoneNumberIncludeInactive(createAdminDto.PhoneNumber);
            if (adminWithSamePhone != null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.PHONE_NUMBER_EXISTED,
                };
            }

            string randomUsername = RandomStringGenerator.GenerateRandomString(16);
            string randomPassword = RandomStringGenerator.GenerateRandomString(16);

            var newAccount = new Account
            {
                Username = randomUsername,
                Password = BCrypt.Net.BCrypt.HashPassword(randomPassword),
                Role = UserRole.Admin,
            };
            await _accountRepo.AddAccount(newAccount);

            var newAdmin = new Admin
            {
                FirstName = createAdminDto.FirstName,
                LastName = createAdminDto.LastName,
                Email = createAdminDto.Email,
                PhoneNumber = createAdminDto.PhoneNumber,
                Avatar = _configuration["Application:DefaultUserAvatar"],
                AccountId = newAccount.Id,
                CreatedById = authUserId,
            };
            await _adminRepo.AddAdmin(newAdmin);

            await _mailerService.SendWelcomeNewAdminEmail(
                createAdminDto.Email,
                $"{createAdminDto.LastName} {createAdminDto.FirstName}",
                randomUsername,
                randomPassword,
                $"{_configuration["Application:ClientUrl"]}/profile/change-password"
            );

            return new ServiceResponse
            {
                Status = ResStatusCode.CREATED,
                Success = true,
                Message = SuccessMessage.CREATE_ADMIN_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> UpdateAdminProfile(UpdateAdminDto updateAdminDto, int adminId)
        {
            var targetAdmin = await _adminRepo.GetAdminById(adminId);
            if (targetAdmin == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.USER_NOT_FOUND,
                };
            }

            targetAdmin.FirstName = updateAdminDto.FirstName;
            targetAdmin.LastName = updateAdminDto.LastName;

            if (!string.IsNullOrEmpty(updateAdminDto.Avatar))
            {
                targetAdmin.Avatar = updateAdminDto.Avatar;
            }

            var adminWithThisPhoneNumber = await _adminRepo.GetAdminByPhoneNumber(updateAdminDto.PhoneNumber);
            if (adminWithThisPhoneNumber != null && adminWithThisPhoneNumber.Id != adminId)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.PHONE_NUMBER_EXISTED,
                };
            }

            targetAdmin.PhoneNumber = updateAdminDto.PhoneNumber;

            await _adminRepo.UpdateAdmin(targetAdmin);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_USER_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> ToggleAdminActiveStatus(int adminId, int authUserId)
        {
            if (adminId == authUserId)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.FORBIDDEN,
                    Success = false,
                    Message = ErrorMessage.NO_PERMISSION,
                };
            }

            var targetAdmin = await _adminRepo.GetAdminByIdIncludeInactive(adminId);
            if (targetAdmin == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.USER_NOT_FOUND,
                };
            }

            bool newActiveStatus = !targetAdmin.Account!.IsActive;
            targetAdmin.Account!.IsActive = newActiveStatus;

            await _adminRepo.UpdateAdmin(targetAdmin);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = newActiveStatus ? SuccessMessage.REACTIVATE_ACCOUNT_SUCCESSFULLY : SuccessMessage.DEACTIVATE_ACCOUNT_SUCCESSFULLY,
            };
        }
    }
}
