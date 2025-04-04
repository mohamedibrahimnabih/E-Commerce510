using E_Commerce510.Models;
using E_Commerce510.Repositories;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_Commerce510.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public CartController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
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

        public IActionResult Pay()
        {
            var appUserId = _userManager.GetUserId(User);

            var carts = _cartRepository.Get(e => e.ApplicationUserId == appUserId, [e => e.Product]);

            Order order = new();
            order.ApplicationUserId = appUserId;
            order.OrderDate = DateTime.Now;
            order.OrderTotal = carts.Sum(e => e.Product.Price * e.Count);
            order.PaymentStatus = false;
            order.Status = OrderStatus.Pending;

            _orderRepository.Create(order);
            _orderRepository.Commit();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.OrderId}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
            };

            foreach (var item in carts)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                            Description = item.Product.Description,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            order.SessionId = session.Id;

            _orderRepository.Commit();

            return Redirect(session.Url);
        }
    }
}
