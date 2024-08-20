using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using landingpage.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace landingpage.Controllers
{
    public class AuthController : Controller
    {
        private readonly ProductsdataContext _db;

        public AuthController(ProductsdataContext db)
        {
            _db = db;
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user)
        {
            var existingUser = _db.Users.FirstOrDefault(a => a.Email == user.Email);

            if (existingUser == null)
            {
                var hasher = new PasswordHasher<string>();
                string hashedPassword = hasher.HashPassword(user.Email, user.Password);

                user.Password = hashedPassword;
                _db.Users.Add(user);
                _db.SaveChanges();

                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Msg = "User already registered. Please login.";
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            bool isAuthenticated = false;

            ClaimsIdentity identity = null;
            string controller = "";

            var checkUser = _db.Users.FirstOrDefault(a => a.Email == user.Email);
            if (checkUser != null)
            {
                var hasher = new PasswordHasher<string>();
                var verifyPass = hasher.VerifyHashedPassword(user.Email, checkUser.Password, user.Password);
                if (verifyPass == PasswordVerificationResult.Success && checkUser.Roleid == 1)
                {
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,checkUser.Username ),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Sid, checkUser.Id.ToString()),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                    controller = "Admin";

                    HttpContext.Session.SetInt32("UserID", checkUser.Id);
                    HttpContext.Session.SetString("UserEmail", checkUser.Email);


                }
                else if (verifyPass == PasswordVerificationResult.Success && checkUser.Roleid == 2)
                {
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,checkUser.Username ),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Sid, checkUser.Id.ToString())
                }, CookieAuthenticationDefaults.AuthenticationScheme);


                    isAuthenticated = true;
                    controller = "Home";
                    HttpContext.Session.SetInt32("UserID", checkUser.Id);
                    HttpContext.Session.SetString("UserEmail", checkUser.Email); ;
                }
                else
                {
                    ViewBag.msg = "Invalid Credentials";
                    return View();

                }

                if (isAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", controller);

                }

                else
                {
                    ViewBag.msg = "Login Failed";
                    return View();
                }



            }
            else
            {
                ViewBag.msg = "Invalid User";
                return View();
            }



        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
