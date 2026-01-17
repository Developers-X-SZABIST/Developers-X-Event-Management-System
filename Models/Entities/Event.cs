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

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        
    }
}
