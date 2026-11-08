using Educhain.Data;
using Educhain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Educhain.Controllers
{
    [Authorize(Roles = "Admin,Teacher")] // Chỉ Admin và Giảng viên mới có quyền vào đây
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. DANH SÁCH & TÌM KIẾM SINH VIÊN
        public async Task<IActionResult> Index(string searchString)
        {
            var students = from s in _context.Students select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.FullName.Contains(searchString) 
                                            || s.StudentCode.Contains(searchString)
                                            || s.ClassName.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await students.OrderByDescending(s => s.CreatedAt).ToListAsync());
        }

        // 2. FORM THÊM SINH VIÊN (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 3. XỬ LÝ LƯU SINH VIÊN MỚI (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng mã sinh viên
                if (await _context.Students.AnyAsync(s => s.StudentCode == student.StudentCode))
                {
                    ModelState.AddModelError("StudentCode", "Mã sinh viên này đã tồn tại trong hệ thống.");
                    return View(student);
                }

                student.CreatedAt = DateTime.UtcNow;
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // 4. FORM SỬA THÔNG TIN SINH VIÊN (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // 5. XỬ LÝ CẬP NHẬT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Students.AnyAsync(e => e.Id == student.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // 6. XÓA SINH VIÊN (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}