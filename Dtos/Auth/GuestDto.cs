using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Auth;
using server.Enums;

namespace server.Dtos.Auth
{
    public class GuestDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string Role { get; } = UserRole.Guest.ToString();
    }
}
