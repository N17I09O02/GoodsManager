using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web_Manage.Models;
using Web_Manage.Services;

namespace Web_Manage.ViewControllers
{
    public class ImageController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly S3Service _s3Service;

        public ImageController(S3Service s3Service, IHttpClientFactory httpClientFactory, IOptions<ApiSettings> options)
        {
            _s3Service = s3Service;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, int idDonHang, int idLanDatHang)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.ImageUrl = null;
                ViewBag.Error = "File không hợp lệ";
                return View();
            }

            // 1. Upload lên S3
            var key = await _s3Service.UploadFileAsync(file);
            var url = _s3Service.GetFileUrl(key);
            Image image;
            // 2. Tạo bản ghi Image
            if (idDonHang == null || idDonHang == 0)
            {
                image = new Image
                {
                    IdLanDatHang = idLanDatHang,
                    URL = key
                };
            }    
            else if (idLanDatHang == null || idLanDatHang == 0)
            {
                image = new Image
                {
                    IdDonHang = idDonHang,
                    URL = key
                };
            }
            else
            {
                Console.WriteLine($"🔥 Không thể xác định ID đơn hàng hoặc ID lần đặt hàng: {idDonHang}, {idLanDatHang}");
                TempData["Error"] = "Không thể xác định ID đơn hàng hoặc ID lần đặt hàng.";
                return RedirectToAction("DetailLanDatHang", "DetailDonHang", new { idDonHang = idDonHang });
            }

            var response = await _httpClient.PostAsJsonAsync("api/ImageApi", image);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"🔥 Lưu ảnh thất bại");
                TempData["Error"] = "Lưu ảnh thất bại";
                return RedirectToAction("DetailLanDatHang", "DetailDonHang", new { idDonHang = idDonHang });
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
