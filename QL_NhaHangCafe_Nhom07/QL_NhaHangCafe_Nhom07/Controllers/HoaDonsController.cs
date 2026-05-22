using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QL_NhaHangCafe_Nhom07.Data;
using QL_NhaHangCafe_Nhom07.Models;

namespace QL_NhaHangCafe_Nhom07.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly QlNhaHangCafeContext _context;

        public HoaDonController(QlNhaHangCafeContext context)
        {
            _context = context;
        }

        // ========================================
        // DANH SÁCH HÓA ĐƠN
        // ========================================
        public async Task<IActionResult> Index()
        {
            var ds = await _context.HoaDons
                .Include(x => x.MaBanNavigation)
                .Include(x => x.MaKhNavigation)
                .Include(x => x.MaNvNavigation)
                .OrderByDescending(x => x.MaHd)
                .ToListAsync();

            return View(ds);
        }

        // ========================================
        // TẠO HÓA ĐƠN - GET
        // ========================================
        public IActionResult Create()
        {
            LoadData();
            return View();
        }

        // ========================================
        // TẠO HÓA ĐƠN - POST (ĐÃ SỬA LỖI MẤT CHI TIẾT)
        // ========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            HoaDon hoaDon,
            int[] monAnIds,
            int[] soLuongs)
        {
            // Bỏ qua validate các thuộc tính liên kết tự động của EF Core
            ModelState.Remove("MaBanNavigation");
            ModelState.Remove("MaKhNavigation");
            ModelState.Remove("MaNvNavigation");
            ModelState.Remove("ChiTietHoaDons");

            try
            {
                if (ModelState.IsValid)
                {
                    hoaDon.NgayLap = DateTime.Now;
                    decimal tongTien = 0;

                    // Khởi tạo danh sách chi tiết hóa đơn gắn liền với thực thể hoaDon
                    hoaDon.ChiTietHoaDons = new List<ChiTietHoaDon>();

                    // Kiểm tra dữ liệu mảng món ăn từ giao diện gửi lên
                    if (monAnIds != null && soLuongs != null)
                    {
                        for (int i = 0; i < monAnIds.Length; i++)
                        {
                            // Chỉ xử lý các món ăn có số lượng lớn hơn 0
                            if (soLuongs[i] > 0)
                            {
                                var mon = await _context.MonAns
                                    .FirstOrDefaultAsync(x => x.MaMon == monAnIds[i]);

                                if (mon != null)
                                {
                                    decimal gia = mon.Gia;
                                    int sl = soLuongs[i];

                                    ChiTietHoaDon ct = new ChiTietHoaDon()
                                    {
                                        // EF Core sẽ tự gán MaHd sau khi hóa đơn chính được chèn
                                        MaMon = mon.MaMon,
                                        SoLuong = sl,
                                        DonGia = gia
                                    };

                                    tongTien += gia * sl;

                                    // Thêm trực tiếp vào danh sách chi tiết của hoaDon
                                    hoaDon.ChiTietHoaDons.Add(ct);
                                }
                            }
                        }
                    }

                    // Gán tổng tiền đã tính toán hoàn chỉnh vào hóa đơn
                    hoaDon.TongTien = tongTien;

                    // Lưu một lần duy nhất, EF tự xử lý lưu Hóa đơn & toàn bộ Chi tiết liên quan
                    _context.HoaDons.Add(hoaDon);
                    await _context.SaveChangesAsync();

                    // Thông báo thành công
                    TempData["success"] = "Lập hóa đơn thành công!";

                    // Điều hướng về trang danh sách
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                // Hiển thị chi tiết lỗi nếu phát sinh trong quá trình chạy
                TempData["error"] = "Lỗi hệ thống: " + ex.Message;
            }

            // Nếu dữ liệu không hợp lệ, tải lại dữ liệu cho các Dropdown và Form
            LoadData();
            return View(hoaDon);
        }

        // ========================================
        // CHI TIẾT HÓA ĐƠN
        // ========================================
        public async Task<IActionResult> Details(int id)
        {
            var hoaDon = await _context.HoaDons
                .Include(x => x.MaBanNavigation)
                .Include(x => x.MaKhNavigation)
                .Include(x => x.MaNvNavigation)
                .Include(x => x.ChiTietHoaDons)
                    .ThenInclude(x => x.MaMonNavigation)
                .FirstOrDefaultAsync(x => x.MaHd == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // ========================================
        // XÓA HÓA ĐƠN
        // ========================================
        public async Task<IActionResult> Delete(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            // Xóa toàn bộ các bản ghi trong bảng chi tiết trước để tránh lỗi khóa ngoại
            var chiTiet = await _context.ChiTietHoaDons
                .Where(x => x.MaHd == id)
                .ToListAsync();

            _context.ChiTietHoaDons.RemoveRange(chiTiet);

            // Xóa hóa đơn chính
            _context.HoaDons.Remove(hoaDon);

            await _context.SaveChangesAsync();

            TempData["success"] = "Xóa hóa đơn thành công!";

            return RedirectToAction(nameof(Index));
        }

        // ========================================
        // TẢI DỮ LIỆU DROPDOWN DANH SÁCH
        // ========================================
        private void LoadData()
        {
            ViewBag.MaBan = new SelectList(
                _context.Bans,
                "MaBan",
                "TenBan");

            ViewBag.MaKh = new SelectList(
                _context.KhachHangs,
                "MaKh",
                "HoTen");

            ViewBag.MaNv = new SelectList(
                _context.NhanViens,
                "MaNv",
                "HoTen");

            ViewBag.MonAns = _context.MonAns.ToList();
        }
    }
}