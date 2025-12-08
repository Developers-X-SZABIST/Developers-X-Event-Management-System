using Event_Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace Event_Management_System.Models
{
    public class DatabaseContext : DbContext
    {
        //using the framework for database context
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        // User Management
        public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }

        // 2. Event Management
        public DbSet<Event> Events { get; set; }

        // Linking stuff
        public DbSet<Registration> Registrations { get; set; }
        //public DbSet<EventOrganizer> EventOrganizers { get; set; } //ON HOLD


    }
}
