using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Floor
    {
        public int Id { get; set; }
        public int FloorNumber { get; set; }
        public List<Room> Rooms { get; set; } = [];
    }
}
