using System.Diagnostics;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Filter out past deadlines and include registrations for seat counting
            var events = await _context.Events
                .Include(e => e.Registrations)
                .Where(e => e.RegistrationDeadline >= DateTime.Now)
                .OrderBy(e => e.RegistrationDeadline)
                .ToListAsync();

            return View(events);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
