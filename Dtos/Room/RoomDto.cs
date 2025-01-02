using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Auth;
using server.Enums;
using server.Models;

namespace server.Dtos.Room
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Status { get; set; } = RoomStatus.Available.ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? FloorId { get; set; }
        public int? RoomClassId { get; set; }
        public int? CreatedById { get; set; }
        public RoomFloorInfo? Floor { get; set; }
        public RoomRoomClassInfo? RoomClass { get; set; }
        public UserInfo? CreatedBy { get; set; }
        public List<RoomFeatureInfo>? Features { get; set; } = [];
        public List<string>? Images { get; set; } = [];
    }

    public class RoomFloorInfo
    {
        public int Id { get; set; }
        public string FloorNumber { get; set; } = string.Empty;
    }

    public class RoomRoomClassInfo
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Capacity { get; set; }
    }

    public class RoomFeatureInfo
    {
        public int? FeatureId { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
