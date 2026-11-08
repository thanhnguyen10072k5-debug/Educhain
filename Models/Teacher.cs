using System.ComponentModel.DataAnnotations;

namespace Educhain.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã giảng viên")]
        [Display(Name = "Mã giảng viên")]
        public string TeacherCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Khoa / Bộ môn")]
        public string Department { get; set; } = "Công nghệ thông tin";

        [Display(Name = "Học vị")]
        public string AcademicRank { get; set; } = "Thạc sĩ";

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}