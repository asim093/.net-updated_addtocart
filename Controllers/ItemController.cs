using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using landingpage.Models;

namespace landingpage.Controllers
{
    public class ItemController : Controller
    {
        private readonly ProductsdataContext db;

        public ItemController(ProductsdataContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var data = db.Items.Include(item => item.Cat).ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string imagename = DateTime.Now.ToString("yymmddhhmmss") + "-" + Path.GetFileName(file.FileName);
                var imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");

                if (!Directory.Exists(imagepath))
                {
                    Directory.CreateDirectory(imagepath);
                }

                var imageValue = Path.Combine(imagepath, imagename);

                using (var stream = new FileStream(imageValue, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var dbimage = Path.Combine("/Uploads", imagename);
                item.Pimage = dbimage;
            }

            db.Items.Add(item); 
            await db.SaveChangesAsync();

            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName", item.CatId);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Item item, IFormFile file, string oldImage)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (file != null && file.Length > 0)
            {
                string imagename = DateTime.Now.ToString("yymmddhhmmss") + "-" + Path.GetFileName(file.FileName);
                var imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");

                if (!Directory.Exists(imagepath))
                {
                    Directory.CreateDirectory(imagepath);
                }

                var imageValue = Path.Combine(imagepath, imagename);

                using (var stream = new FileStream(imageValue, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var dbimage = Path.Combine("/Uploads", imagename);
                item.Pimage = dbimage;
            }
            else
            {
                item.Pimage = oldImage;
            }

            try
            {
                db.Items.Update(item);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName", item.CatId);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = db.Items.Find(id);
            if (item != null)
            {
                db.Items.Remove(item);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        private bool ItemExists(int id)
        {
            return db.Items.Any(e => e.Id == id);
        }
    }
}
