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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    PasswordHash = registerVM.Password,
                    Address = registerVM.Address
                };

                var result = await userManager.CreateAsync(applicationUser);

                if(!result.Succeeded)
                {
                    return View(registerVM);
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
            }

            return View(registerVM);
        }
    }
}
