using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarAdvertiser.Models
{
    public class UserAdvertsViewModel
    {
        public int AdvertId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }

        [DataType(DataType.Date)]
        public DateTime ClosingDate { get; set; }
    }
}