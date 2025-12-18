using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Event_Management_System.Models;
using Event_Management_System.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Event_Management_System.Controllers
{
    //Admin Only Access
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : Controller
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,PasswordHash,Email")] User user)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                }

                // Default role if none provided
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = Roles.Public;
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,Role")] User user)
        {
            if (id != user.UserId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch user from DB
                    var dbUser = await _context.Users.FindAsync(id);
                    if (dbUser == null) return NotFound();

                    // Update editable fields
                    dbUser.Username = user.Username;
                    dbUser.Email = user.Email;

                   

                    // Only Admin can update Role
                    if (User.IsInRole(Roles.Admin))
                    {
                        dbUser.Role = user.Role;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Any(e => e.UserId == user.UserId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


        //Here Action Level Authentication for Admin
        //Only Admin can set roles
        //[Authorize(Roles = Roles.Admin)]
        //public async Task<IActionResult> SetRole(int id, string role)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null) return NotFound();

        //    if (role != Roles.Public && role != Roles.Organizer && role != Roles.Admin)
        //        return BadRequest("Invalid role");

        //    user.Role = role;
        //    _context.Update(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //Implemented in edit view directly so redundant
    }
}
