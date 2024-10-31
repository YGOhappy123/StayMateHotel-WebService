using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
