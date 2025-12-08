using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management_System.Models.Entities
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        //Status of the registration (e.g., Confirmed, Pending, Cancelled)

        [Required]
        public string Status { get; set; }

        // Foreign key to link registration to user who registered (Participant user)

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        // Foreign key to link registration to the event
        [ForeignKey("Event")]
        public int EventId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)] //to avoid multiple deletes
                                                  //1. User-> Registration-> Event
                                                  //2. User-> Event
        public Event Event { get; set; }


    }
}

//For tracking event registrations by users