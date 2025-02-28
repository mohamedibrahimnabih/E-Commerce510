using E_Commerce510.Models;
using E_Commerce510.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Identity.Contollers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        
    }
}
