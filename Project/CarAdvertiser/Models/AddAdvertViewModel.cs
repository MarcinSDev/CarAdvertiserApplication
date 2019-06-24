using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarAdvertiser.Models
{
    public class AddAdvertViewModel
    {
        public AddAdvertViewModel()
        {
            Additionals = new List<CheckBoxList>();
        }

        [Required(ErrorMessage = "Please Choose Car Manufacturer")]
        [Display(Name = "Select Car Manufacturer")]
        public int ManufacturerId { get; set; }

        [Required(ErrorMessage = "Please Choose Car Model")]
        [Display(Name = "Select Car Model")]
        public int CarModelId { get; set; }

        [Required(ErrorMessage = "Please Enter Price for the Car")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Please Enter Current Mileage for the Car")]
        public int CurrentMileage { get; set; }

        [Required(ErrorMessage = "Please Enter Horse Power of the Car")]
        public int HorsePower { get; set; }

        [Required(ErrorMessage = "Please Enter the Amount of Previous Owners")]
        public int AmountOfPrevOwners { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "First appearance of the advert")]
        [Required(ErrorMessage = "Please enter the first day of the appearance of the advert")]
        public DateTime AdvertOpenDate { get; set; }

        [Display(Name = "How many days should the advert be seen?")]
        [Range(1, 30, ErrorMessage = "Number of days must be between 1 and 30!")]
        public int AdvertAliveDays { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string AdvertDescription { get; set; }

        [Display(Name = "Select Engine Size")]
        public int EngineSizeId { get; set; }

        [Display(Name = "Select Body Type")]
        public int BodyTypeId { get; set; }

        [Display(Name = "Select Transmission")]
        public int TransmissionId { get; set; }

        [Display(Name = "Select Seat Amount")]
        public int SeatAmountId { get; set; }

        [Display(Name = "Select Drivetrain")]
        public int DriveTrainId { get; set; }

        [Display(Name = "Select Fuel Type")]
        public int FuelTypeId { get; set; }

        [Display(Name = "Select Door Amount")]
        public int DoorAmountId { get; set; }

        [Display(Name = "Select Vehicle Colour")]
        public int ColourId { get; set; }

        [Display(Name = "Select Year Of Registration")]
        public int RegYear { get; set; }

        [Display(Name = "Premium advert")]
        public bool IsPremium { get; set; }

        public double TotalPrice
        {
            get
            {
                return AdvertAliveDays * 1.2;
            }
        }

        public List<CheckBoxList> Additionals { get; set; }
    }
}