using System.ComponentModel.DataAnnotations;

namespace Event_Management_System.Models.Entities
{
    public class Role
    {
        //[Key]
        //public int RoleId { get; set; }

        //[Required]
        //public string RoleName { get; set; } // e.g., Admin, Organizer, Participant

        //// Navigation property, all users with this role

        //public ICollection<User> Users { get; set; } = new List<User>();


    }
}


// Role entity for user roles and permissions