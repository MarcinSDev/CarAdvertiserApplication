using CarAdvertiser.DTO.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO.ValueEntities
{
    [Table("Value.DriveTrain")]
    public class DriveTrain : ValueBaseEntity
    {
        public DriveTrain()
        {
            Advertisements = new HashSet<Advertisement>();
        }
        
        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
