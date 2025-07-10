using System;
using Web_Manage.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Manage.Services
{
    public class ChiTietDatHangService
    {
        private readonly ApplicationDbContext _context;

        public ChiTietDatHangService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChiTietDatHang?> GetByIdAsync(int id)
        {
            return await _context.ChiTietDatHangs.FindAsync(id);
        }

        public async Task<List<ChiTietDatHang>> GetAllAsync()
        {
            return await _context.ChiTietDatHangs.ToListAsync();
        }

        public async Task<List<ChiTietDatHang>> GetByIdLanDatHangAsync(int idLanDatHang)
        {
            return await _context.ChiTietDatHangs
                .Where(c => c.IdLanDatHang == idLanDatHang)
                .ToListAsync();
        }

        public async Task AddAsync(ChiTietDatHang chiTiet)
        {
            _context.ChiTietDatHangs.Add(chiTiet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietDatHang chiTiet)
        {
            var existing = await _context.ChiTietDatHangs.FindAsync(chiTiet.IdChiTietDonHang);
            if (existing == null)
                throw new Exception($"Không tìm thấy chi tiết đặt hàng với Id = {chiTiet.IdChiTietDonHang}");

            // Cập nhật chỉ trạng thái
            existing.TrangThai = chiTiet.TrangThai;
            existing.TongTien = chiTiet.TongTien;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.ChiTietDatHangs.FindAsync(id);
            if (item != null)
            {
                _context.ChiTietDatHangs.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
