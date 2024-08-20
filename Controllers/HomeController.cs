using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using landingpage.Models;

namespace landingpage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductsdataContext _db;

        public HomeController(ProductsdataContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Products()
        {
            var data = _db.Items.Include(item => item.Cat);
            return View(data.ToList());
        }

        [Authorize(Roles = "User")]
        public IActionResult Details(int id)
        {
            var data = _db.Items
                           .Include(item => item.Cat);
            var item = data.FirstOrDefault(prd => prd.Id == id);


            if (item != null)
            {
                Cart cart = new Cart();
                ViewBag.Cart = cart;

                return View(item);
            }
            else
            {
                return RedirectToAction("Products");
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Cart cart)
        {
            cart.Total = cart.Price * cart.Qty;
            _db.Carts.Add(cart);
            _db.SaveChanges();
            return RedirectToAction("Products");
        }

    }   
    }

