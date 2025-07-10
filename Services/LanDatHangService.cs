using System;
using Web_Manage.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Manage.Services
{
    public class LanDatHangService
    {
        private readonly ApplicationDbContext _context;

        public LanDatHangService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LanDatHang>> GetAllAsync()
        {
            return await _context.LanDatHangs.ToListAsync();
        }

        public async Task<LanDatHang?> GetByIdAsync(int id)
        {
            return await _context.LanDatHangs.FindAsync(id);
        }

        public async Task<List<LanDatHang>> GetByIdDonHangAsync(int idDonHang)
        {
            return await _context.LanDatHangs
                .Where(l => l.IdDonHang == idDonHang)
                .ToListAsync();
        }

        public async Task AddAsync(LanDatHang lanDatHang)
        {
            _context.LanDatHangs.Add(lanDatHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LanDatHang lanDatHang)
        {
            _context.LanDatHangs.Update(lanDatHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTienAsync(int id, double tongTien)
        {
            var existing = await _context.LanDatHangs.FindAsync(id);
            if (existing == null)
                throw new Exception($"Không tìm thấy đơn hàng với Id = {id}");

            existing.TongTien = tongTien;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTrangThai(int id, bool trangThai)
        {
            var existing = await _context.LanDatHangs.FindAsync(id);
            if (existing == null)
                throw new Exception($"Không tìm thấy đơn hàng với Id = {id}");

            existing.ThanhToan = trangThai;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.LanDatHangs.FindAsync(id);
            if (item != null)
            {
                _context.LanDatHangs.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
