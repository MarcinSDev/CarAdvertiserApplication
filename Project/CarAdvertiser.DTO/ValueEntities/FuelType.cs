using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.FuelType")]
    public class FuelType : ValueBaseEntity
    {
        public FuelType()
        {
            Advertisements = new HashSet<Advertisement>();
        }
        
        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
