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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        //Status of the registration (e.g., Confirmed, Pending, Cancelled)

        [Required]
        public string Status { get; set; }

        // Foreign key to link registration to user who registered (Participant user)

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        




        // Foreign key to link registration to the event
        [ForeignKey("Event")]
        public int EventId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)] 
        public Event? Event { get; set; }
        
    }
}

//For tracking event registrations by users