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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }

        //what OnModelCreating is doing is that when model gets created it creates an index for Username column
        //for this step I believe migrations need to be added again
    }
}
