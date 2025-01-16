using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.File
{
    public class UploadBase64ImageDto
    {
        [Required]
        public required string Base64Image { get; set; }
    }
}
