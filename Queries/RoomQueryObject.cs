using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Models
{
    public class RoomQueryObject
    {
        public string? RoomName { get; set; } = null;
        public RoomStatus? Status { get; set; }
        public int? FloorId { get; set; }
        public int? RoomTypeId { get; set; }
        public string? SortBy { get; set; } = null;
        public string? SortDirection { get; set; } = "ASC";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
