using System.ComponentModel.DataAnnotations;

namespace Web_Manage.Models
{
    public class KhachHang
    {
        [Key]
        public int IdKhachHang { get; set; }
        [Required]
        public string TenKhachHang { get; set; }
        [Required]
        public string DiaChi { get; set; }
        [Required]
        public string SDT { get; set; }
        public string? Email { get; set; }
        public double? TienNo { get; set; } // Số tiền nợ của khách hàng
        public bool DoanhNghiep { get; set; } // true: Doanh nghiệp, false: Cá nhân
        // Thêm các thuộc tính khác nếu cần
    }
}
