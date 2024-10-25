using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public int BasePrice { get; set; }
        public List<Room> Rooms { get; set; } = [];
    }
}
