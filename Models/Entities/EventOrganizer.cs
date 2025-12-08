using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management_System.Models.Entities
{
    public class EventOrganizer
    {
        //[Key]
        //public int EventOrganizerId { get; set; }


        ////Foreign key to the Event being organized
        //[ForeignKey("Event")]
        //public int EventId { get; set; }
        //public Event Event { get; set; }

        ////Foreign to the User who is the organizer
        //[ForeignKey("Organizer")]
        //public int OrganizerId { get; set; }
        //public User Organizer { get; set; }

    }
}


//Since User and Event have a many to many relationship, we need an intermediary entity to represent the relationship between them. This entity will hold foreign keys to both User and Event entities.

