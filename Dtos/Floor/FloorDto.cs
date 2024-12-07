using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Floor
{
    public class FloorDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Số tầng không được để trống")]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Số tầng chỉ được chứa chữ in hoa và số")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Số tầng phải từ 1-10 ký tự")]
        public string FloorNumber { get; set; } = string.Empty;
    }
}