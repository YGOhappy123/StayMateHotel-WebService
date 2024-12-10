using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.RoomClass
{
    public class RoomClassDto
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Capacity { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedById { get; set; }
    }
}