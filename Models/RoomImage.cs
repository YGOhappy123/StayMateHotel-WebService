using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class RoomImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int? RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
