using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
