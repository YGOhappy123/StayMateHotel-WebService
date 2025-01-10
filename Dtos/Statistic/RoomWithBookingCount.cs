using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Statistic
{
    public class RoomWithBookingCount : Models.Room
    {
        public int BookingCount { get; set; }

        public RoomWithBookingCount(Models.Room room, int bookingCount)
        {
            this.Id = room.Id;
            this.RoomNumber = room.RoomNumber;
            this.Status = room.Status;
            this.CreatedAt = room.CreatedAt;
            this.FloorId = room.FloorId;
            this.Floor = room.Floor;
            this.RoomClassId = room.RoomClassId;
            this.RoomClass = room.RoomClass;
            this.CreatedById = room.CreatedById;
            this.CreatedBy = room.CreatedBy;
            this.Images = room.Images;
            this.BookingRooms = room.BookingRooms;
            this.BookingCount = bookingCount;
        }
    }
}
