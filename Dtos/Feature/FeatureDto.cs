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
        public UserInfo? CreatedBy { get; set; }
        public List<FeatureRoomClassInfo>? RoomClass { get; set; } = new List<FeatureRoomClassInfo>();
    }

    public class FeatureRoomClassInfo
    {
        public int? Id { get; set; }
        public string? ClassName { get; set; }
        public int Quantity { get; set; } = 1;
    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
