using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.Colour")]
    public class Colour : ValueBaseEntity
    {
        public Colour()
        {
            Advertisements = new HashSet<Advertisement>();
        }
        
        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
