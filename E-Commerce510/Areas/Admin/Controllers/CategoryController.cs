using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = categoryRepository.Get();
            //

            return View(categories.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            //ModelState.Remove("Products");

            if(ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.Commit();

                //TempData["notifation"] = "Add category successfuly";
                CookieOptions cookieOptions = new()
                {
                    Expires = DateTime.Now.AddSeconds(10)
                };

                Response.Cookies.Append("notifation", "Add category successfuly", cookieOptions);

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            var category = categoryRepository.GetOne(e => e.Id == categoryId);
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
                categoryRepository.Edit(category);
                categoryRepository.Commit();

                TempData["notifation"] = "Update category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public ActionResult Delete(int categoryId)
        {
            var category = categoryRepository.GetOne(e => e.Id == categoryId);

            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();

                TempData["notifation"] = "Delete category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
