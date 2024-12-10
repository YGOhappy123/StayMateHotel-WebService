using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Floor
{
    public class FloorDto
    {
        public int Id { get; set; }
        public string FloorNumber { get; set; } = string.Empty;
        public List<string>? Rooms { get; set; } = [];
    }
}
