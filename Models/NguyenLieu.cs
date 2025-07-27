using System.ComponentModel.DataAnnotations;

namespace Web_Manage.Models
{
    public class NguyenLieu
    {
        [Key]
        public int IdNguyenLieu { get; set; }
        public string TenNguyenLieu { get; set; }
        public int DonViTinh { get; set; }
        public float SoLuongTon { get; set; }
        public string? GhiChu { get; set; }
        public bool TrangThai { get; set; } = true; // Mặc định là còn sử dụng
    }
}
