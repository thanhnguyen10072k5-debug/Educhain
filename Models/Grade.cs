using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Educhain.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        // Khóa ngoại liên kết Sinh viên
        [Required(ErrorMessage = "Vui lòng chọn sinh viên")]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        // Khóa ngoại liên kết Môn học
        [Required(ErrorMessage = "Vui lòng chọn môn học")]
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm số")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải từ 0.0 đến 10.0")]
        [Column(TypeName = "decimal(4,2)")]
        [Display(Name = "Điểm số")]
        public decimal Score { get; set; }

        [Display(Name = "Học kỳ")]
        public string Semester { get; set; } = "Học kỳ 1 (2026 - 2027)";

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        // Cột chuẩn bị sẵn cho Blockchain
        [Display(Name = "Mã băm giao dịch (TxHash)")]
        public string? BlockchainTxHash { get; set; }

        public bool IsVerifiedOnChain { get; set; } = false;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}