using server.Dtos.Response;
using server.Dtos.RoomClass;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Queries;
using server.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Services
{
    public class RoomClassService : IRoomClassService
    {
        private readonly IRoomClassRepository _roomClassRepo;

        public RoomClassService(IRoomClassRepository roomClassRepo)
        {
            _roomClassRepo = roomClassRepo;
        }

        public async Task<ServiceResponse<List<RoomClass>>> GetAllRoomClasses(BaseQueryObject queryObject)
        {
            var (roomClasses, total) = await _roomClassRepo.GetAllRoomClasses(queryObject);

            return new ServiceResponse<List<RoomClass>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = roomClasses,
                Total = total,
                Took = roomClasses.Count,
            };
        }

        public async Task<ServiceResponse<RoomClass>> GetRoomClassById(int roomClassId)
        {
            var roomClass = await _roomClassRepo.GetRoomClassById(roomClassId);
            if (roomClass == null)
            {
                return new ServiceResponse<RoomClass>
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.ROOM_CLASS_NOT_FOUND,
                };
            }

            return new ServiceResponse<RoomClass>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = roomClass,
            };
        }

        public async Task<ServiceResponse> CreateNewRoomClass(CreateUpdateRoomClassDto createRoomClassDto, int adminId)
        {
            var roomClassWithSameName = await _roomClassRepo.GetRoomClassByName(createRoomClassDto.ClassName);
            if (roomClassWithSameName != null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_ROOM_CLASS_NAME,
                };
            }

            var newRoomClass = new RoomClass
            {
                ClassName = createRoomClassDto.ClassName,
                BasePrice = createRoomClassDto.BasePrice,
                Capacity = createRoomClassDto.Capacity,
                CreatedById = adminId,
                RoomClassFeatures = [],
            };

            foreach (var feature in createRoomClassDto.Features)
            {
                var roomClassFeature = new RoomClassFeature { FeatureId = feature.FeatureId, Quantity = feature.Quantity   };
                newRoomClass.RoomClassFeatures.Add(roomClassFeature);
            }


            await _roomClassRepo.CreateNewRoomClass(newRoomClass);
            return new ServiceResponse
            {
                Status = ResStatusCode.CREATED,
                Success = true,
                Message = SuccessMessage.CREATE_ROOM_CLASS_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> UpdateRoomClass(int roomClassId, CreateUpdateRoomClassDto updateRoomClassDto)
        {
            var targetRoomClass = await _roomClassRepo.GetRoomClassById(roomClassId);
            if (targetRoomClass == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.ROOM_CLASS_NOT_FOUND,
                };
            }

            var roomClassWithSameName = await _roomClassRepo.GetRoomClassByName(updateRoomClassDto.ClassName);
            if (roomClassWithSameName != null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_ROOM_CLASS_NAME,
                };
            }

            targetRoomClass.ClassName = updateRoomClassDto.ClassName;
            targetRoomClass.BasePrice = updateRoomClassDto.BasePrice;
            targetRoomClass.Capacity = updateRoomClassDto.Capacity;
            targetRoomClass.RoomClassFeatures = [];

            await _roomClassRepo.DeleteFeatureOfRoomClass(roomClassId);
            foreach (var feature in updateRoomClassDto.Features)
            {
                var roomClassFeature = new RoomClassFeature { FeatureId = feature.FeatureId, Quantity = feature.Quantity   };
                targetRoomClass.RoomClassFeatures.Add(roomClassFeature);
            }

            await _roomClassRepo.UpdateRoomClass(targetRoomClass);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_ROOM_CLASS_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> DeleteRoomClass(int roomClassId)
        {
            var roomClass = await _roomClassRepo.GetRoomClassById(roomClassId);
            if (roomClass == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.ROOM_CLASS_NOT_FOUND,
                };
            }

            var roomCount = await _roomClassRepo.CountRoomsInRoomClass(roomClassId);
            if (roomCount > 0)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.BAD_REQUEST,
                    Success = false,
                    Message = ErrorMessage.ROOM_CLASS_CANNOT_BE_DELETED,
                };
            }

            await _roomClassRepo.DeleteRoomClass(roomClass);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.DELETE_ROOM_CLASS_SUCCESSFULLY,
            };
        }

    }
}
