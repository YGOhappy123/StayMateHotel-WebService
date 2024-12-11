using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Room;

namespace server.Dtos.Floor
{
    public class FloorDto
    {
        public int Id { get; set; }
        public string FloorNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public UserInfo? CreatedBy { get; set; }
        public List<string>? Rooms { get; set; } = [];
    }
}
