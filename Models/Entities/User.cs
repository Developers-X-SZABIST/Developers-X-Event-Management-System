using System.ComponentModel.DataAnnotations;
namespace Event_Management_System.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }

        // 🔹 Hardcoded roles
        public string Role { get; set; }  = Roles.Public;

        // Navigation properties
        public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }

    // 🔹 Central place for allowed roles
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Organizer = "Organizer";
        public const string Public = "Public";
    }
}
