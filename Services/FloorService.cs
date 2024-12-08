using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Floor;
using server.Dtos.Response;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepo;

        public FloorService(IFloorRepository floorRepo)
        {
            _floorRepo = floorRepo;
        }

        public async Task<ServiceResponse<List<Floor>>> GetAllFloors(BaseQueryObject queryObject)
        {
            var (floors, total) = await _floorRepo.GetAllFloors(queryObject);

            return new ServiceResponse<List<Floor>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = floors,
            };
        }

        public async Task<ServiceResponse<Floor>> GetFloorById(int floorId)
        {
            var floor = await _floorRepo.GetFloorById(floorId);

            if (floor == null)
            {
                return new ServiceResponse<Floor>
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.FLOOR_NOT_FOUND,
                };
            }

            return new ServiceResponse<Floor>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = floor,
            };
        }

        public async Task<ServiceResponse> CreateNewFloor(CreateFloorDto createFloorDto)
        {
            var floorsWithSameName = await _floorRepo.GetFloorsByFloorNumber(createFloorDto.FloorNumber);

            if (floorsWithSameName != null && floorsWithSameName.Count > 0)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_FLOOR_NAME,
                };
            }

            var newFloor = new Floor
            {
                FloorNumber = createFloorDto.FloorNumber,
            };

            await _floorRepo.AddFloor(newFloor);

            return new ServiceResponse
            {
                Status = ResStatusCode.CREATED,
                Success = true,
                Message = SuccessMessage.CREATE_FLOOR_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> UpdateFloor(UpdateFloorDto updateFloorDto, int floorId)
        {
            var targetFloor = await _floorRepo.GetFloorById(floorId);
            if (targetFloor == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.FLOOR_NOT_FOUND_OR_UNAVAILABLE,
                };
            }

            var floorWithSameName = await _floorRepo.GetFloorsByFloorNumber(updateFloorDto.FloorNumber);
            if (floorWithSameName != null && floorWithSameName.Where(f => f.Id != floorId).ToList().Count > 0)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_FLOOR_NAME,
                };
            }

            targetFloor.FloorNumber = updateFloorDto.FloorNumber;
            await _floorRepo.UpdateFloor(targetFloor);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_FLOOR_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> DeleteFloor(int floorId)
        {
            var targetFloor = await _floorRepo.GetFloorById(floorId);
            
            if (targetFloor == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.FLOOR_NOT_FOUND,
                };
            }

            await _floorRepo.DeleteFloor(targetFloor);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.DELETE_FLOOR_SUCCESSFULLY,
            };
        }

    }
}