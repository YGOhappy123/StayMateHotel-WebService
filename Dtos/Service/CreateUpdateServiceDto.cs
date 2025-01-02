using System;
using System.ComponentModel.DataAnnotations;

namespace server.Dtos.Service
{
    public class CreateUpdateServiceDto
    {
        [Required] // Yêu cầu nhập tên dịch vụ
        public string Name { get; set; } = string.Empty;

        [Required] // Yêu cầu nhập giá dịch vụ
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required] // Yêu cầu biết dịch vụ có sẵn hay không
        public bool IsAvailable { get; set; }

        [Required] // Yêu cầu nhập thời gian tạo dịch vụ
        public DateTime CreatedAt { get; set; }

        [Required] // Yêu cầu nhập ID của người tạo (Admin)
        public int CreatedById { get; set; }
    }
}
