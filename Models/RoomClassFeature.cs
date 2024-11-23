using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class RoomClassFeature
    {
        public int? RoomClassId { get; set; }
        public RoomClass? RoomClass { get; set; }
        public int? FeatureId { get; set; }
        public Feature? Feature { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
