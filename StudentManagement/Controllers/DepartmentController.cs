using Microsoft.AspNetCore.Mvc;

using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var departments = _context.Departments.ToList();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(department);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var department = _context.Departments.Find(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var department = _context.Departments.Find(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Department department)
        {
            if (id != department.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(department);

            _context.Update(department);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var department = _context.Departments.Find(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var department = _context.Departments.Find(id);
            if (department != null)
            {
                // Prevent deleting department with existing faculties or courses
                var hasFaculties = _context.Faculties.Any(f => f.DepartmentId == id);
                var hasCourses = _context.Courses.Any(c => c.DepartmentId == id);
                if (hasFaculties || hasCourses)
                {
                    // Add model error and return to Delete view with message
                    ModelState.AddModelError(string.Empty, "Cannot delete department because faculties or courses are assigned to it.");
                    return View(department);
                }

                _context.Departments.Remove(department);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}