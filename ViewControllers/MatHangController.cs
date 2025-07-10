using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Web_Manage.Models;

namespace Web_Manage.ViewControllers
{
    public class MatHangController : Controller
    {
        private readonly HttpClient _httpClient;

        public MatHangController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        // Hiển thị danh sách
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/MatHangApi");
            var data = await response.Content.ReadFromJsonAsync<List<MatHang>>();
            return View(data);
        }

        // Hiển thị form thêm mới
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(MatHang mathang)
        {
            Console.WriteLine($"🔥 Tên: {mathang.TenMatHang}, Giá: {mathang.Gia}");
            var response = await _httpClient.PostAsJsonAsync("api/MatHangApi", mathang);

            if (response.IsSuccessStatusCode)
            {
                var history = new LichSuMatHang
                {
                    IdMatHang = mathang.IdMatHang,
                    Gia = mathang.Gia,
                    ThoiGian = DateTime.Now
                };
                await _httpClient.PostAsJsonAsync("api/MatHangApi/history", history);
                return RedirectToAction("Index");
            }    
            return View(mathang);
        }

        // Hiển thị form chỉnh sửa
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/MatHangApi/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var data = await response.Content.ReadFromJsonAsync<MatHang>();
            return View(data);
        }

        // Xử lý submit cập nhật
        [HttpPost]
        public async Task<IActionResult> Edit(MatHang mathang)
        {
            var response = await _httpClient.PutAsJsonAsync("api/MatHangApi", mathang);
            
            if (response.IsSuccessStatusCode)
            {
                var history = new LichSuMatHang
                {
                    IdMatHang = mathang.IdMatHang,
                    Gia = mathang.Gia,
                    ThoiGian = DateTime.Now
                };
                await _httpClient.PostAsJsonAsync("api/MatHangApi/history", history);
                return RedirectToAction("Index");
            }    
            return View(mathang);
        }
    }
}
