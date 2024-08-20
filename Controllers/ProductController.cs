using landingpage.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace landingpage.Controllers
{
    public class ProductController : Controller
    {

        private readonly ProductsdataContext _db;

        public ProductController( ProductsdataContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.Products.ToList();
            return View(products);
        }

        public IActionResult create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create(Product prd)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(prd);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product prd)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(prd);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product prd)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Remove(prd);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                return View();
            }
        }





    }
}
