using System.Diagnostics;
using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce510.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string productName, double minPrice, double maxPrice, int categoryId, bool isHot)
        {
            IQueryable<Product> products = dbContext.Products.Include(e => e.Category);

            if(productName != null)
            {
                products = products.Where(e=>e.Name.Contains(productName));
            }

            if(minPrice>0)
            {
                products = products.Where(e => e.Price > minPrice);
            }

            if (maxPrice > 0)
            {
                products = products.Where(e => e.Price < maxPrice);
            }

            if (categoryId > 0)
            {
                products = products.Where(e => e.CategoryId == categoryId);
            }

            //if(isHot)
            //{
            //    products = products.Where(e => e.Discount > 12);
            //}

            ViewBag.categories = dbContext.Categories.ToList();

            ViewBag.ProductName = productName;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(products.ToList());
        }

        public IActionResult Details(int productId)
        {
            var product = dbContext.Products.Include(e => e.Category).FirstOrDefault(e => e.Id == productId);

            if (product != null)
            {
                var productsWithSameCategory = dbContext.Products.Where(e => e.CategoryId == product.CategoryId).Skip(1).Take(4).ToList();

                //var productsWithSameCategoryName = new ProductsWithSameCategoryName()
                //{
                //    Product = product,
                //    ProductsWithSameCategory = productsWithSameCategory
                //};

                var productsWithSameCategoryName = new
                {
                    Product = product,
                    ProductsWithSameCategory = productsWithSameCategory,
                };

                //ViewBag.productsWithSameCategoryName = productsWithSameCategory;
                //ViewData["productsWithSameCategoryName"] = productsWithSameCategory;

                return View(productsWithSameCategoryName);
            }

            return RedirectToAction("NotFoundPage");
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
