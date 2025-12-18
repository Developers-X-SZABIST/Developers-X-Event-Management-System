using Event_Management_System.Models;
using Event_Management_System.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;




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

                    if (!string.IsNullOrEmpty(loggedInUser.PasswordHash) && BCrypt.Net.BCrypt.Verify(user.PasswordHash, loggedInUser.PasswordHash))
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

        [Authorize] //user needs to be logged in before logging out that is why authorize used here
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token"); //remove the token we stored in cookies
            return RedirectToAction("Index", "Home");
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

                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    ModelState.AddModelError("", "Password is required.");
                    return View(user);
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                user.Role = "Public"; //by default new users are public 
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                //Automatically log in
                var token = GenerateToken(user);

                Response.Cookies.Append("jwt_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });


                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "Public")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF32.GetBytes("class-work-5E")
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
