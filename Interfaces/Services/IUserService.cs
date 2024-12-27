using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.User;
using server.Models;
using server.Queries;

namespace server.Interfaces.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<List<Guest>>> GetAllGuests(BaseQueryObject queryObject);
        Task<ServiceResponse> UpdateGuestProfile(UpdateGuestDto updateGuestDto, int guestId);

        Task<ServiceResponse<List<Admin>>> GetAllAdmins(BaseQueryObject queryObject);
        Task<ServiceResponse> CreateNewAdmin(CreateAdminDto createAdminDto, int authUserId);
        Task<ServiceResponse> UpdateAdminProfile(UpdateAdminDto updateAdminDto, int adminId);
        Task<ServiceResponse> ToggleAdminActiveStatus(int adminId, int authUserId);
    }
}
