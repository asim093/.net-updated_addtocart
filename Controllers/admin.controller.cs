using landingpage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace landingpage.Controllers
{
    public class Admin : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
           
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                return Content("Your data is good");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            if (email == "admin@gmail.com" && pass == "123")
            {
                HttpContext.Session.SetString("role", "admin");
                HttpContext.Session.SetString("email", email);
                return RedirectToAction("Index" );


            }
            else if (email == "user@gmail.com" && pass == "12345")
            {
                HttpContext.Session.SetString("role", "user");
                HttpContext.Session.SetString("email", email);
                return RedirectToAction("Index", "Home");
            }
           
            else
            {
                ViewBag.msg = "Invalid credentials";
                return View();
            }
        }


        public IActionResult AdminLogout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("email");
            return RedirectToAction("Login");

        }
    }
}
