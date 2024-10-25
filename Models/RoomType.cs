using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal BasePrice { get; set; }
        public List<Room> Rooms { get; set; } = [];
    }
}
