using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.RoomClass
{
    public class RoomClassDto
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Capacity { get; set; }
        public DateTime CreatedAt { get; set; }
        public AdminInfo? CreatedBy { get; set; }
        public int? CreateById { get; set; }
        public List<RoomClassFeatureInfo>? Features { get; set; } = [];
    }

    public class AdminInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
    }

    public class RoomClassFeatureInfo
    {
        public int? FeatureId { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
    }
}
