using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.RoomClass;
using server.Models;

namespace server.Extensions.Mappers{
    public static class RoomClassMapper
    {
      // Phương thức để chuyển đổi từ RoomClass sang RoomClassDto
     public static RoomClassDto ToRoomClassDto(this RoomClass roomclassModel)
    {

        return new RoomClassDto
        {
            Id = roomclassModel.Id,
            ClassName = roomclassModel.ClassName,
            BasePrice = roomclassModel.BasePrice,
            Capacity = roomclassModel.Capacity,
            CreatedAt = roomclassModel.CreatedAt,
            CreatedBy = roomclassModel?.CreatedBy == null
                        ? null
                        : new UserInfo
                        {
                            Id = roomclassModel.CreatedBy.Id,
                            FirstName = roomclassModel.CreatedBy.FirstName,
                            LastName = roomclassModel.CreatedBy.LastName,
                            Email = roomclassModel.CreatedBy.Email,
                        },

        };
    }

}
}