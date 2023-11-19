using lr10.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace lr10.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterConsultation(Registration model)
        {
            if (!ModelState.IsValid) return View("Index", model);
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation ()
        {
            return View("Confirmation");
        }
    }
}