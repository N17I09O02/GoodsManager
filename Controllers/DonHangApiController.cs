using Microsoft.AspNetCore.Mvc;
using Web_Manage.Services;
using Web_Manage.Models;
namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonHangApiController : Controller
    {
        private readonly DonHangService _service;
        private readonly KhachHangService _khachHangService;
        public DonHangApiController(DonHangService service, KhachHangService khachHangService)
        {
            _service = service;
            _khachHangService = khachHangService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync(DateOnly.FromDateTime(DateTime.Now)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DonHang donHang) // Gửi JSON => cần [FromBody]
        {
            await _service.AddAsync(donHang);
            Console.WriteLine($"API ➡️ Tạo đơn hàng cho khách hàng ID = {donHang.IdKhachHang}");
            return Ok(donHang);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var donHang = await _service.GetByIdAsync(id);
            if (donHang == null)
                return NotFound();
            return Ok(donHang);
        }

        [HttpGet("ByIdAndDate/{id}/{ngay}")]
        public async Task<IActionResult> GetByIdAndDate(int id, DateOnly ngay)
        {
            var donHang = await _service.GetByIdKHAnDateAsync(id, ngay);
            if (donHang == null)
                return NotFound();
            return Ok(donHang);
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetByIdKhachHang(int id)
        {
            var donHang = await _service.GetByIdKhachHangAsync(id);
            return Ok(donHang);
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DonHang donHang)
        {
            await _service.UpdateAsync(donHang);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateThanhToan/{id}/{thanhtoan}")]
        public async Task<IActionResult> UpdateThanhToan(int id, bool thanhtoan)
        {
            var donhang = await _service.GetByIdAsync(id);
            var khachhang = await _khachHangService.GetByIdAsync(donhang.IdKhachHang);

            if (khachhang.TienNo == null)
                khachhang.TienNo = 0;

            var ngayDat = donhang.NgayDat.ToDateTime(TimeOnly.MinValue);
            var homNay = DateTime.Now;

            if (thanhtoan)
            {
                khachhang.TienNo -= donhang.TongTien;
                // Lưu cập nhật cho khách hàng
                await _khachHangService.UpdateTruTienAsync(khachhang.IdKhachHang, donhang.TongTien);
            }
            else if (!thanhtoan && (homNay - ngayDat).TotalDays <= 2)
            {
                khachhang.TienNo += donhang.TongTien;
                // Lưu cập nhật cho khách hàng
                await _khachHangService.UpdateTienAsync(khachhang.IdKhachHang, donhang.TongTien);
            }

            // Cập nhật lại trạng thái thanh toán
            await _service.UpdateThanhToan(id, thanhtoan);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
