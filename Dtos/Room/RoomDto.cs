using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Dtos.Room
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public RoomStatus Status { get; set; }
        public int? FloorId { get; set; }
        public int? RoomTypeId { get; set; }
    }
}
