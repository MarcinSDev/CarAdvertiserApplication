using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarAdvertiser.BLL.Services;
using CarAdvertiser.DTO;

namespace CarAdvertiser.Models
{
    public class SelectedAdvertViewModel
    {
        public SelectedAdvertViewModel()
        {
            Images = new List<byte[]>();
            Extras = new List<CarExtras>();
            BookingDates = new List<CheckBoxList>();
        }
        public int Id { get; set; }
        public int AdvertiserId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int HorsePower { get; set; }
        public int AmountOfOwners { get; set; }
        public string Description { get; set; }
        public DateTime AdvertCloseDate { get; set; }
        public string BodyType { get; set; }
        public string Transmission { get; set; }
        public decimal SeatAmount { get; set; }
        public string DriveTrain { get; set; }
        public string FuelType { get; set; }
        public int DoorAmount { get; set; }
        public string Colour { get; set; }
        public int RegYear { get; set; }
        public int Price { get; set; }
        public decimal EngineSize { get; set; }
        public IEnumerable<byte[]> Images { get; set; }
        public IEnumerable<CarExtras> Extras { get; set; }
        public List<CheckBoxList> BookingDates { get; set; }
    }
}