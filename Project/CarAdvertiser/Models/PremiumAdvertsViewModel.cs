using System.Collections.Generic;

namespace CarAdvertiser.Models
{
    public class PremiumAdvertViewModel
    {
        public PremiumAdvertViewModel()
        {
            Images = new List<byte[]>();
        }

        public string Make { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
        public int AdvertId { get; set; }
        public decimal EngineSize { get; set; }
        public string BodyType { get; set; }
        public string FuelType { get; set; }
        public int RegYear { get; set; }
        public string Colour { get; set; }
        public string Description { get; set; }
        public string SellerName { get; set; }
        public IEnumerable<byte[]> Images { get; set; }
    }
}