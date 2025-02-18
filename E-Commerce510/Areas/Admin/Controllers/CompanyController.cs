using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var companies = dbContext.Companies.ToList();

            return View(companies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            //ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                dbContext.Companies.Add(company);
                dbContext.SaveChanges();

                TempData["notifation"] = "Add company successfuly";

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpGet]
        public IActionResult Edit(int companyId)
        {
            var company = dbContext.Companies.Find(companyId);
            if (company != null)
            {
                return View(company);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company)
        {
            if (company != null)
            {
                dbContext.Companies.Update(new Company()
                {
                    Id = company.Id,
                    Name = company.Name,
                    Description = company.Description,
                    Scale = company.Scale
                });
                dbContext.SaveChanges();

                TempData["notifation"] = "Update company successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
