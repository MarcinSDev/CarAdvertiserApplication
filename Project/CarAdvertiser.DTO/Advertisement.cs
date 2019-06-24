using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using CarAdvertiser.DTO.BaseEntities;
using CarAdvertiser.DTO.ValueEntities;

namespace CarAdvertiser.DTO
{
    [Table("Advertisement")]
    public class Advertisement : BaseEntity
    {
        public Advertisement()
        {
            BookingAvailabilities = new HashSet<BookingAvailability>();
            Extras = new HashSet<CarExtras>();
            Images = new HashSet<Image>();
        }
        public int Price { get; set; }

        public int CurrentMileage { get; set; }

        public int HorsePower { get; set; }

        public int AmountOfPrevOwners { get; set; }

        [Required]
        public string AdvertDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime AdvertOpenDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime AdvertCloseDate { get; set; }

        public bool IsSold { get; set; }

        public int RegYear { get; set; }

        //[Required]
        //[StringLength(128)]
        public int AppUserId { get; set; }

        public int CarModelId { get; set; }
        public int EngineSizeId { get; set; }
        public int BodyTypeId { get; set; }
        public int TransmissionId { get; set; }
        public int SeatAmountId { get; set; }
        public int DriveTrainId { get; set; }
        public int FuelTypeId { get; set; }
        public int DoorAmountId { get; set; }
        public int ColourId { get; set; }
        public bool IsPremium { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUserV2 AppUser { get; set; }

        [ForeignKey("CarModelId")]
        public virtual CarModel CarModel { get; set; }

        [ForeignKey("EngineSizeId")]
        public virtual EngineSize EngineSize { get; set; }

        [ForeignKey("BodyTypeId")]
        public virtual BodyType BodyType { get; set; }

        [ForeignKey("TransmissionId")]
        public virtual Transmission Transmission { get; set; }

        [ForeignKey("SeatAmountId")]
        public virtual SeatAmount SeatAmount { get; set; }

        [ForeignKey("DriveTrainId")]
        public virtual DriveTrain DriveTrain { get; set; }

        [ForeignKey("FuelTypeId")]
        public virtual FuelType FuelType { get; set; }

        [ForeignKey("DoorAmountId")]
        public virtual DoorAmount DoorAmount { get; set; }

        [ForeignKey("ColourId")]
        public virtual Colour Colour { get; set; }


        public virtual ICollection<BookingAvailability> BookingAvailabilities { get; set; }
        public virtual ICollection<CarExtras> Extras { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
 