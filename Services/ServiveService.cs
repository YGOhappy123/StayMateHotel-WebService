using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.Service;  // Import Dto cho Service
using server.Models;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Queries;
using server.Utilities;

namespace server.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepo;

        public ServiceService(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        // Lấy tất cả các Service với các bộ lọc và sắp xếp
        public async Task<ServiceResponse<List<Service>>> GetAllServices(BaseQueryObject queryObject)
        {
            var (services, total) = await _serviceRepo.GetAllServices(queryObject);

            return new ServiceResponse<List<Service>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = services,
                Total = total,
                Took = services.Count,
            };
        }

        // Lấy thông tin Service theo ID
        public async Task<ServiceResponse<Service>> GetServiceById(int serviceId)
        {
            var service = await _serviceRepo.GetServiceById(serviceId);
            if (service == null)
            {
                return new ServiceResponse<Service>
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.SERVICE_NOT_FOUND,
                };
            }

            return new ServiceResponse<Service>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = service,
            };
        }

        // Tạo mới một Service
        public async Task<ServiceResponse> CreateNewService(CreateUpdateServiceDto createServiceDto, int adminId)
        {
            var serviceWithSameName = await _serviceRepo.GetServiceByName(createServiceDto.Name);
            if (serviceWithSameName != null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_SERVICE_NAME,
                };
            }

            var newService = new Service
            {
                Name = createServiceDto.Name,
                Price = createServiceDto.Price,
                IsAvailable = createServiceDto.IsAvailable,
                CreatedById = adminId,
                CreatedAt = DateTime.UtcNow,
            };

            await _serviceRepo.CreateNewService(newService);
            return new ServiceResponse
            {
                Status = ResStatusCode.CREATED,
                Success = true,
                Message = SuccessMessage.CREATE_SERVICE_SUCCESSFULLY,
            };
        }

        // Cập nhật thông tin của một Service
        public async Task<ServiceResponse> UpdateService(int serviceId, CreateUpdateServiceDto updateServiceDto)
        {
            var targetService = await _serviceRepo.GetServiceById(serviceId);
            if (targetService == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.SERVICE_NOT_FOUND,
                };
            }

            // Kiểm tra nếu tên service bị trùng
            var serviceWithSameName = await _serviceRepo.GetServiceByName(updateServiceDto.Name);
            if (serviceWithSameName != null && serviceWithSameName.Id != serviceId)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_SERVICE_NAME,
                };
            }

            targetService.Name = updateServiceDto.Name;
            targetService.Price = updateServiceDto.Price;
            targetService.IsAvailable = updateServiceDto.IsAvailable;

            await _serviceRepo.UpdateService(targetService);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_SERVICE_SUCCESSFULLY,
            };
        }

        // Xóa một Service
        public async Task<ServiceResponse> DeleteService(int serviceId)
        {
            var service = await _serviceRepo.GetServiceById(serviceId);
            if (service == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.SERVICE_NOT_FOUND,
                };
            }

            await _serviceRepo.DeleteService(service);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.DELETE_SERVICE_SUCCESSFULLY,
            };
        }
    }
}
