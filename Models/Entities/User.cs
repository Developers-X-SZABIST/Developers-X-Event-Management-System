
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management_System.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }




        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public string Email { get; set; }





        //// Foreign key to link users to their roles
        //[ForeignKey("Role")]
        //public int RoleId { get; set; }
        //public Role Role { get; set; } // Navigation property to Role entity


        //Navigation properties
        public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        //public ICollection<EventOrganizer> OrganizedEvents { get; set; } = new List<EventOrganizer>(); //tracking which events this user is assigned to as an organizer         //ON HOLD
    }
}

//Represents whoever interacts with the system (Admin, Organizer, Participants)