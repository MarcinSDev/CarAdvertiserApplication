using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.CarModel")]
    public class CarModel : ValueBaseEntity
    {
        public CarModel()
        {
            Advertisements = new HashSet<Advertisement>();
            WantedCars = new HashSet<WantedCar>();
        }

        public int ManufacturerId { get; set; }

        [ForeignKey("ManufacturerId")]
        public virtual CarManufacturer CarManufacturer { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
        public virtual ICollection<WantedCar> WantedCars { get; set; }
    }
}
