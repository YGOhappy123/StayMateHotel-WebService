using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Floor;
using server.Models;

namespace server.Extensions.Mappers
{
    public static class FloorMapper
    {
        public static FloorDto ToFloorDto(this Floor floorModel)
        {
            return new FloorDto
            {
                Id = floorModel.Id,
                FloorNumber = floorModel.FloorNumber,
            };
        }
    }
}