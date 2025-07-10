using Microsoft.AspNetCore.Mvc;
using Web_Manage.Services;
using Web_Manage.Models;
namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiTietDatHangApiController : Controller
    {
        private readonly ChiTietDatHangService _service;
        private readonly LanDatHangService _lanDatHang;
        private readonly DonHangService _donHang;
        private readonly KhachHangService _khachHangService;
        public ChiTietDatHangApiController(ChiTietDatHangService service, LanDatHangService lanDatHangService, DonHangService donHang, KhachHangService khachHangService)
        {
            _service = service;
            _lanDatHang = lanDatHangService;
            _donHang = donHang;
            _khachHangService = khachHangService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChiTietDatHang chiTietDatHang) // Gửi JSON => cần [FromBody]
        {
            await _service.AddAsync(chiTietDatHang);
            return Ok(chiTietDatHang);
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetByIdLanDatHang(int id)
        {
            try
            {
                var chiTietDatHang = await _service.GetByIdLanDatHangAsync(id);
                return Ok(chiTietDatHang);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi API: " + ex.Message);
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ChiTietDatHang chiTiet)
        {
            var existing = await _service.GetByIdAsync(chiTiet.IdChiTietDonHang);
            if (existing == null) return NotFound();

            var landathang = await _lanDatHang.GetByIdAsync(existing.IdLanDatHang);
            var donhang = await _donHang.GetByIdAsync(landathang.IdDonHang);
            var khachhang = await _khachHangService.GetByIdAsync(donhang.IdKhachHang);

            // Nếu trạng thái thay đổi
            if (existing.TrangThai != chiTiet.TrangThai)
            {
                // Chuyển từ Đã giao → Chưa giao hoặc Huỷ → Trừ tiền
                if (existing.TrangThai == 1 && (chiTiet.TrangThai == 0 || chiTiet.TrangThai == 2))
                {
                    landathang.TongTien -= existing.TongTien;
                    donhang.TongTien -= existing.TongTien;
                }

                // Chuyển từ Chưa giao hoặc Huỷ → Đã giao → Cộng tiền
                if ((existing.TrangThai == 0 || existing.TrangThai == 2) && chiTiet.TrangThai == 1)
                {
                    landathang.TongTien += existing.TongTien;
                    donhang.TongTien += existing.TongTien;
                }
            }

            // ✅ Cập nhật trạng thái mới
            existing.TrangThai = chiTiet.TrangThai;
            await _service.UpdateAsync(existing);

            // ✅ Sau khi cập nhật, tính lại toàn bộ để đảm bảo chính xác
            var allChiTiet = await _service.GetByIdLanDatHangAsync(landathang.IdLanDatHang);
            landathang.TongTien = allChiTiet
                .Where(x => x.TrangThai == 1)
                .Sum(x => x.TongTien);

            await _lanDatHang.UpdateTienAsync(landathang.IdLanDatHang, landathang.TongTien);

            var allLanDat = await _lanDatHang.GetByIdDonHangAsync(donhang.IdDonHang);
            donhang.TongTien = allLanDat.Sum(x => x.TongTien);

            await _donHang.UpdateTienAsync(donhang.IdDonHang, donhang.TongTien);

            await _khachHangService.UpdateTienAsync(khachhang.IdKhachHang, donhang.TongTien);

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
