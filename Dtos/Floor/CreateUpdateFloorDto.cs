using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Floor
{
    public class CreateUpdateFloorDto
    {
        [Required]
        public String FloorNumber { get; set; } = String.Empty;
    }
}
