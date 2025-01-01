using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.User
{
    public class UpdateAdminDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
        public string? Avatar { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
