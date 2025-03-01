﻿using E_Commerce510.Models;
using E_Commerce510.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace E_Commerce510.Areas.Identity.Contollers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                    Address = registerVM.Address
                };

                var result = await userManager.CreateAsync(applicationUser, registerVM.Password);

                if(!result.Succeeded)
                {
                    return View(registerVM);
                }
                else
                {
                    await signInManager.SignInAsync(applicationUser, false);
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
            }

            return View(registerVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
                var appUser = await userManager.FindByEmailAsync(loginVM.Email);

                if(appUser != null)
                {
                    bool result = await userManager.CheckPasswordAsync(appUser, loginVM.Password);

                    if(result)
                    {
                        // Login
                        await signInManager.SignInAsync(appUser, loginVM.RemmeberMe);

                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password does not match");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "User Name does not found");
                }
            }

            return View(loginVM);
        }

        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}
