using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QL_NhaHangCafe_Nhom07.Data;
using QL_NhaHangCafe_Nhom07.Models;

namespace QL_NhaHangCafe_Nhom07.Controllers
{
    public class BansController : Controller
    {
        private readonly QlNhaHangCafeContext _context;

        public BansController(QlNhaHangCafeContext context)
        {
            _context = context;
        }

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var data = _context.Bans.Include(x => x.MaKhuVucNavigation);
            return View(await data.ToListAsync());
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewBag.MaKhuVuc = new SelectList(_context.KhuVucs, "MaKhuVuc", "TenKhuVuc");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ban ban)
        {
            // Fix lỗi navigation EF Core
            ModelState.Remove("MaKhuVucNavigation");

            if (string.IsNullOrEmpty(ban.TrangThai))
                ban.TrangThai = "Trống";

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Bans.Add(ban);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm bàn thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }

            ViewBag.MaKhuVuc = new SelectList(_context.KhuVucs, "MaKhuVuc", "TenKhuVuc", ban.MaKhuVuc);
            return View(ban);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ban = await _context.Bans.FindAsync(id);
            if (ban == null) return NotFound();

            ViewBag.MaKhuVuc = new SelectList(_context.KhuVucs, "MaKhuVuc", "TenKhuVuc", ban.MaKhuVuc);
            return View(ban);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ban ban)
        {
            if (id != ban.MaBan) return NotFound();

            ModelState.Remove("MaKhuVucNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ban);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bans.Any(e => e.MaBan == ban.MaBan))
                        return NotFound();

                    throw;
                }
            }

            ViewBag.MaKhuVuc = new SelectList(_context.KhuVucs, "MaKhuVuc", "TenKhuVuc", ban.MaKhuVuc);
            return View(ban);
        }
    }
}