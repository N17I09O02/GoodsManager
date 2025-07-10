using System.ComponentModel.DataAnnotations;

namespace Web_Manage.Models
{
    public class MatHang
    {
        [Key]
        public int IdMatHang { get; set; }
        [Required]
        public string TenMatHang { get; set; }
        [Required]
        public float Gia { get; set; }

        public Boolean TrangThai { get; set; } = true; // true = Còn hàng, false = Hết hàng
    }

    public class LichSuMatHang
    {
        [Key]
        public DateTime ThoiGian { get; set; }
        [Required]
        public int IdMatHang { get; set; }
        [Required]
        public float Gia { get; set; } // Giá của mặt hàng tại thời điểm ghi nhận
        [Required]
        public string TrangThai { get; set; } // Trạng thái của mặt hàng (ví dụ: "Còn hàng", "Hết hàng", "Giá thay đổi")
    }
}
