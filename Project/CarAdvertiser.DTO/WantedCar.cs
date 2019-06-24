using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.DTO.BaseEntities;
using CarAdvertiser.DTO.ValueEntities;

namespace CarAdvertiser.DTO
{
    [Table("WantedCar")]
    public class WantedCar : BaseEntity
    {
        public int MinimumYear { get; set; }
        public decimal MaxPrice { get; set; }
        public bool IsWanted { get; set; }

        public int UserId { get; set; }

        public int ModelId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUserV2 AppUser { get; set; }

        [ForeignKey("ModelId")]
        public virtual CarModel CarModel { get; set; }
    }
}
