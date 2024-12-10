using System;
using System.ComponentModel.DataAnnotations;

namespace server.Dtos.Feature
{
    public class CreateUpdateFeatureDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
    }
}
