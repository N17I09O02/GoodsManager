using Web_Manage.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Manage.Services
{
    public class DoanhNghiepService
    {
        private readonly ApplicationDbContext _context;
        public DoanhNghiepService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<DoanhNghiep?> GetByIdAsync(int id)
        {
            return await _context.DoanhNghieps.Where(dn => dn.IdKhachHang == id).FirstOrDefaultAsync();
        }
        public async Task AddDoanhNghiepAsync(DoanhNghiep doanhNghiep)
        {
            _context.DoanhNghieps.Add(doanhNghiep);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoanhNghiepAsync(DoanhNghiep doanhNghiep)
        {
            _context.DoanhNghieps.Update(doanhNghiep);
            await _context.SaveChangesAsync();
        }
    }
}
