using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;

namespace UPTrain.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IRepository<User> _userRepo;

        public UsersController(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAllAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepo.AddAsync(user);
                await _userRepo.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userRepo.GetOneAsync(u => u.UserId == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId) return NotFound();

            if (ModelState.IsValid)
            {
                await _userRepo.Update(user);
                await _userRepo.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userRepo.GetOneAsync(u => u.Id == id);

            if (user is not null)
            {
                await _userRepo.Delete(user);
                await _userRepo.CommitAsync();

                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var course = await _userRepo.GetOneAsync(c => c.UserId == id);

            if (course is not null)
            {
                await _userRepo.Delete(course);
                await _userRepo.CommitAsync();

                TempData["SuccessMessage"] = "Course deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
