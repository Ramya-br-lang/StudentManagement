using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // ================= LOAD DROPDOWNS =================
        private void LoadDropdowns()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "DepartmentName");
            // Do not load all courses by default; courses will be loaded based on selected department via AJAX
            ViewBag.Courses = new SelectList(Enumerable.Empty<SelectListItem>());
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {
            var students = _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course);

            return View(await students.ToListAsync());
        }

        // ================= DETAILS =================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
                return NotFound();

            return PartialView(student);
        }

        // ================= CREATE =================
        public async Task<IActionResult> Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            // Server-side validation: ensure course exists and belongs to selected department
            var course = await _context.Courses.FindAsync(student.CourseId);
            if (course == null || course.DepartmentId != student.DepartmentId)
            {
                ModelState.AddModelError("CourseId", "Selected course is invalid for the chosen department.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(student);
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            LoadDropdowns();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
                return NotFound();
            // Validate course belongs to department
            var course = await _context.Courses.FindAsync(student.CourseId);
            if (course == null || course.DepartmentId != student.DepartmentId)
            {
                ModelState.AddModelError("CourseId", "Selected course is invalid for the chosen department.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(student);
            }

            _context.Update(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

