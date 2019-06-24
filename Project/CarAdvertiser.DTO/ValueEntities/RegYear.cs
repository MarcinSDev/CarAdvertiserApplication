using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.DTO.BaseEntities;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.RegYear")]
    public class RegYear : ValueBaseEntity
    {
        public RegYear()
        {
            Advertisements = new HashSet<Advertisement>();
        }

        [NotMapped]
        private new string Value { get; set; }

        [Column("Value")]
        public int RegisteredYear { get; set; }
        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
