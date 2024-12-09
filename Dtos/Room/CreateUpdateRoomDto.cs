using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Room
{
    public class CreateUpdateRoomDto
    {
        [Required]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        public int FloorId { get; set; }

        [Required]
        public int RoomClassId { get; set; }

        [Required]
        public List<string> Images { get; set; } = [];
    }
}
