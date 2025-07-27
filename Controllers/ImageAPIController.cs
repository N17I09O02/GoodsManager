using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Web_Manage.Models;
using Web_Manage.Services;

namespace Web_Manage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageAPIController : Controller
    {
        private readonly S3Service _s3Service;
        public ImageAPIController(S3Service s3Service)
        {
            _s3Service = s3Service;
        }

        [HttpGet("get-url/{id}")]
        public async Task<IActionResult> GetPresignedUrl(int id)
        {
            Console.WriteLine($"API ➡️ Lấy URL ảnh với ID = {id}");
            var image = await _s3Service.GetImagesByIdAsync(id);

            if (image == null || string.IsNullOrEmpty(image.URL))
                return NotFound("Không có ảnh");

            var url = _s3Service.GeneratePresignedUrl(image.URL); // Truyền key
            return Ok(new { url });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Image image)
        {
            Console.WriteLine($"API ➡️ Tạo ảnh với ID = {image.IdDonHang} hoặc {image.IdLanDatHang}");
            var save = await _s3Service.AddImageAsync(image);
            return Ok(save);
        }
    }
}
