using Microsoft.AspNetCore.Mvc;
using Web_Manage.Models;
using Web_Manage.Services;

namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NguyenLieuAPIController : Controller
    {
        private readonly NguyenLieuService _service;
        public NguyenLieuAPIController(NguyenLieuService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var nguyenLieus = await _service.GetAllAsync();
            if (nguyenLieus == null || !nguyenLieus.Any())
                return NotFound("Không có nguyên liệu nào trong hệ thống");
            return Ok(nguyenLieus);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var nguyenLieu = await _service.GetByIdAsync(id);
            if (nguyenLieu == null)
                return NotFound();
            return Ok(nguyenLieu);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NguyenLieu nguyenLieu)
        {
            if (nguyenLieu == null)
                return BadRequest("Nguyên liệu không được để trống");

            await _service.AddAsync(nguyenLieu);
            Console.WriteLine($"API ➡️ Tạo nguyên liệu với ID = {nguyenLieu.IdNguyenLieu}");
            return Ok(nguyenLieu);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NguyenLieu nguyenLieu)
        {
            if (nguyenLieu == null)
                return BadRequest("Nguyên liệu không được để trống");

            await _service.UpdateAsync(nguyenLieu);
            Console.WriteLine($"API ➡️ Cập nhật nguyên liệu với ID = {nguyenLieu.IdNguyenLieu}");
            return Ok(nguyenLieu);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                Console.WriteLine($"API ➡️ Xóa nguyên liệu với ID = {id}");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa nguyên liệu: {ex.Message}");
                return NotFound(ex.Message);
            }
        }
    }
}
