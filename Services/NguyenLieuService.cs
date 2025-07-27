using Web_Manage.Models;
using Microsoft.EntityFrameworkCore;


namespace Web_Manage.Services
{
    public class NguyenLieuService
    {
        private readonly ApplicationDbContext _context;
        public NguyenLieuService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NguyenLieu?> GetByIdAsync(int id)
        {
            return await _context.nguyenLieus.FindAsync(id);
        }

        public async Task<List<NguyenLieu>> GetAllAsync()
        {
            return await _context.nguyenLieus.ToListAsync();
        }

        public async Task AddAsync(NguyenLieu nguyenLieu)
        {
            _context.nguyenLieus.Add(nguyenLieu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NguyenLieu nguyenLieu)
        {
            _context.nguyenLieus.Update(nguyenLieu);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var nguyenLieu = await _context.nguyenLieus.FindAsync(id);
            if (nguyenLieu == null)
                throw new Exception($"Không tìm thấy nguyên liệu với Id = {id}");

            _context.nguyenLieus.Remove(nguyenLieu);
            await _context.SaveChangesAsync();
        }
    }
}
