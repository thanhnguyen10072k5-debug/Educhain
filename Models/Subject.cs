using System.ComponentModel.DataAnnotations;

namespace Educhain.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã môn học")]
        [Display(Name = "Mã môn học")]
        public string SubjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tên môn học")]
        [Display(Name = "Tên môn học")]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số tín chỉ")]
        [Range(1, 10, ErrorMessage = "Số tín chỉ phải từ 1 đến 10")]
        [Display(Name = "Số tín chỉ")]
        public int Credits { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}