using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace E_Commerce510.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, ICartRepository cartRepository, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _cartRepository = cartRepository;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public IActionResult Success(int orderId)
        {
            using var transaction = _applicationDbContext.Database.BeginTransaction();
            var appUserId = _userManager.GetUserId(User);

            try
            {
                var order = _orderRepository.GetOne(e => e.OrderId == orderId);

                if (order != null && order.PaymentStatus == false)
                {
                    order.Status = Models.OrderStatus.InProgress;
                    order.PaymentStatus = true;

                    var service = new SessionService();
                    var session = service.Get(order.SessionId);

                    order.PaymentStripeId = session.PaymentIntentId;

                    _orderRepository.Commit();

                    var carts = _cartRepository.Get(e => e.ApplicationUserId == appUserId, [e => e.Product]);

                    List<OrderItem> orderItems = new();
                    foreach (var item in carts)
                    {
                        orderItems.Add(new()
                        {
                            Count = item.Count,
                            OrderId = orderId,
                            Price = item.Product.Price,
                            ProductId = item.ProductId
                        });
                    }

                    _orderItemRepository.CreateAll(orderItems);
                    _orderItemRepository.Commit();

                    _cartRepository.DeleteAll(carts.ToList());
                    _cartRepository.Commit();

                    transaction.Commit();

                    return View();
                }

                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
            }
            catch
            {
                transaction.Rollback();

                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
            }
        }
    }
}
