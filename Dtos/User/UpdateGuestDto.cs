using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace server.Dtos.User
{
    public class UpdateGuestDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Url]
        public string? Avatar { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
