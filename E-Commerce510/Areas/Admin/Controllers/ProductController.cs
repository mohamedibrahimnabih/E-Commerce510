using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var products = dbContext.Products;

            return View(products.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            var category = dbContext.Categories;
            ViewBag.Category = category;

            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile file)
        {
            ModelState.Remove("Img");
            ModelState.Remove("file");
            if(ModelState.IsValid)
            {
                #region Save img into wwwroot
                if (file != null && file.Length > 0)
                {
                    // File Name, File Path
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    // Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save img into db
                    product.Img = fileName;
                }
                #endregion

                dbContext.Products.Add(product);
                dbContext.SaveChanges();
                TempData["notifation"] = "Add product successfuly";
                return RedirectToAction(nameof(Index));

            }

            var category = dbContext.Categories;
            ViewBag.Category = category;
            return View(product);
        }

        public IActionResult Edit(int productId)
        {
            var categories = dbContext.Categories;

            var products = dbContext.Products.FirstOrDefault(e => e.Id == productId);

            var productsWithCategories = new
            {
                categories,
                products
            };

            return View(productsWithCategories);
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile file)
        {
            #region Save img into wwwroot
            var productInDb = dbContext.Products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);
            
            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", productInDb.Img);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Copy Img to file
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Save img into db
                product.Img = fileName;
            }
            else
            {
                product.Img = productInDb.Img;
            }
            #endregion

            if (product != null)
            {
                dbContext.Products.Update(product);

                dbContext.SaveChanges();
                TempData["notifation"] = "Update product successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int productId)
        {
            var product = dbContext.Products.FirstOrDefault(e=>e.Id == productId);
            if (product != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", product.Img);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                dbContext.Remove(product);
                dbContext.SaveChanges();
                TempData["notifation"] = "Delete product successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
