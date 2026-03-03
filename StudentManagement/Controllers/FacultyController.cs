using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class FacultyController : Controller
    {
        private readonly AppDbContext _context;

        public FacultyController(AppDbContext context)
        {
            _context = context;
        }

        private void LoadDropdowns()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "DepartmentName");
        }

        public IActionResult Index()
        {
            var faculties = _context.Faculties.Include(f => f.Department).ToList();
            return View(faculties);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = _context.Faculties.Include(f => f.Department).FirstOrDefault(f => f.Id == id);
            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Faculty faculty)
        {
            var dept = _context.Departments.Find(faculty.DepartmentId);
            if (dept == null)
            {
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(faculty);
            }

            _context.Faculties.Add(faculty);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = _context.Faculties.Find(id);
            if (faculty == null)
                return NotFound();

            LoadDropdowns();
            return View(faculty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Faculty faculty)
        {
            if (id != faculty.Id)
                return NotFound();
            var dept = _context.Departments.Find(faculty.DepartmentId);
            if (dept == null)
            {
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(faculty);
            }

            _context.Update(faculty);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var faculty = _context.Faculties.Include(f => f.Department).FirstOrDefault(f => f.Id == id);
            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var faculty = _context.Faculties.Find(id);
            if (faculty != null)
            {
                _context.Faculties.Remove(faculty);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Faculty/AssignCourses/5
        public IActionResult AssignCourses(int id)
        {
            var faculty = _context.Faculties.Include(f => f.Department).FirstOrDefault(f => f.Id == id);
            if (faculty == null)
                return NotFound();

            // only courses from the faculty's department
            var courses = _context.Courses.Where(c => c.DepartmentId == faculty.DepartmentId).ToList();

            var assignedCourseIds = _context.FacultyCourses.Where(fc => fc.FacultyId == id).Select(fc => fc.CourseId).ToList();

            ViewBag.Courses = new MultiSelectList(courses, "Id", "CourseName", assignedCourseIds);
            return View(faculty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignCourses(int id, int[] selectedCourseIds)
        {
            var faculty = _context.Faculties.Find(id);
            if (faculty == null)
                return NotFound();

            // ensure selected courses belong to faculty's department
            var validCourseIds = _context.Courses.Where(c => c.DepartmentId == faculty.DepartmentId).Select(c => c.Id).ToHashSet();

            selectedCourseIds = selectedCourseIds ?? new int[0];

            foreach (var courseId in selectedCourseIds)
            {
                if (!validCourseIds.Contains(courseId))
                    continue; // skip invalid (cross-department) assignment

                var exists = _context.FacultyCourses.Any(fc => fc.FacultyId == id && fc.CourseId == courseId);
                if (!exists)
                {
                    _context.FacultyCourses.Add(new FacultyCourse { FacultyId = id, CourseId = courseId });
                }
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
