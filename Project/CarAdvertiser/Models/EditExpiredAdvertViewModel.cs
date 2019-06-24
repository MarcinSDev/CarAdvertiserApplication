using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarAdvertiser.Models
{
    public class EditExpiredAdvertViewModel
    {
        public int AdvertId { get; set; }
        public int Mileage { get; set; }
        public int Price { get; set; }
        public DateTime NewOpenDate { get; set; }
        public DateTime NewCloseDate { get; set; }
    }
}