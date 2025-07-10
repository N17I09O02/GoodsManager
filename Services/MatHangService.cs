using System;
using Web_Manage.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Manage.Services
{
    public class MatHangService
    {
        private readonly ApplicationDbContext _context;

        public MatHangService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MatHang?> GetByIdAsync(int id)
        {
            return await _context.MatHangs.FindAsync(id);
        }

        public async Task<LichSuMatHang?> GetHistory(DateTime time)
        {
            return await _context.LichSuMatHangs.FindAsync(time);
        }

        public async Task<List<MatHang>> GetActiveAsync()
        {
            return await _context.MatHangs
                .Where(mh => mh.TrangThai == true)
                .ToListAsync();
        }

        public async Task AddHistoryAsync(LichSuMatHang lichSu)
        {
            _context.LichSuMatHangs.Add(lichSu);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MatHang>> GetAllAsync()
        {
            return await _context.MatHangs.ToListAsync();
        }

        public async Task AddAsync(MatHang hangHoa)
        {
            _context.MatHangs.Add(hangHoa);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MatHang hangHoa)
        {
            _context.MatHangs.Update(hangHoa);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.MatHangs.FindAsync(id);
            if (item == null)
            {
                Console.WriteLine($"❌ Không tìm thấy mặt hàng với Id = {id}");
                throw new Exception($"Không tìm thấy mặt hàng có Id = {id}");
            }

            _context.MatHangs.Remove(item);
            await _context.SaveChangesAsync();
        }


    }
}
