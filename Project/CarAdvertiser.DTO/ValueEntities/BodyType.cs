using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CarAdvertiser.DTO.BaseEntities;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.BodyType")]
    public class BodyType : ValueBaseEntity
    {
        public BodyType()
        {
            Advertisements = new HashSet<Advertisement>();
        }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
