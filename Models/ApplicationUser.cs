using Microsoft.AspNetCore.Identity;

namespace Educhain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? WalletAddress { get; set; } // Chuẩn bị sẵn cho địa chỉ ví MetaMask
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}