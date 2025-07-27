using Web_Manage.Models;

namespace Web_Manage.ViewModels
{
    public class DoiTacNguyenLieuViewModel
    {
        public DoiTac DoiTac { get; set; }

        public List<NguyenLieuGiaViewModel> NguyenLieuGias { get; set; } = new();
    }

    public class NguyenLieuGiaViewModel
    {
        public int IdNguyenLieu { get; set; }
        public string TenNguyenLieu { get; set; }
        public bool DuocChon { get; set; }
        public float Gia { get; set; }
        public bool DangCungCap { get; set; }
    }


}
