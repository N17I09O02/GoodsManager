using Web_Manage.Models;

namespace Web_Manage.ViewModels
{
    public class DatHangViewModel
    {
        public int IdKhachHang { get; set; }
        public int IdMatHang { get; set; }
        public string SoLuongText { get; set; }
        public List<MatHang> DanhSachMatHang { get; set;}
    }
}
