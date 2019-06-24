using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.EngineSize")]
    public class EngineSize : ValueBaseEntity
    {
        public EngineSize()
        {
            Advertisements = new HashSet<Advertisement>();
        }
        
        [NotMapped]
        private new string Value { get; set; }

        [Column("Value")]
        public decimal Size { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
