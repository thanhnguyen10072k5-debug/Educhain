using Educhain.Data;
using Educhain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Educhain.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách giảng viên
        public async Task<IActionResult> Index(string searchString)
        {
            var teachers = from t in _context.Teachers select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(t => t.FullName.Contains(searchString) 
                                            || t.TeacherCode.Contains(searchString)
                                            || t.Department.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await teachers.OrderByDescending(t => t.CreatedAt).ToListAsync());
        }

        // Form thêm giảng viên
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Teachers.AnyAsync(t => t.TeacherCode == teacher.TeacherCode))
                {
                    ModelState.AddModelError("TeacherCode", "Mã giảng viên này đã tồn tại.");
                    return View(teacher);
                }

                teacher.CreatedAt = DateTime.UtcNow;
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // Form sửa giảng viên
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher teacher)
        {
            if (id != teacher.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Teachers.AnyAsync(e => e.Id == teacher.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // Xóa giảng viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}