using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();

        CompanyRepository companyRepository = new CompanyRepository();

        public IActionResult Index()
        {
            var companies = companyRepository.Get();

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
                companyRepository.Create(company);
                companyRepository.Commit();

                TempData["notifation"] = "Add company successfuly";

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpGet]
        public IActionResult Edit(int companyId)
        {
            var company = companyRepository.GetOne(e => e.Id == companyId);
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
                companyRepository.Edit(company);
                companyRepository.Commit();

                TempData["notifation"] = "Update company successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
