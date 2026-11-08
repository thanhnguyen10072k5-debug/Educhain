using Educhain.Data;
using Educhain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Educhain.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class GradeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. DANH SÁCH BẢNG ĐIỂM
        public async Task<IActionResult> Index(string searchString)
        {
            var gradesQuery = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                gradesQuery = gradesQuery.Where(g => 
                    (g.Student != null && (g.Student.FullName.Contains(searchString) || g.Student.StudentCode.Contains(searchString))) ||
                    (g.Subject != null && (g.Subject.SubjectName.Contains(searchString) || g.Subject.SubjectCode.Contains(searchString))));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await gradesQuery.OrderByDescending(g => g.UpdatedAt).ToListAsync());
        }

        // 2. FORM NHẬP ĐIỂM MỚI (GET)
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _context.Students.OrderBy(s => s.FullName).ToListAsync(), "Id", "FullName");
            ViewBag.Subjects = new SelectList(await _context.Subjects.OrderBy(s => s.SubjectName).ToListAsync(), "Id", "SubjectName");
            return View();
        }

        // 3. XỬ LÝ LƯU ĐIỂM (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade grade)
        {
            if (ModelState.IsValid)
            {
                grade.UpdatedAt = DateTime.UtcNow;
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _context.Students.OrderBy(s => s.FullName).ToListAsync(), "Id", "FullName", grade.StudentId);
            ViewBag.Subjects = new SelectList(await _context.Subjects.OrderBy(s => s.SubjectName).ToListAsync(), "Id", "SubjectName", grade.SubjectId);
            return View(grade);
        }

        // 4. XÓA BẢNG ĐIỂM (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}