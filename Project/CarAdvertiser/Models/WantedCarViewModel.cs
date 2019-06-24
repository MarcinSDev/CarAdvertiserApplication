using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarAdvertiser.Models
{
    public class WantedCarViewModel
    {
        public int Make { get; set; }
        public int Model { get; set; }
        public int MinYear { get; set; }
        public int MaxPrice { get; set; }

    }
}