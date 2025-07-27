using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Extensions.Options;
using Web_Manage.Models;
using Web_Manage.ViewModels;

namespace Web_Manage.ViewControllers
{
    public class DoiTacController : Controller
    {
        private readonly HttpClient _httpClient;
        public DoiTacController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/DoiTacAPI");
            if (!response.IsSuccessStatusCode)
            {
                // Đọc lỗi trả về (vì không phải JSON)
                var errText = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❌ API lỗi: " + response.StatusCode + "\n" + errText);
                return View(new List<DoiTac>());
            }
            var data = await response.Content.ReadFromJsonAsync<List<DoiTac>>();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/DoiTacAPI/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ API lỗi: " + response.StatusCode);
                return NotFound();
            }

            var doiTac = await response.Content.ReadFromJsonAsync<Models.DoiTac>();
            return View(doiTac);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.GetAsync("api/NguyenLieuAPI");
            var nguyenLieuList = await response.Content.ReadFromJsonAsync<List<NguyenLieu>>();

            var viewModel = new DoiTacNguyenLieuViewModel
            {
                DoiTac = new DoiTac(),
                NguyenLieuGias = nguyenLieuList?
                    .Where(x => x.TrangThai)
                    .Select(x => new NguyenLieuGiaViewModel
                    {
                        IdNguyenLieu = x.IdNguyenLieu,
                        TenNguyenLieu = x.TenNguyenLieu,
                        DuocChon = false,
                        Gia = 0
                    }).ToList() ?? new()
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Create(DoiTacNguyenLieuViewModel model)
        {
            if (!ModelState.IsValid || model.DoiTac == null)
            {
                // Lấy lại danh sách nguyên liệu nếu form có lỗi
                var nguyenLieus = await _httpClient.GetFromJsonAsync<List<NguyenLieu>>("api/NguyenLieuAPI");
                model.NguyenLieuGias = nguyenLieus?
                    .Where(x => x.TrangThai)
                    .Select(x => new NguyenLieuGiaViewModel
                    {
                        IdNguyenLieu = x.IdNguyenLieu,
                        TenNguyenLieu = x.TenNguyenLieu,
                        DuocChon = false,
                        Gia = 0
                    }).ToList() ?? new();

                return View(model);
            }

            // Gửi tạo đối tác
            var response = await _httpClient.PostAsJsonAsync("api/DoiTacAPI", model.DoiTac);
            if (!response.IsSuccessStatusCode)
            {
                var errText = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "Lỗi khi tạo đối tác");
                return View(model);
            }

            var doiTacRes = await response.Content.ReadFromJsonAsync<DoiTac>();

            // Gửi từng nguyên liệu đã chọn
            foreach (var item in model.NguyenLieuGias.Where(nl => nl.DuocChon))
            {
                var payload = new
                {
                    IdDoiTac = doiTacRes.IdDoiTac,
                    IdNguyenLieu = item.IdNguyenLieu,
                    Gia = item.Gia
                };

                var res = await _httpClient.PostAsJsonAsync("api/DoiTacNguyenLieuAPI", payload);
                if (!res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Không thể tạo nguyên liệu {item.IdNguyenLieu} cho đối tác.");
                    // Có thể log hoặc xử lý lỗi tiếp tục/lưu log
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // 1. Lấy đối tác
            var doiTacRes = await _httpClient.GetAsync($"api/DoiTacAPI/{id}");
            if (!doiTacRes.IsSuccessStatusCode)
                return NotFound();
            var doiTac = await doiTacRes.Content.ReadFromJsonAsync<DoiTac>();

            // 2. Lấy toàn bộ nguyên liệu
            var nguyenLieuRes = await _httpClient.GetAsync("api/NguyenLieuAPI");
            var allNguyenLieus = await nguyenLieuRes.Content.ReadFromJsonAsync<List<NguyenLieu>>() ?? new();

            // 3. Lấy toàn bộ nguyên liệu đối tác đã chọn (danh sách)
            var nguyenLieuGiasDaChon = new List<DoiTacNguyenLieu>();
            var nguyenLieuGiaRes = await _httpClient.GetAsync($"api/DoiTacNguyenLieuAPI/{id}");
            if (nguyenLieuGiaRes.IsSuccessStatusCode)
            {
                nguyenLieuGiasDaChon = await nguyenLieuGiaRes.Content.ReadFromJsonAsync<List<DoiTacNguyenLieu>>() ?? new();
            }
            else
            {
                var content = await nguyenLieuGiaRes.Content.ReadAsStringAsync();
                Console.WriteLine("❌ API lỗi hoặc không trả về JSON:");
                Console.WriteLine(content);
            }

            // 4. Không lọc, hiển thị toàn bộ nguyên liệu
            var viewModel = new DoiTacNguyenLieuViewModel
            {
                DoiTac = doiTac,
                NguyenLieuGias = allNguyenLieus.Select(nl =>
                {
                    var matched = nguyenLieuGiasDaChon.FirstOrDefault(x => x.IdNguyenLieu == nl.IdNguyenLieu);
                    return new NguyenLieuGiaViewModel
                    {
                        IdNguyenLieu = nl.IdNguyenLieu,
                        TenNguyenLieu = nl.TenNguyenLieu,
                        Gia = matched?.Gia ?? 0,
                        DangCungCap = matched?.TrangThai == true
                    };
                }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(DoiTacNguyenLieuViewModel model)
        {
            Console.WriteLine("✅ Bắt đầu cập nhật đối tác...");
            if (!ModelState.IsValid || model.DoiTac == null)
            {
                // Lấy lại danh sách nguyên liệu nếu có lỗi
                var nguyenLieus = await _httpClient.GetFromJsonAsync<List<NguyenLieu>>("api/NguyenLieuAPI");
                model.NguyenLieuGias = nguyenLieus?
                    .Where(x => x.TrangThai)
                    .Select(x => new NguyenLieuGiaViewModel
                    {
                        IdNguyenLieu = x.IdNguyenLieu,
                        TenNguyenLieu = x.TenNguyenLieu,
                        DuocChon = false,
                        Gia = 0
                    }).ToList() ?? new();
                Console.WriteLine("❌ Dữ liệu không hợp lệ, trả về View với lỗi.");
                return View(model);
            }

            // Cập nhật đối tác
            var response = await _httpClient.PutAsJsonAsync($"api/DoiTacAPI/{model.DoiTac.IdDoiTac}", model.DoiTac);
            if (!response.IsSuccessStatusCode)
            {
                var errText = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "Lỗi khi cập nhật đối tác: " + errText);
                return View(model);
            }

            // Cập nhật nguyên liệu cung cấp
            foreach (var item in model.NguyenLieuGias)
            {
                Console.WriteLine($"Id Nguyên liệu: {item.IdNguyenLieu}, Nguyên liệu: {item.TenNguyenLieu}, DuocChon: {item.DangCungCap}, Giá: {item.Gia}");
                // Bỏ qua nếu chưa được chọn (tức là không muốn tạo mới)
                if (!item.DuocChon && item.Gia == 0)
                {
                    Console.WriteLine($"❌ Bỏ qua nguyên liệu {item.IdNguyenLieu} vì không được chọn.");
                    continue;
                }

                var dto = new DoiTacNguyenLieu
                {
                    IdDoiTac = model.DoiTac.IdDoiTac,
                    IdNguyenLieu = item.IdNguyenLieu,
                    Gia = item.Gia,
                    TrangThai = item.DangCungCap
                };

                // Gửi yêu cầu kiểm tra có tồn tại bản ghi đúng cả IdDoiTac và IdNguyenLieu không
                var checkRes = await _httpClient.GetAsync($"api/DoiTacNguyenLieuAPI/{ dto.IdNguyenLieu}/{dto.IdDoiTac}");
                Console.WriteLine($"Kiểm tra thông tin: {dto.IdNguyenLieu}/{dto.IdDoiTac} - Trạng thái: {dto.TrangThai}");
                if (checkRes.IsSuccessStatusCode)
                {
                    // ✅ Có rồi → cập nhật
                    var res = await _httpClient.PutAsJsonAsync($"api/DoiTacNguyenLieuAPI", dto);
                    Console.WriteLine($"Cập nhật nguyên liệu {item.IdNguyenLieu} cho đối tác {dto.IdDoiTac}.");
                    if (!res.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"❌ Không thể cập nhật nguyên liệu {item.IdNguyenLieu}.");
                    }
                }
                else
                {
                    // ✅ Chỉ tạo mới nếu được chọn
                    var res = await _httpClient.PostAsJsonAsync("api/DoiTacNguyenLieuAPI", dto);
                    if (!res.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"❌ Không thể tạo nguyên liệu {item.IdNguyenLieu}.");
                    }
                }
            }

            Console.WriteLine("✅ Cập nhật đối tác và nguyên liệu thành công!");
            return RedirectToAction("Index");
        }



    }
}
