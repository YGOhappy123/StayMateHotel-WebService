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
        public FeatureCreatedByInfo? CreatedBy { get; set; }
        public List<FeatureRoomClassInfo>? RoomClassFeatures { get; set; } = new List<FeatureRoomClassInfo>();
    }

    public class FeatureCreatedByInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class FeatureRoomClassInfo
    {
        public int? Id { get; set; }
        public string? ClassName { get; set; } = string.Empty;
    }
}
