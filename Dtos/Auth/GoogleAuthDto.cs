using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Auth
{
    public class GoogleAuthDto
    {
        [Required]
        public string GoogleAccessToken { get; set; } = string.Empty;
    }
}
