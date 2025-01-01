using System;
using System.Collections.Generic;
using server.Models;

namespace server.Dtos.Feature
{
    public class FeatureDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedById { get; set; }
        public AdminInfo? CreatedBy { get; set; }
        public List<FeatureRoomClassInfo>? RoomClasses { get; set; } = [];
    }

    public class AdminInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
    }


    public class FeatureRoomClassInfo
    {
        public int? RoomClassId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
