using System;
using Microsoft.EntityFrameworkCore;
using Web_Manage.Models;
namespace Web_Manage.Services
{
    public class DonHangService
    {
        private readonly ApplicationDbContext _context;

        public DonHangService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DonHang?> GetByIdAsync(int id)
        {
            return await _context.DonHangs.FindAsync(id);
        }

        public async Task<List<DonHang>> GetByIdKhachHangAsync(int id)
        {
            return await _context.DonHangs
                .Where(dh => dh.IdKhachHang == id)
                .ToListAsync();
        }

        public async Task<DonHang?> GetByDateAsync(DateOnly ngay)
        {
            return await _context.DonHangs.FirstOrDefaultAsync(dh => dh.NgayDat == ngay);
        }

        public async Task<DonHang?> GetByIdKHAnDateAsync(int id, DateOnly ngay)
        {
            return await _context.DonHangs.FirstOrDefaultAsync(dh => dh.IdKhachHang == id && dh.NgayDat == ngay);
        }

        public async Task<List<DonHang>> GetAllAsync(DateOnly dateOnly)
        {
            return await _context.DonHangs.Where(dh => dh.NgayDat == dateOnly).ToListAsync();
        }

        public async Task AddAsync(DonHang donHang)
        {
            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DonHang donHang)
        {
            _context.DonHangs.Update(donHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTienAsync(int id, double tongTien)
        {
            var existing = await _context.DonHangs.FindAsync(id);
            if (existing == null)
                throw new Exception($"Không tìm thấy đơn hàng với Id = {id}");

            existing.TongTien = tongTien;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateThanhToan(int id, bool daThanhToan)
        {
            var existing = await _context.DonHangs.FindAsync(id);
            if (existing == null)
                throw new Exception($"Không tìm thấy đơn hàng với Id = {id}");

            existing.ThanhToan = daThanhToan;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.DonHangs.FindAsync(id);
            if (item != null)
            {
                _context.DonHangs.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
