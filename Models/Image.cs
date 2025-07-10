using System.ComponentModel.DataAnnotations;

namespace Web_Manage.Models
{
    public class Image
    {
        [Key]
        public int IdImage { get; set; }
        public int? IdLanDatHang { get; set; }
        public int? IdDonHang { get; set; }
        public string URL { get; set; }
    }
}
