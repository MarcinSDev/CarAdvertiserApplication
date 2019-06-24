using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarAdvertiser.DTO.ValueEntities;

namespace CarAdvertiser.Models
{
    public class AdvertSearchViewModel
    {
        public AdvertSearchViewModel()
        {
            Styles = new List<CheckBoxList>();
            Engines = new List<CheckBoxList>();
            Doors = new List<CheckBoxList>();
            Seats = new List<CheckBoxList>();
            Colours = new List<CheckBoxList>();
            Transmissions = new List<CheckBoxList>();
            Drivetrains = new List<CheckBoxList>();
            Fueltypes = new List<CheckBoxList>();
        }

        public int? MinRegYear { get; set; }
        public int? MaxRegYear { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MaxPrevOwners { get; set; }
        public int? MaxMilleage { get; set; }
        public int? MinHorsePower { get; set; }

        public CarManufacturer CarManufacturer { get; set; }
        public CarModel CarModel { get; set; }
        public List<CheckBoxList> Styles { get; set; }
        public List<CheckBoxList> Engines { get; set; }
        public List<CheckBoxList> Doors { get; set; }
        public List<CheckBoxList> Seats { get; set; }
        public List<CheckBoxList> Colours { get; set; }
        public List<CheckBoxList> Transmissions { get; set; }
        public List<CheckBoxList> Drivetrains { get; set; }
        public List<CheckBoxList> Fueltypes { get; set; }


    }
}