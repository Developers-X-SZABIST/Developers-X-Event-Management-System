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

        //From what i understand, the ID is the actual foreign key, the User User is just the navigation property, meaning that we get the link from ID
        //and the User User is just to access the user object to get the details (i think view bag uses this)




        // Foreign key to link registration to the event
        [ForeignKey("Event")]
        public int EventId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)] //to avoid multiple deletes
                                                  //1. User-> Registration-> Event
                                                  //2. User-> Event
        public Event? Event { get; set; }
        //keep nullable so modal state can accept
        //We are restricting delete on event because if we are removing an event, its tied up to multiple users' registrations, so we need to remove all those registrations of the users then remove the event, but for user just removing all related data is fine.
    }
}

//For tracking event registrations by users