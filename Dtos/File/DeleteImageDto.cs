using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.File
{
    public class DeleteImageDto
    {
        [Required]
        [Url]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
