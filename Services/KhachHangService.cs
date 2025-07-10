using System;
using Web_Manage.Models;
using Microsoft.EntityFrameworkCore;


namespace Web_Manage.Services
{
    public class KhachHangService
    {
        private readonly ApplicationDbContext _context;

        public KhachHangService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<KhachHang?> GetByIdAsync(int id)
        {
            return await _context.KhachHangs.FindAsync(id);
        }

        public async Task<List<KhachHang>> GetAllAsync()
        {
            return await _context.KhachHangs.ToListAsync();
        }

        public async Task AddAsync(KhachHang khachHang)
        {
            _context.KhachHangs.Add(khachHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhachHang khachHang)
        {
            _context.KhachHangs.Update(khachHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTienAsync(int id, double tongTien)
        {
            var existing = await _context.KhachHangs.FindAsync(id);
            if (existing == null)
                throw new Exception($"Không tìm thấy khách hàng với Id = {id}");

            existing.TienNo = tongTien;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTruTienAsync(int id, double truTien)
        {
            var existing = await _context.KhachHangs.FindAsync(id);
            if (existing == null)
                throw new Exception($"Không tìm thấy khách hàng với Id = {id}");

            existing.TienNo -= truTien;
            if (existing.TienNo < 0) existing.TienNo = 0; // Đảm bảo tiền nợ không âm
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.KhachHangs.FindAsync(id);
            if (item != null)
            {
                _context.KhachHangs.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
