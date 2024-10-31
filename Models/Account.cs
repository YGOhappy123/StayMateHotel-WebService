using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Guest;
        public bool IsActive { get; set; } = true;
        public Guest? Guest { get; set; }
        public Admin? Admin { get; set; }
    }
}
