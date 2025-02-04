using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var categories = dbContext.Categories;
            //
            return View(categories.ToList());
        }

        public IActionResult CreateNew()
        {
            return View();
        }

        public IActionResult SaveNew(string categoryName)
        {
            if(categoryName != null)
            {
                dbContext.Categories.Add(new Category()
                {
                    Name = categoryName
                });
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
