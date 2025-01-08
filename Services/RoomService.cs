using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OutputCaching;
using server.Dtos.Response;
using server.Dtos.Room;
using server.Enums;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepo;

        public RoomService(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<ServiceResponse<List<Room>>> GetAllRooms(BaseQueryObject queryObject)
        {
            var (rooms, total) = await _roomRepo.GetAllRooms(queryObject);

            return new ServiceResponse<List<Room>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = rooms,
                Total = total,
                Took = rooms.Count,
            };
        }

        public async Task<ServiceResponse<Room>> GetRoomById(int roomId)
        {
            var room = await _roomRepo.GetRoomById(roomId);
            if (room == null || room.Status != RoomStatus.Available)
            {
                return new ServiceResponse<Room>
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.ROOM_NOT_FOUND_OR_UNAVAILABLE,
                };
            }

            return new ServiceResponse<Room>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = room,
            };
        }

        public async Task<ServiceResponse> CreateNewRoom(CreateUpdateRoomDto createRoomDto, int adminId)
        {
            var roomWithSameNumber = await _roomRepo.GetRoomByRoomNumber(createRoomDto.RoomNumber);
            if (roomWithSameNumber != null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_ROOM_NUMBER,
                };
            }

            var newRoom = new Room
            {
                RoomNumber = createRoomDto.RoomNumber,
                FloorId = createRoomDto.FloorId,
                RoomClassId = createRoomDto.RoomClassId,
                Status = RoomStatus.Available,
                CreatedById = adminId,
                Images = [],
            };

            foreach (var image in createRoomDto.Images)
            {
                var roomImage = new RoomImage { ImageUrl = image };
                newRoom.Images.Add(roomImage);
            }

            await _roomRepo.CreateNewRoom(newRoom);
            return new ServiceResponse
            {
                Status = ResStatusCode.CREATED,
                Success = true,
                Message = SuccessMessage.CREATE_ROOM_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> UpdateRoom(int roomId, CreateUpdateRoomDto updateRoomDto)
        {
            var targetRoom = await _roomRepo.GetRoomById(roomId);
            if (targetRoom == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.ROOM_NOT_FOUND,
                };
            }

            if (targetRoom.Status == RoomStatus.Occupied)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.BAD_REQUEST,
                    Success = false,
                    Message = ErrorMessage.ROOM_CANNOT_BE_UPDATED,
                };
            }

            var isRoomBooked = await _roomRepo.CheckIfRoomIsBooked(roomId);
            if (isRoomBooked)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.BAD_REQUEST,
                    Success = false,
                    Message = ErrorMessage.ROOM_IS_RESERVED_AND_WILL_BE_OCCUPIED_SOON,
                };
            }

            var roomWithSameNumber = await _roomRepo.GetRoomByRoomNumber(updateRoomDto.RoomNumber);
            if (roomWithSameNumber != null && roomWithSameNumber.Id != roomId)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_ROOM_NUMBER,
                };
            }

            targetRoom.RoomNumber = updateRoomDto.RoomNumber;
            targetRoom.FloorId = updateRoomDto.FloorId;
            targetRoom.RoomClassId = updateRoomDto.RoomClassId;
            targetRoom.Images = [];

            await _roomRepo.DeleteOldImagesOfRoom(roomId);
            foreach (var image in updateRoomDto.Images)
            {
                var roomImage = new RoomImage { ImageUrl = image };
                targetRoom.Images.Add(roomImage);
            }

            await _roomRepo.UpdateRoom(targetRoom);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_ROOM_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> DeleteRoom(int roomId)
        {
            var room = await _roomRepo.GetRoomById(roomId);
            if (room == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.ROOM_NOT_FOUND,
                };
            }

            var bookedTimes = await _roomRepo.CountBookedTimes(roomId);
            if (bookedTimes > 0)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.BAD_REQUEST,
                    Success = false,
                    Message = ErrorMessage.ROOM_CANNOT_BE_DELETED,
                };
            }

            await _roomRepo.DeleteRoom(room);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.DELETE_ROOM_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> ToggleMaintenance(int roomId)
        {
            var room = await _roomRepo.GetRoomById(roomId);
            if (room == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.ROOM_NOT_FOUND,
                };
            }

            if (room.Status == RoomStatus.Occupied)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.BAD_REQUEST,
                    Success = false,
                    Message = ErrorMessage.ROOM_CANNOT_BE_UPDATED,
                };
            }

            var isRoomBooked = await _roomRepo.CheckIfRoomIsBooked(roomId);
            if (isRoomBooked && room.Status == RoomStatus.Available)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.BAD_REQUEST,
                    Success = false,
                    Message = ErrorMessage.ROOM_IS_RESERVED_AND_WILL_BE_OCCUPIED_SOON,
                };
            }

            room.Status = room.Status == RoomStatus.OutOfService ? RoomStatus.Available : RoomStatus.OutOfService;

            await _roomRepo.UpdateRoom(room);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_ROOM_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> MarkCleaningDone(int roomId)
        {
            var room = await _roomRepo.GetRoomById(roomId);
            if (room == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.ROOM_NOT_FOUND,
                };
            }

            if (room.Status != RoomStatus.UnderCleaning)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.BAD_REQUEST,
                    Success = false,
                    Message = ErrorMessage.ROOM_CANNOT_BE_UPDATED,
                };
            }

            room.Status = RoomStatus.Available;

            await _roomRepo.UpdateRoom(room);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_ROOM_SUCCESSFULLY,
            };
        }
    }
}
