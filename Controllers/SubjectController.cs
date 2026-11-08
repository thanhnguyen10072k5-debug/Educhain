using Educhain.Data;
using Educhain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Educhain.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách môn học
        public async Task<IActionResult> Index(string searchString)
        {
            var subjects = from s in _context.Subjects select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                subjects = subjects.Where(s => s.SubjectName.Contains(searchString) 
                                            || s.SubjectCode.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await subjects.OrderBy(s => s.SubjectCode).ToListAsync());
        }

        // Form thêm môn học
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Subjects.AnyAsync(s => s.SubjectCode == subject.SubjectCode))
                {
                    ModelState.AddModelError("SubjectCode", "Mã môn học đã tồn tại.");
                    return View(subject);
                }

                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // Form sửa môn học
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();

            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject subject)
        {
            if (id != subject.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Subjects.AnyAsync(e => e.Id == subject.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // Xóa môn học
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}