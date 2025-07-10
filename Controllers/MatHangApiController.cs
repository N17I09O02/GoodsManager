using Microsoft.AspNetCore.Mvc;
using Web_Manage.Models; // nếu bạn để model ở đây
using Microsoft.EntityFrameworkCore;
using Web_Manage.Services;

namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatHangApiController : ControllerBase
    {
        private readonly MatHangService _service;

        public MatHangApiController(MatHangService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var activeItems = await _service.GetActiveAsync();
            return Ok(activeItems);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MatHang mathang) // Gửi JSON => cần [FromBody]
        {
            await _service.AddAsync(mathang);
            return Ok(mathang);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var matHang = await _service.GetByIdAsync(id);
            if (matHang == null)
                return NotFound();
            return Ok(matHang);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MatHang mathang)
        {
            await _service.UpdateAsync(mathang);
            return Ok();
        }

        [HttpGet("history/{time}")]
        public async Task<IActionResult> GetHistory(DateTime time)
        {
            var lichSu = await _service.GetHistory(time);
            if (lichSu == null)
                return NotFound();
            return Ok(lichSu);
        }

        [HttpPost("history")]
        public async Task<IActionResult> CreateHistory([FromBody] LichSuMatHang lichSu)
        {
            await _service.AddHistoryAsync(lichSu);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 DELETE ERROR: {ex.Message}");
                return StatusCode(500, "Lỗi server khi xóa mặt hàng.");
            }
        }

    }
}
