using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web_Manage.Models;

namespace Web_Manage.ViewControllers
{
    public class NguyenLieuController : Controller
    {
        private readonly HttpClient _httpClient;

        public NguyenLieuController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/NguyenLieuAPI");
            if (!response.IsSuccessStatusCode)
            {
                // Đọc lỗi trả về (vì không phải JSON)
                var errText = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❌ API lỗi: " + response.StatusCode + "\n" + errText);
                return View(new List<NguyenLieu>());
            }
            var data = await response.Content.ReadFromJsonAsync<List<NguyenLieu>>();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NguyenLieu nguyenLieu)
        {
            if (nguyenLieu == null)
            {
                ModelState.AddModelError("", "Nguyên liệu không được để trống");
                return View(nguyenLieu);
            }

            var response = await _httpClient.PostAsJsonAsync("api/NguyenLieuAPI", nguyenLieu);
            if (!response.IsSuccessStatusCode)
            {
                var errText = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❌ API lỗi: " + response.StatusCode + "\n" + errText);
                ModelState.AddModelError("", "Lỗi khi tạo nguyên liệu");
                return View(nguyenLieu);
            }

            Console.WriteLine($"API ➡️ Tạo nguyên liệu với ID = {nguyenLieu.IdNguyenLieu}");
            return RedirectToAction("Index");
        }
    }
}
