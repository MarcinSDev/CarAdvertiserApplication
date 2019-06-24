using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.SeatAmount")]
    public class SeatAmount : ValueBaseEntity
    {
        public SeatAmount()
        {
            Advertisements = new HashSet<Advertisement>();
        }

        [NotMapped]
        private new string Value { get; set; }

        [Column("Value")]
        public decimal NumberOfSeats { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
