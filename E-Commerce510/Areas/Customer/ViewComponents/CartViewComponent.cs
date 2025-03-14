using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace E_Commerce510.Areas.Customer.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;

        public CartViewComponent(UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var appUserId = _userManager.GetUserId(HttpContext.User);

            if(appUserId != null)
            {
                var totalNumber = _cartRepository.Get(e => e.ApplicationUserId == appUserId).Count();

                return View(totalNumber);
            }

            return View(0);
        }
    }
}
