using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarAdvertiser.BLL.Services;
using CarAdvertiser.DTO;

namespace CarAdvertiser.Models
{
    public class SmallSearchResultViewModel
    {
        public SmallSearchResultViewModel()
        {
            //Advertisement = new Advertisement();
            Images = new List<byte[]>();
        }
        public int AdvertisementId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }
        public decimal EngineSize { get; set; }
        public int RegYear { get; set; }
        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                string result = Description;
                if (!string.IsNullOrEmpty(Description))
                {
                    if (Description.Length > 100)
                    {
                        result = Description.Substring(0, 100).Trim();//we may cut in half a word in this case, so...
                        var lastWhiteSpace = result.LastIndexOf(" ", StringComparison.InvariantCultureIgnoreCase);//we look for the last whitespace...
                        result = result.Substring(0, lastWhiteSpace).Trim() + " [...]";//and drop the half-cut word :)
                    }
                }

                return result;
            }
        }
        public IEnumerable<byte[]> Images { get; set; }
        //public Advertisement Advertisement;
    }
}