using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QL_NhaHangCafe_Nhom07.Data;
using QL_NhaHangCafe_Nhom07.Models;

namespace QL_NhaHangCafe_Nhom07.Controllers
{
    public class DatBansController : Controller
    {
        private readonly QlNhaHangCafeContext _context;

        public DatBansController(QlNhaHangCafeContext context)
        {
            _context = context;
        }

        // =========================
        // DANH SÁCH ĐẶT BÀN
        // =========================
        public async Task<IActionResult> Index()
        {
            var ds = await _context.DatBans
                .Include(x => x.MaBanNavigation)
                .Include(x => x.MaKhNavigation)
                .ToListAsync();

            return View(ds);
        }

        // =========================
        // CHI TIẾT
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datBan = await _context.DatBans
                .Include(x => x.MaBanNavigation)
                .Include(x => x.MaKhNavigation)
                .FirstOrDefaultAsync(x => x.MaDatBan == id);

            if (datBan == null)
            {
                return NotFound();
            }

            return View(datBan);
        }

        // =========================
        // GET CREATE
        // =========================
        public IActionResult Create()
        {
            LoadData();
            return View();
        }

        // =========================
        // POST CREATE (ĐÃ SỬA LỖI ĐỊNH DẠNG TRẠNG THÁI BÀN)
        // =========================
        // =========================
        // POST CREATE (ĐÃ ĐỒNG BỘ CHUỖI TRẠNG THÁI VỚI SQL)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MaDatBan,MaKh,MaBan,SoLuongKhach,ThoiGianDat,GhiChu")]
    DatBan datBan)
        {
            // Bỏ qua validate các trường liên kết thực thể (Navigation)
            ModelState.Remove("MaBanNavigation");
            ModelState.Remove("MaKhNavigation");

            // Kiểm tra tính hợp lệ của thời gian đặt
            if (datBan.ThoiGianDat < DateTime.Now)
            {
                ModelState.AddModelError("", "Không được chọn thời gian quá khứ");
            }

            // Kiểm tra sức chứa bàn ăn
            var ban = await _context.Bans.FindAsync(datBan.MaBan);
            if (ban != null)
            {
                if (datBan.SoLuongKhach > ban.SoChoNgoi)
                {
                    ModelState.AddModelError("", "Bàn không đủ chỗ");
                }
            }

            // Nếu dữ liệu hợp lệ, tiến hành lưu trữ
            if (ModelState.IsValid)
            {
                datBan.NgayTao = DateTime.Now;

                /* 1. Trạng thái của đơn đặt bàn (Bảng DatBan): 
                   Theo CHECK constraint của bảng DatBan, giá trị phải thuộc: 'Chờ xác nhận', 'Đã xác nhận', 'Đã hủy', 'Hoàn thành'.
                   Mặc định khi tạo mới ta nên để là "Chờ xác nhận" hoặc "Đã xác nhận" tùy quy trình.
                */
                datBan.TrangThai = "Đã xác nhận";

                _context.DatBans.Add(datBan);

                /* 2. Trạng thái của vị trí bàn vật lý (Bảng Ban):
                   Sửa từ "DaDat" / "Đã đặt trước" thành đúng chữ "Đã đặt" để vượt qua constraint CK_Ban_TrangThai
                */
                if (ban != null)
                {
                    ban.TrangThai = "Đã đặt"; // CHÍNH XÁC THEO SQL: N'Đã đặt'
                }

                // Thực thi lưu xuống Database
                await _context.SaveChangesAsync();

                TempData["success"] = "Đặt bàn thành công";
                return RedirectToAction(nameof(Index));
            }

            LoadData();
            return View(datBan);
        }

        // =========================
        // GET EDIT
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datBan = await _context.DatBans.FindAsync(id);
            if (datBan == null)
            {
                return NotFound();
            }

            LoadData();
            return View(datBan);
        }

        // =========================
        // POST EDIT
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("MaDatBan,MaKh,MaBan,SoLuongKhach,ThoiGianDat,GhiChu,TrangThai,NgayTao")]
            DatBan datBan)
        {
            if (id != datBan.MaDatBan)
            {
                return NotFound();
            }

            ModelState.Remove("MaBanNavigation");
            ModelState.Remove("MaKhNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datBan);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Cập nhật thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatBanExists(datBan.MaDatBan))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadData();
            return View(datBan);
        }

        // =========================
        // GET DELETE
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datBan = await _context.DatBans
                .Include(x => x.MaBanNavigation)
                .Include(x => x.MaKhNavigation)
                .FirstOrDefaultAsync(x => x.MaDatBan == id);

            if (datBan == null)
            {
                return NotFound();
            }

            return View(datBan);
        }

        // =========================
        // POST DELETE
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datBan = await _context.DatBans.FindAsync(id);

            if (datBan != null)
            {
                // Trả bàn về trạng thái trống
                var ban = await _context.Bans.FindAsync(datBan.MaBan);
                if (ban != null)
                {
                    ban.TrangThai = "Trống";
                }

                _context.DatBans.Remove(datBan);
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Xóa đặt bàn thành công";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // CHECK EXISTS
        // =========================
        private bool DatBanExists(int id)
        {
            return _context.DatBans.Any(e => e.MaDatBan == id);
        }

        // =========================
        // LOAD DROPDOWN
        // =========================
        private void LoadData()
        {
            ViewData["MaBan"] = new SelectList(
                _context.Bans
                    .Where(x => x.TrangThai == "Trống")
                    .ToList(),
                "MaBan",
                "TenBan");

            ViewData["MaKh"] = new SelectList(
                _context.KhachHangs.ToList(),
                "MaKh",
                "HoTen");
        }
    }
}