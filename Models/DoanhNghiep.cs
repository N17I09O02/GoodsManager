using System.ComponentModel.DataAnnotations;

namespace Web_Manage.Models
{
    public class DoanhNghiep
    {
        [Key]
        public int IdDoanhNghiep { get; set; }
        public string TenNguoiDaiDien { get; set; }
        public int MaSoThue { get; set; }
        public int IdKhachHang { get; set; } // Khóa ngoại đến KhachHang
    }
}
