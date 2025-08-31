﻿using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories; 
using System.Threading.Tasks;

namespace UPTrain.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IUserRepository _userRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IQuizRepository _quizRepo;

        public HomeController(
            ICourseRepository courseRepo,
            IUserRepository userRepo,
            IEnrollmentRepository enrollmentRepo,
            IQuizRepository quizRepo)
        {
            _courseRepo = courseRepo;
            _userRepo = userRepo;
            _enrollmentRepo = enrollmentRepo;
            _quizRepo = quizRepo;
        }

        public async Task<IActionResult> Index()
        {
            var totalCourses = (await _courseRepo.GetAllAsync()).Count();
            var totalUsers = (await _userRepo.GetAllAsync()).Count();
            var totalEnrollments = (await _enrollmentRepo.GetAllAsync()).Count();
            var totalQuizzes = (await _quizRepo.GetAllAsync()).Count();

            ViewBag.TotalCourses = totalCourses;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalEnrollments = totalEnrollments;
            ViewBag.TotalQuizzes = totalQuizzes;

            return View();
        }
    }
}
