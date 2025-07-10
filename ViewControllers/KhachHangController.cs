using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Web_Manage.Models;

namespace Web_Manage.ViewControllers
{
    public class KhachHangController : Controller
    {
        private readonly HttpClient _httpClient;

        public KhachHangController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        // Hiển thị danh sách
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/KhachHangApi");
            var data = await response.Content.ReadFromJsonAsync<List<KhachHang>>();
            return View(data);
        }

        // Hiển thị form thêm mới
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(KhachHang khachHang)
        {
            if (khachHang == null)
                return BadRequest("Khách hàng không được để trống.");

            var response = await _httpClient.PostAsJsonAsync("api/KhachHangApi", khachHang);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(khachHang);
        }

        // Hiển thị form chỉnh sửa
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/KhachHangApi/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var data = await response.Content.ReadFromJsonAsync<KhachHang>();
            return View(data);
        }

        // Xử lý submit cập nhật
        [HttpPost]
        public async Task<IActionResult> Edit(KhachHang khachHang)
        {
            if (khachHang == null || khachHang.IdKhachHang <= 0)
                return BadRequest("Thông tin khách hàng không hợp lệ.");

            var response = await _httpClient.PutAsJsonAsync($"api/KhachHangApi/{khachHang.IdKhachHang}", khachHang);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(khachHang);
        }
        // Xóa
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/KhachHangApi/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return NotFound();
        }

        /*[HttpGet]
        public async Task<IActionResult> IndexDoanhNghiep(int idKhachHang)
        {
            var response = await _httpClient.GetAsync($"api/KhachHangApi/DoanhNghiep/{idKhachHang}");
            return 
        }*/

        //Hiển thị form thêm doanh nghiệp
        [HttpGet]
        public IActionResult CreateDoanhNghiep() => View();

        [HttpPost]
        public async Task<IActionResult> CreateDoanhNghiep(DoanhNghiep doanhNghiep)
        {
            if (doanhNghiep == null)
                return BadRequest("Doanh nghiệp không được để trống.");

            var response = await _httpClient.PostAsJsonAsync("api/KhachHangApi/CreateDoanhNghiep", doanhNghiep);
            if (response.IsSuccessStatusCode)

                return RedirectToAction("Index");
            return View(doanhNghiep);
        }
    }
}
