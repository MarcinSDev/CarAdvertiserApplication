using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarAdvertiser.Models
{
    public class MyBookingsViewModel
    {
        public int BookingId { get; set; }
        public int AvailabilityId { get; set; }
        public int BookingUserId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string AdvertOwner { get; set; }
        public DateTime BookingDate { get; set; }
    }
}