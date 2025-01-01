using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Models
{
    public class Admin : AppUser
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public int? CreatedById { get; set; }
        public Admin? CreatedBy { get; set; }
        public List<Admin> CreatedAdmins { get; set; } = [];
        public List<Room> CreatedRooms { get; set; } = [];
        public List<RoomClass> CreatedRoomClasses { get; set; } = [];
        public List<Floor> CreatedFloors { get; set; } = [];
        public List<Feature> CreatedFeatures { get; set; } = [];
        public List<Service> CreatedServices { get; set; } = [];
    }
}
