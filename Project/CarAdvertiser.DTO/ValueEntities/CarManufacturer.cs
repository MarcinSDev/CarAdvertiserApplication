using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.CarManufacturer")]
    public class CarManufacturer : ValueBaseEntity
    {
        public CarManufacturer()
        {
            CarModels = new HashSet<CarModel>();
        }

        public virtual ICollection<CarModel> CarModels { get; set; }
    }
}
