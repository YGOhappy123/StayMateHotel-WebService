using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Auth
{
    public class DeactivateAccountDto
    {
        [Required]
        public int TargetUserId { get; set; }

        [Required]
        public string TargetUserRole { get; set; } = string.Empty;
    }
}
