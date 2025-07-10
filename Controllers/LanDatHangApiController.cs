using Microsoft.AspNetCore.Mvc;
using Web_Manage.Services;
using Web_Manage.Models;
namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanDatHangApiController : Controller
    {
        private readonly LanDatHangService _service;
        private readonly DonHangService _donHangService;
        private readonly KhachHangService _khachHangService;
        public LanDatHangApiController(LanDatHangService service, DonHangService donHangService, KhachHangService khachHangService)
        {
            _service = service;
            _donHangService = donHangService;
            _khachHangService = khachHangService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LanDatHang lanDatHang) // Gửi JSON => cần [FromBody]
        {
            await _service.AddAsync(lanDatHang);
            return Ok(lanDatHang);
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lanDatHang = await _service.GetByIdDonHangAsync(id);
            return Ok(lanDatHang);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LanDatHang lanDatHang)
        {
            await _service.UpdateAsync(lanDatHang);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateThanhToan/{id}/{tongTien}")]
        public async Task<IActionResult> UpdateThanhToan(int id, bool trangthai)
        {
            var lanDatHang = await _service.GetByIdAsync(id);
            var donHang = await _donHangService.GetByIdAsync(lanDatHang.IdDonHang);
            var khachHang = await _khachHangService.GetByIdAsync(donHang.IdKhachHang);

            if(trangthai)
            {
                donHang.TongTien -= lanDatHang.TongTien;
                khachHang.TienNo -= lanDatHang.TongTien;
                await _donHangService.UpdateTienAsync(donHang.IdDonHang, donHang.TongTien);
                await _khachHangService.UpdateTienAsync(khachHang.IdKhachHang, khachHang.TienNo ?? 0);
            }
            else
            {
                donHang.TongTien += lanDatHang.TongTien;
                khachHang.TienNo += lanDatHang.TongTien;
                await _donHangService.UpdateTienAsync(donHang.IdDonHang, donHang.TongTien);
                await _khachHangService.UpdateTienAsync(khachHang.IdKhachHang, khachHang.TienNo ?? 0);
            }    

            await _service.UpdateTrangThai(id, trangthai);
            
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
