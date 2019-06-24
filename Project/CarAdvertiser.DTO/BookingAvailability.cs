using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarAdvertiser.DTO.BaseEntities;
using CarAdvertiser.DTO.ValueEntities;

namespace CarAdvertiser.DTO
{
    [Table("BookingAvailability")]
    public class BookingAvailability : BaseEntity
    {
        public BookingAvailability()
        {
            Bookings = new HashSet<Booking>();
        }

        public int AdvertId { get; set; }

        [DataType(DataType.Date)]
        public DateTime AvailableDate { get; set; }

        [ForeignKey("AdvertId")]
        public virtual Advertisement Advertisement { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
