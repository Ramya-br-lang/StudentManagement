using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        private void LoadDropdowns()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "DepartmentName");
        }

        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // GET: /Course/GetByDepartment/5
        public IActionResult GetByDepartment(int id)
        {
            var courses = _context.Courses.Where(c => c.DepartmentId == id)
                .Select(c => new { c.Id, c.CourseName })
                .ToList();

            return Json(courses);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            // ensure department exists
            var dept = _context.Departments.Find(course.DepartmentId);
            if (dept == null)
            {
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(course);
            }

            _context.Courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();

            LoadDropdowns();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Course course)
        {
            if (id != course.Id)
                return NotFound();

            // ensure department exists
            var dept = _context.Departments.Find(course.DepartmentId);
            if (dept == null)
            {
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(course);
            }

            _context.Update(course);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
