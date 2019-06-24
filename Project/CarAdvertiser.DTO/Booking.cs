using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarAdvertiser.DTO.BaseEntities;
using CarAdvertiser.DTO.ValueEntities;

namespace CarAdvertiser.DTO
{
    [Table("Booking")]
    public class Booking : BaseEntity
    {
        public int AvailabilityId { get; set; }
        
        public int BookingUserId { get; set; }

        [ForeignKey("AvailabilityId")]
        public virtual BookingAvailability BookingAvailability { get; set; }

        [ForeignKey("BookingUserId")]
        public virtual AppUserV2 AppUser { get; set; }
    }
}
