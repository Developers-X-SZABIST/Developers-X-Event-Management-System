using Event_Management_System.Models;
using Event_Management_System.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Event_Management_System.Controllers
{
    public class AuthController : Controller
    {
        private readonly DatabaseContext _db;
        public AuthController(DatabaseContext db)
        {
            _db = db;
        }

        // GET: Login
        public IActionResult Login()
        {
            if (!User.IsInRole(Roles.Admin) && !User.IsInRole(Roles.Organizer) && !User.IsInRole(Roles.Public))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!User.IsInRole(Roles.Admin) && !User.IsInRole(Roles.Organizer) && !User.IsInRole(Roles.Public))
            {
                if (ModelState.IsValid)
                {
                    var loggedInUser = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                    if (loggedInUser != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(user.PasswordHash, loggedInUser.PasswordHash))
                        {
                            var token = GenerateToken(loggedInUser);
                            Response.Cookies.Append("jwt_token", token, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict
                            });

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.ErroMessage = "Incorrect Password!";
                        }
                    }
                    else
                    {
                        ViewBag.ErroMessage = "Incorrect Username!";
                    }
                }

                return View(user);
                }
            return RedirectToAction("Index", "Home");
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
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
                if (_db.Users.Any(u => u.Email == user.Email))
                {
                    ViewBag.ErroMessage = "Email already exists! Please try another Email.";
                    return View(user);
                }
                // Hash the password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                // Ensure role is always Public for normal registration
                user.Role = Roles.Public;

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                return RedirectToAction("Login", "Auth");
            }

            return View(user);
        }

        // JWT generation
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? Roles.Public)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes("class-work-5E"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Login", "Auth");
        }

    }
}
