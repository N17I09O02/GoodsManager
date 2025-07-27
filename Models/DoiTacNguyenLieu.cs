using System.ComponentModel.DataAnnotations;

namespace Web_Manage.Models
{
    public class DoiTacNguyenLieu
    {
        [Key]
        public int IdDTMH { get; set; }
        public int IdDoiTac { get; set; }
        public int IdNguyenLieu { get; set; }
        public float Gia { get; set; }
        public bool TrangThai { get; set; } = true;
    }
}
