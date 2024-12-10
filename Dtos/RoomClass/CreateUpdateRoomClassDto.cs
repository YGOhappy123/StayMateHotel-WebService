using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.RoomClass
{
    public class CreateUpdateRoomClassDto
    {
    
            [Required]
            public string ClassName { get; set; } = string.Empty;
            [Required]
            public decimal BasePrice { get; set; }
            [Required]
            public int Capacity { get; set; }
            [Required]
            public List<RoomClassFeatureDto> Features { get; set; } = new List<RoomClassFeatureDto>();

       
    
    }

    public class RoomClassFeatureDto
    {
        [Required]
        public int FeatureId {get; set;}
        [Required]
        public int Quantity {get; set;}
    }
}