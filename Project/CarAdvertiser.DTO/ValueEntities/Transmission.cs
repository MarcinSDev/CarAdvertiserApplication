using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.Transmission")]
    public class Transmission : ValueBaseEntity
    {
        public Transmission()
        {
            Advertisements = new HashSet<Advertisement>();
        }
        
        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
