using Microsoft.AspNetCore.Mvc;
using Web_Manage.Models;
using Web_Manage.Services;
using Web_Manage.ViewModels;

namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoiTacNguyenLieuAPIController : ControllerBase
    {
        private readonly DoiTacNguyenLieuService _doiTacNguyenLieuService;
        public DoiTacNguyenLieuAPIController(DoiTacNguyenLieuService doiTacNguyenLieuService)
        {
            _doiTacNguyenLieuService = doiTacNguyenLieuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var doiTacNguyenLieu = await _doiTacNguyenLieuService.GetAsync(id);
            if (doiTacNguyenLieu == null)
            {
                return NotFound();
            }
            return Ok(doiTacNguyenLieu);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdDoiTac(int id)
        {
            var doiTacNguyenLieu = await _doiTacNguyenLieuService.GetByIdDoiTacAsync(id);
            if (doiTacNguyenLieu == null)
            {
                return NotFound();
            }
            return Ok(doiTacNguyenLieu);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdNguyenLieu(int idNguyenLieu)
        {
            var doiTacNguyenLieu = await _doiTacNguyenLieuService.GetByIdNguyenLieuAsync(idNguyenLieu);
            if (doiTacNguyenLieu == null)
            {
                return NotFound();
            }
            return Ok(doiTacNguyenLieu);
        }

        [HttpGet("{idNguyenLieu}/{IdDoiTac}")]
        public async Task<IActionResult> GetIdNLDT(int idNguyenLieu, int IdDoiTac)
        {
            var doiTacNguyenLieu = await _doiTacNguyenLieuService.GetByIdAsync(IdDoiTac, idNguyenLieu);
            if (doiTacNguyenLieu == null)
            {
                return NotFound();
            }
            return Ok(doiTacNguyenLieu);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoiTacNguyenLieu dto)
        {
            if (dto.IdDoiTac == 0 || dto.IdNguyenLieu == 0)
                return BadRequest("Thiếu thông tin nguyên liệu hoặc đối tác.");

            await _doiTacNguyenLieuService.AddAsync(dto);
            return Ok(dto);
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DoiTacNguyenLieu dto)
        {
            if (dto == null)
                return BadRequest("Thông tin rỗng.");

            var entity = await _doiTacNguyenLieuService.GetByIdAsync(dto.IdDoiTac, dto.IdNguyenLieu);
            if (entity == null)
                return NotFound("Không tìm thấy bản ghi để cập nhật.");

            // Cập nhật dữ liệu
            entity.Gia = dto.Gia;
            entity.TrangThai = dto.TrangThai;

            await _doiTacNguyenLieuService.UpdateAsync(entity);
            return Ok(entity);
        }

    }
}
