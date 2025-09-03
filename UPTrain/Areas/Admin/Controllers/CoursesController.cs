using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoursesController : Controller
    {
        private readonly IRepository<Courses> _courseRepo;
        private readonly UserManager<User> _userManager;

        public CoursesController(IRepository<Courses> courseRepo, UserManager<User> userManager)
        {
            _courseRepo = courseRepo;
            _userManager = userManager;
        }

  
        public async Task<IActionResult> Index()
        {
            var courses = await _courseRepo.GetAllAsync(includes: c => c.CreatedBy);
            return View(courses);
        }


        public async Task<IActionResult> Create()
        {
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Courses course ,IFormFile ImageUrl)
        {
          
            {
                
                var user = await _userManager.GetUserAsync(User);

               
                course.CreatedById = user.Id;
                course.CreatedDate = DateTime.Now;

               
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/courses", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }

                    course.ImageUrl = "/images/courses/" + fileName;
                }

                await _courseRepo.AddAsync(course);
                await _courseRepo.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

        }


        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseRepo.GetOneAsync(c => c.CourseId == id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Courses course)
        {
            if (id != course.CourseId) return NotFound();

            if (ModelState.IsValid)
            {
                course.UpdatedDate = DateTime.Now;

                await _courseRepo.Update(course);
                await _courseRepo.CommitAsync();

                TempData["SuccessMessage"] = "Course updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseRepo.GetOneAsync(c => c.CourseId == id);

            if (course != null)
            {
                await _courseRepo.Delete(course);
                await _courseRepo.CommitAsync();

                TempData["SuccessMessage"] = "Course deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
