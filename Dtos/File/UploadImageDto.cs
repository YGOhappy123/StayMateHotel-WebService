using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.File
{
    public class UploadImageDto
    {
        [Required]
        public required IFormFile File { get; set; }
    }
}
