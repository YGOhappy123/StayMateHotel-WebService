using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedById { get; set; }
        public Admin? CreatedBy { get; set; }
        public List<RoomClassFeature> RoomClassFeatures { get; set; } = [];
    }
}
