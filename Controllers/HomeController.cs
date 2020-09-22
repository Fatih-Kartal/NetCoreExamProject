using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreExamProject.Models;

namespace NetCoreExamProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDBContext DB;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            this.DB = new MyDBContext();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = DB.Users.Where(w => w.Username == model.Username).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "There is no user belong to this username");
                    return View(model);
                }
                else
                {
                    if (user.Password == model.Password)
                    {
                        return RedirectToAction("Index", "Exams");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Wrong password");
                    }
                }
            }
            return View(model);
        }
    }
}
