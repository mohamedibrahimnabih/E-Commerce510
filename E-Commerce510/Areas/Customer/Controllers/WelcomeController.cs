﻿using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Controllers
{
    [Area("Customer")]
    public class WelcomeController : Controller
    {
        public IActionResult Home()
        {
            string name = "Mohamed";
            double salary = 1000;

            return View(salary);
        }
    }
}
