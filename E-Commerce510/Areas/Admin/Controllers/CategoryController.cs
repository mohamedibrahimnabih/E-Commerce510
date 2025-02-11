using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var categories = dbContext.Categories;
            //
            return View(categories.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string categoryName)
        {
            if (categoryName != null)
            {
                dbContext.Categories.Add(new Category()
                {
                    Name = categoryName
                });
                dbContext.SaveChanges();

                TempData["notifation"] = "Add product successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            var category = dbContext.Categories.Find(categoryId);
            if (category != null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category != null)
            {
                dbContext.Categories.Update(new Category()
                {
                    Id = category.Id,
                    Name = category.Name
                });
                dbContext.SaveChanges();

                TempData["notifation"] = "Update product successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public ActionResult Delete(int categoryId)
        {
            var category = dbContext.Categories.Find(categoryId);

            if (category != null)
            {
                dbContext.Categories.Remove(category);
                dbContext.SaveChanges();

                TempData["notifation"] = "Delete product successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
