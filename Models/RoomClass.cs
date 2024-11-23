using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class RoomClass
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }
        public int Capacity { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedById { get; set; }
        public Admin? CreatedBy { get; set; }
        public List<Room> Rooms { get; set; } = [];
        public List<RoomClassFeature> RoomClassFeatures { get; set; } = [];
    }
}
