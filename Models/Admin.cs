using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Models
{
    public class Admin : AppUser
    {
        public Gender Gender { get; set; } = Gender.Male;
        public int? CreatedById { get; set; }
        public Admin? CreatedBy { get; set; }
        public List<Admin> CreatedAdmins { get; set; } = [];
    }
}
