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

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .ToListAsync();
            return View(students);
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

            return View(student);
        }

        // ================= CREATE =================
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            // Server-side validation for department-course relationship
            if (student.DepartmentId > 0 && student.CourseId > 0)
            {
                var course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.Id == student.CourseId && c.DepartmentId == student.DepartmentId);
                
                if (course == null)
                {
                    ModelState.AddModelError("CourseId", "Selected course does not belong to the selected department.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(student.DepartmentId);
                return View(student);
            }

            _context.Add(student);
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

            await LoadDropdowns(student.DepartmentId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
                return NotFound();

            // Server-side validation for department-course relationship
            if (student.DepartmentId > 0 && student.CourseId > 0)
            {
                var course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.Id == student.CourseId && c.DepartmentId == student.DepartmentId);
                
                if (course == null)
                {
                    ModelState.AddModelError("CourseId", "Selected course does not belong to the selected department.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(student.DepartmentId);
                return View(student);
            }

            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == student.Id))
                    return NotFound();
                else
                    throw;
            }

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

        // ================= AJAX METHODS =================
        [HttpGet]
        public async Task<JsonResult> GetCoursesByDepartment(int departmentId)
        {
            var courses = await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .Select(c => new { value = c.Id, text = c.CourseName })
                .ToListAsync();

            return Json(courses);
        }

        // ================= HELPER METHODS =================
        private async Task LoadDropdowns(int? selectedDepartmentId = null)
        {
            ViewBag.DepartmentId = new SelectList(
                await _context.Departments.ToListAsync(), 
                "Id", 
                "DepartmentName", 
                selectedDepartmentId);

            if (selectedDepartmentId.HasValue)
            {
                ViewBag.CourseId = new SelectList(
                    await _context.Courses.Where(c => c.DepartmentId == selectedDepartmentId).ToListAsync(),
                    "Id", 
                    "CourseName");
            }
            else
            {
                ViewBag.CourseId = new SelectList(new List<Course>(), "Id", "CourseName");
            }
        }
    }
}
