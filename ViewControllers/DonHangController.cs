using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web_Manage.Models;
using Web_Manage.ViewModels;

namespace Web_Manage.ViewControllers
{
    public class DonHangController : Controller
    {
        private readonly HttpClient _httpClient;

        public DonHangController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        [HttpGet]
        public async Task<IActionResult> IndexDonHang()
        {
            var response = await _httpClient.GetAsync("api/DonHangApi");
            var data = await response.Content.ReadFromJsonAsync<List<DonHang>>();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> IndexLanDatHang()
        {
            var response = await _httpClient.GetAsync("api/LanDatHangApi");
            var data = await response.Content.ReadFromJsonAsync<List<LanDatHang>>();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> IndexChiTietDatHang()
        {
            var response = await _httpClient.GetAsync("api/ChiTietDatHangApi");
            var data = await response.Content.ReadFromJsonAsync<List<ChiTietDatHang>>();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int idkhachHang)
        {
            var response = await _httpClient.GetAsync("api/MatHangApi/active");
            var danhSachMatHang = await response.Content.ReadFromJsonAsync<List<MatHang>>();

            var vm = new DatHangViewModel
            {
                IdKhachHang = idkhachHang,
                DanhSachMatHang = danhSachMatHang,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DatHangViewModel model)
        {
            // 1. Parse số lượng trước – tránh lỗi nếu định dạng sai
            List<float> soLuongs;
            try
            {
                soLuongs = model.SoLuongText
                    .Split(" + ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim().Replace(",", "."))
                    .Select(x => float.Parse(x, System.Globalization.CultureInfo.InvariantCulture))
                    .ToList();
            }
            catch
            {
                ModelState.AddModelError("SoLuongText", "Định dạng số lượng không hợp lệ.");
                return View(model); // hoặc return BadRequest nếu là API
            }

            // 2. Lấy thông tin mặt hàng (trước khi tạo gì hết)
            var resMH = await _httpClient.GetAsync($"api/MatHangApi/{model.IdMatHang}");
            if (!resMH.IsSuccessStatusCode)
                return BadRequest("Không tìm thấy mặt hàng");

            var matHang = await resMH.Content.ReadFromJsonAsync<MatHang>();

            float tongSoLuong = 0;
            float tongTien = 0;

            var chiTietList = new List<ChiTietDatHang>();

            foreach (var sl in soLuongs)
            {
                tongSoLuong += sl;
                tongTien += sl * matHang.Gia;

                chiTietList.Add(new ChiTietDatHang
                {
                    IdMatHang = model.IdMatHang,
                    SoLuong = sl,
                    TongTien = sl * matHang.Gia,
                    TrangThai = 0
                });
            }

            // 3. Tạo đơn hàng (nếu chưa có)
            var ngay = DateOnly.FromDateTime(DateTime.Today);
            var url = $"api/DonHangApi/ByIdAndDate/{model.IdKhachHang}/{ngay:yyyy-MM-dd}";
            var response = await _httpClient.GetAsync(url);
            Console.WriteLine($"Controller ➡️ IdKhachHang được gửi lên: {model.IdKhachHang}");

            DonHang donHang;
            if (!response.IsSuccessStatusCode)
            {
                donHang = new DonHang
                {
                    IdKhachHang = model.IdKhachHang,
                    NgayDat = ngay,
                    TongTien = 0
                };
                var createRes = await _httpClient.PostAsJsonAsync("api/DonHangApi", donHang);
                if (!createRes.IsSuccessStatusCode)
                    return BadRequest("Tạo đơn hàng thất bại");

                donHang = await createRes.Content.ReadFromJsonAsync<DonHang>();
            }
            else
            {
                donHang = await response.Content.ReadFromJsonAsync<DonHang>();
            }

            // 4. Tạo lần đặt hàng
            var lanDatHang = new LanDatHang
            {
                IdDonHang = donHang.IdDonHang,
                ThoiGian = TimeOnly.FromDateTime(DateTime.Now),
                TongTien = 0,
                SoLuong = 0
            };

            var resLan = await _httpClient.PostAsJsonAsync("api/LanDatHangApi", lanDatHang);
            if (!resLan.IsSuccessStatusCode)
                return BadRequest("Tạo lần đặt hàng thất bại");

            lanDatHang = await resLan.Content.ReadFromJsonAsync<LanDatHang>();

            // 5. Tạo từng chi tiết đặt hàng
            foreach (var ct in chiTietList)
            {
                ct.IdLanDatHang = lanDatHang.IdLanDatHang;

                var resCT = await _httpClient.PostAsJsonAsync("api/ChiTietDatHangApi", ct);
                if (!resCT.IsSuccessStatusCode)
                    return BadRequest("Tạo chi tiết đặt hàng thất bại");
            }

            // 6. Cập nhật lại Lần đặt hàng
            var updateLan = new LanDatHang
            {
                IdLanDatHang = lanDatHang.IdLanDatHang,
                IdDonHang = lanDatHang.IdDonHang,
                ThoiGian = lanDatHang.ThoiGian,
                SoLuong = tongSoLuong,
                TongTien = 0
            };

            var resUpdateLan = await _httpClient.PutAsJsonAsync("api/LanDatHangApi", updateLan);
            if (!resUpdateLan.IsSuccessStatusCode)
                return BadRequest("Cập nhật lần đặt hàng thất bại");

            // 7. Cập nhật tổng đơn hàng
            var updateDonHang = new DonHang
            {
                IdDonHang = donHang.IdDonHang,
                IdKhachHang = donHang.IdKhachHang,
                NgayDat = donHang.NgayDat,
                TongTien = 0
            };

            var resUpdateDH = await _httpClient.PutAsJsonAsync("api/DonHangApi", updateDonHang);
            if (!resUpdateDH.IsSuccessStatusCode)
                return BadRequest("Cập nhật đơn hàng thất bại");

            return RedirectToAction("DetailDonHang", new { id = model.IdKhachHang });
        }

        [HttpGet]
        public async Task<IActionResult> DetailDonHang(int id)
        {
            var response = await _httpClient.GetAsync($"api/DonHangApi/ById/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var data = await response.Content.ReadFromJsonAsync<List<DonHang>>();

            if (data == null)
            {
                Console.WriteLine("❌ Không đọc được dữ liệu từ API.");
            }
            else
            {
                Console.WriteLine($"✅ Nhận được {data.Count} đơn hàng.");
            }

            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> DetailLanDatHang(int id)
        {
            var response = await _httpClient.GetAsync($"api/LanDatHangApi/ById/{id}");
            
            var data = await response.Content.ReadFromJsonAsync<List<LanDatHang>>();

            if (data == null)
            {
                Console.WriteLine("❌ Không đọc được dữ liệu từ API.");
            }
            else
            {
                Console.WriteLine($"✅ Nhận được {data.Count} lần đặt hàng.");
            }

            return View(data);
        }
    }
}
