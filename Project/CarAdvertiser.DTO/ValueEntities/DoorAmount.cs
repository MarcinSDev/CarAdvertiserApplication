using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.DoorAmount")]
    public class DoorAmount : ValueBaseEntity
    {
        public DoorAmount()
        {
            Advertisements = new HashSet<Advertisement>();
        }

        [NotMapped]
        private new string Value { get; set; }

        [Column("Value")]
        public int NumberOfDoors { get; set; }

        public virtual  ICollection<Advertisement> Advertisements { get; set; }
    }
}
