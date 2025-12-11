using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management_System.Models.Entities
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }




        
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }
        public string? Location { get; set; }
        public string? MaxCapacity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDeadline { get; set; }





        // Foreign key to link event to its creator (admin user or organizer user )
        //Note ROLES
        //[ForeignKey("CreatedByUser")]
        //public int CreatedByUserId { get; set; }
        //public User CreatedByUser { get; set; } // Navigation property to User entity

        // Navigation property, all registrations for this event

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        //public ICollection<EventOrganizer> EventOrganizers { get; set; } = new List<EventOrganizer>(); //tracking which organizers are assigned to this event         //ON HOLD
    }
}

//Event entity that users register for