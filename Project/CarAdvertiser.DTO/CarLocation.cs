using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.DTO.BaseEntities;

namespace CarAdvertiser.DTO
{   
    [Table("CarLocation")]
    public class CarLocation : BaseEntity
    {
        public int CarAdvertId { get; set; }

        [Required]
        [StringLength(8)]
        public string PostCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [ForeignKey("CarAdvertId")]
        public virtual Advertisement Advertisement { get; set; }
    }
}
