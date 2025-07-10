using System.ComponentModel.DataAnnotations;

namespace Web_Manage.Models
{
    public class DonHang
    {
        [Key]
        public int IdDonHang { get; set; }
        public DateOnly NgayDat { get; set; }
        public int IdKhachHang { get; set; }
        public double TongTien { get; set; }
        public bool ThanhToan { get; set; } = false; // Mặc định là chưa thanh toán
    }

    public class LanDatHang
    {
        [Key]
        public int IdLanDatHang { get; set; }
        public int IdDonHang { get; set; }
        public TimeOnly ThoiGian { get; set; }
        public float SoLuong { get; set; }
        public double TongTien { get; set; }
        public bool ThanhToan { get; set; } = false;
    }

    public class ChiTietDatHang
    {
        [Key]
        public int IdChiTietDonHang { get; set; }
        public int IdLanDatHang { get; set; }
        public int IdMatHang { get; set; }
        public float SoLuong { get; set; }
        public float TongTien { get; set; }
        public int TrangThai { get; set; }
    }
}
