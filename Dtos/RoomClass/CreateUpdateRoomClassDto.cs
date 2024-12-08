using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.RoomClass
{
    public class CreateUpdateRoomClassDto
    {
        // DTO dùng để tạo mới
        public class CreateRoomClassDto
        {
            public string ClassName { get; set; } = string.Empty;
            public decimal BasePrice { get; set; }
            public int Capacity { get; set; }
        }

        // DTO dùng để cập nhật
        public class UpdateRoomClassDto
        {
            public int Id { get; set; }
            public string ClassName { get; set; } = string.Empty;
            public decimal BasePrice { get; set; }
            public int Capacity { get; set; }
        }
    
    }
}