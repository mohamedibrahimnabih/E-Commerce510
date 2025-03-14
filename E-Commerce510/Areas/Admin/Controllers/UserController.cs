using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index(string? query, int page = 1)
        {
            var users = _userRepository.Get();

            if (query != null)
                users = users.Where(e => e.UserName.Contains(query) || e.Email.Contains(query)); // 13

            users = users.Skip((page-1) * 2).Take(2);

            var pageNumbers = Math.Ceiling(users.Count() / (double)2);

            if (pageNumbers + 1 < page)
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });

            ViewBag.PageNumbers = pageNumbers;

            return View(users.ToList());
        }
    }
}
