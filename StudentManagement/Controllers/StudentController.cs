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
            private void LoadDropdowns()
        {
            ViewBag.Departments = new List<string>
    {
        "CSE",
        "ECE",
        "IT",
        "Mechanical",
        "Civil"
    };

            ViewBag.Courses = new List<string>
    {
        "Full Stack",
        "Data Science",
        "AI & ML",
        "Cloud Computing",
        "Cyber Security"
    };
        }
        

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {

            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        // ================= DETAILS =================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            ViewBag.Departments = new List<string>
    {
        "CSE",
        "ECE",
        "IT",
        "Mechanical",
        "Civil"
    };

            ViewBag.Courses = new List<string>
    {
        "Full Stack",
        "Data Science",
        "AI & ML",
        "Cloud Computing",
        "Cyber Security"
    };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                
                ViewBag.Departments = new List<string>
        {
            "CSE",
            "ECE",
            "IT",
            "Mechanical",
            "Civil"
        };

                ViewBag.Courses = new List<string>
        {
            "Full Stack",
            "Data Science",
            "AI & ML",
            "Cloud Computing",
            "Cyber Security"
        };

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

            ViewBag.Departments = new List<string>
    {
        "CSE",
        "ECE",
        "IT",
        "Mechanical",
        "Civil"
    };

            ViewBag.Courses = new List<string>
    {
        "Full Stack",
        "Data Science",
        "AI & ML",
        "Cloud Computing",
        "Cyber Security"
    };

            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                // Reload dropdown lists
                ViewBag.Departments = new List<string>
        {
            "CSE",
            "ECE",
            "IT",
            "Mechanical",
            "Civil"
        };

                ViewBag.Courses = new List<string>
        {
            "Full Stack",
            "Data Science",
            "AI & ML",
            "Cloud Computing",
            "Cyber Security"
        };

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

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}