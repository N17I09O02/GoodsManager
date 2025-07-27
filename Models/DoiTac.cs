using System.ComponentModel.DataAnnotations;

namespace Web_Manage.Models
{
    public class DoiTac
    {
        [Key]
        public int IdDoiTac { get; set; }
        public string TenDoiTac { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string? Email { get; set; }
        public bool TrangThai { get; set; } = true; // Mặc định là hoạt động
    }
}
