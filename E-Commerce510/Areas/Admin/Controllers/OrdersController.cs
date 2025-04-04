using E_Commerce510.Models;
using E_Commerce510.Models.ViewModel;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrdersController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public IActionResult Index()
        {
            var orders = _orderRepository.Get(includes: [e=>e.ApplicationUser]);

            return View(orders);
        }

        public IActionResult Details(int orderId)
        {
            var order = _orderRepository.GetOne(filter: e=>e.OrderId == orderId, includes: [e => e.ApplicationUser]);

            var orderitems = new List<OrderItem>();

            if (order is not null)
            {
                 orderitems = _orderItemRepository.Get(e => e.OrderId == order.OrderId, includes: [e => e.Product]).ToList();
            }

            OrderWithItems orderWithItems = new()
            {
                Order = order,
                OrderItems = orderitems
            };

            return View(orderWithItems);
        }

        public IActionResult Refund(int orderId)
        {
            var order = _orderRepository.GetOne(filter: e => e.OrderId == orderId);

            if(order is not null)
            {
                RefundCreateOptions options = new()
                {
                    Amount = (long)order.OrderTotal,
                    PaymentIntent = order.PaymentStripeId,
                    Reason = RefundReasons.Fraudulent
                };

                var service = new RefundService();
                var session = service.Create(options);

                order.Status = OrderStatus.Canceled;
                _orderRepository.Commit();

                return RedirectToAction("Details");
            }

            return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
        }
    }
}
