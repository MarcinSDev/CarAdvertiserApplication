using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.DTO.BaseEntities;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.AdditionalEquipment")]
    public class AdditionalEquipment : ValueBaseEntity
    {
        public AdditionalEquipment()
        {
            Extras = new HashSet<CarExtras>();
        }

        public virtual ICollection<CarExtras> Extras { get; set; }
    }
}
