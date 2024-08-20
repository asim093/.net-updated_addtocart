using Microsoft.AspNetCore.Mvc;

namespace landingpage.Models
{
    public class CartViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
