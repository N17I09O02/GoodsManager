using Microsoft.AspNetCore.Mvc;
using Web_Manage.Services;
using Web_Manage.Models;
namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoiTacAPIController : ControllerBase
    {
        private readonly DoiTacService doiTacService;
        public DoiTacAPIController(DoiTacService doiTacService)
        {
            this.doiTacService = doiTacService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doiTacs = await doiTacService.GetAllAsync();
            if (doiTacs == null || !doiTacs.Any())
                return NotFound("Không có đối tác nào trong hệ thống");
            return Ok(doiTacs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doiTac = await doiTacService.GetByIdAsync(id);
            if (doiTac == null)
                return NotFound();
            return Ok(doiTac);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoiTac doiTac)
        {
            if (doiTac == null)
                return BadRequest("Đối tác không được để trống");

            await doiTacService.AddAsync(doiTac);
            Console.WriteLine($"API ➡️ Tạo đối tác với ID = {doiTac.IdDoiTac}");
            return Ok(doiTac);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DoiTac doiTac)
        {
            if (doiTac == null || id != doiTac.IdDoiTac)
                return BadRequest("Thông tin không hợp lệ");

            await doiTacService.UpdateAsync(doiTac);
            Console.WriteLine($"API ➡️ Cập nhật đối tác với ID = {doiTac.IdDoiTac}");
            return Ok(doiTac);
        }

    }
}
