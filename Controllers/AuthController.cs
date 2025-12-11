using Event_Management_System.Models;
using Event_Management_System.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;



namespace Event_Management_System.Controllers
{
    public class AuthController : Controller
    {
        private DatabaseContext _db;
        public AuthController(DatabaseContext db)
        {
            _db = db;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.Any(u => u.Username == user.Username))
                {
                    var loggedInUser = await _db.Users.FirstOrDefaultAsync(u => u.Username.Equals(user.Username));

                    if (BCrypt.Net.BCrypt.Verify(user.PasswordHash, loggedInUser.PasswordHash))
                        return RedirectToAction("Index", "Home");
                    else
                        ViewBag.ErroMessage = "Incorrect Password!";
                }
                else
                {
                    ViewBag.ErroMessage = "Incorrect Username!";
                    return View(user);
                }
            }

            return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.Any(u => u.Username == user.Username))
                {
                    ViewBag.ErroMessage = "Username already exists! Please try another Username";
                    return View(user);
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return RedirectToAction("Login", "Auth");
            }

            return View(user);
        }
    }
}
