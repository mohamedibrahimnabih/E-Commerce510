using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;

        public CartController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        public IActionResult AddToCart(int productId, int count)
        {
            var appUserId = _userManager.GetUserId(User);

            Cart cart = new Cart()
            {
                ApplicationUserId = appUserId,
                ProductId = productId,
                Count = count
            };

            var cartInDb = _cartRepository.GetOne(e => e.ApplicationUserId == appUserId && e.ProductId == productId);
            if (cartInDb != null)
                cartInDb.Count += count;
            else
                _cartRepository.Create(cart);

            _cartRepository.Commit();

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var appUserId = _userManager.GetUserId(User);

            var cart = _cartRepository.Get(e => e.ApplicationUserId == appUserId, [e => e.Product, e => e.ApplicationUser]);

            ViewBag.Total = cart.Sum(e => e.Product.Price * e.Count);

            return View(cart.ToList());
        }

        public IActionResult Increment(int productId)
        {
            var appUserId = _userManager.GetUserId(User);

            var cart = _cartRepository.GetOne(e => e.ApplicationUserId == appUserId && e.ProductId == productId);

            cart.Count++;
            _cartRepository.Commit();

            return RedirectToAction("Index");
        }

        public IActionResult Decrement(int productId)
        {
            var appUserId = _userManager.GetUserId(User);

            var cart = _cartRepository.GetOne(e => e.ApplicationUserId == appUserId && e.ProductId == productId);

            if (cart.Count > 1)
            {
                cart.Count--;
                _cartRepository.Commit();
            }

            return RedirectToAction("Index");
        }
    }
}
