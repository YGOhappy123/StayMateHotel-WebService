using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public RoomStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? FloorId { get; set; }
        public Floor? Floor { get; set; }
        public int? RoomClassId { get; set; }
        public RoomClass? RoomClass { get; set; }
        public int? CreatedById { get; set; }
        public Admin? CreatedBy { get; set; }
        public List<RoomImage> Images { get; set; } = [];
        public List<BookingRoom> BookingRooms { get; set; } = [];
    }
}
