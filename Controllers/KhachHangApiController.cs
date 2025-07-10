using Microsoft.AspNetCore.Mvc;
using Web_Manage.Models;
using Web_Manage.Services;

namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KhachHangApiController : Controller
    {
        private readonly KhachHangService _service;
        private readonly DoanhNghiepService _doanhNghiepService;
        public KhachHangApiController(KhachHangService service, DoanhNghiepService doanhNghiepService)
        {
            _service = service;
            _doanhNghiepService = doanhNghiepService;
        }

        [HttpGet]
        public IActionResult Index() => Ok(_service.GetAllAsync().Result);

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var khachHang = await _service.GetByIdAsync(id);
            if (khachHang == null)
                return NotFound();
            return Ok(khachHang);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KhachHang khachHang)
        {
            if (khachHang == null)
                return BadRequest("Khách hàng không được để trống.");
            await _service.AddAsync(khachHang);
            return CreatedAtAction(nameof(GetById), new { id = khachHang.IdKhachHang }, khachHang);
        }

        [HttpPost("CreateDoanhNghiep")]
        public async Task<IActionResult> CreateDoanhNghiep([FromBody] DoanhNghiep doanhNghiep)
        {
            if (doanhNghiep == null)
                return BadRequest("Doanh nghiệp không được để trống.");
            await _doanhNghiepService.AddDoanhNghiepAsync(doanhNghiep);
            return CreatedAtAction(nameof(GetById), new { id = doanhNghiep.IdKhachHang }, doanhNghiep);
        }

        [HttpGet("DoanhNghiep/{id}")]
        public async Task<IActionResult> GetDoanhNghiepById(int id)
        {
            var doanhNghiep = await _doanhNghiepService.GetByIdAsync(id);
            if (doanhNghiep == null)
                return NotFound();
            return Ok(doanhNghiep);
        }

        [HttpPut("UpdateDoanhNghiep/{id}")]
        public async Task<IActionResult> UpdateDoanhNghiep(int id)
        {
            return NotFound("Chức năng cập nhật doanh nghiệp chưa được triển khai.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] KhachHang khachHang)
        {
            if (khachHang == null || khachHang.IdKhachHang != id)
                return BadRequest("Thông tin khách hàng không hợp lệ.");
            var existingKhachHang = await _service.GetByIdAsync(id);
            if (existingKhachHang == null)
                return NotFound();
            await _service.UpdateAsync(khachHang);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingKhachHang = await _service.GetByIdAsync(id);
            if (existingKhachHang == null)
                return NotFound();
            await _service.DeleteAsync(id);
            return NoContent();
        }

    }
}
