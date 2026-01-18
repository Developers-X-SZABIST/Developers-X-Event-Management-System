using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Event_Management_System.Models;
using Event_Management_System.Models.Entities;

namespace Event_Management_System.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly DatabaseContext _context;

        public EventsController(DatabaseContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null) return NotFound();

            return View(@event);
        }


        [Authorize(Roles = Roles.Admin + "," + Roles.Organizer)]

        public IActionResult Create()
        {
            return View();
        }

       // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin + "," + Roles.Organizer)]

        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // EDIT GET
        [Authorize(Roles = Roles.Admin + "," + Roles.Organizer)]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            return View(@event);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin + "," + Roles.Organizer)]

        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // ADMIN ONLY   
        [Authorize(Roles = Roles.Admin + "," + Roles.Organizer)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null) return NotFound();

            return View(@event);
        }

        // ADMIN ONLY
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin + "," + Roles.Organizer)]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Events/RegisterConfirm/5
        [Authorize]
        public async Task<IActionResult> RegisterConfirm(int id)
        {

            var @event = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (@event == null) return NotFound();

            // Get UserId from custom claim
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null) return RedirectToAction("Login", "Auth");

            int userId = int.Parse(userIdClaim);

            // Check if already registered
            bool isRegistered = @event.Registrations.Any(r => r.UserId == userId);
            ViewBag.IsAlreadyRegistered = isRegistered;

            // Seat Logic
            int.TryParse(@event.MaxCapacity, out int max);
            int current = @event.Registrations.Count;
            ViewBag.SeatsLeft = max - current;
            ViewBag.IsFull = current >= max;

            return View(@event);
        }

        // POST: Events/ConfirmRegistration
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRegistration(int eventId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction("Login", "Auth");
            }

            int userId = int.Parse(userIdClaim);

            // Prevent double registration
            var exists = await _context.Registrations
                .AnyAsync(r => r.EventId == eventId && r.UserId == userId);

            if (!exists)
            {
                var registration = new Registration
                {
                    EventId = eventId,
                    UserId = userId,
                    RegistrationDate = DateTime.Now,
                    Status = "Confirmed" // Default status
                };
                _context.Add(registration);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
