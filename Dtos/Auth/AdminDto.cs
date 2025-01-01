using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Dtos.Auth
{
    public class AdminDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Role { get; } = UserRole.Admin.ToString();
        public int? CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
