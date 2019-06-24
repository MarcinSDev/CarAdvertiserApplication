using System.ComponentModel.DataAnnotations.Schema;
using CarAdvertiser.DTO.BaseEntities;
using CarAdvertiser.DTO.ValueEntities;

namespace CarAdvertiser.DTO
{
    [Table("CarExtras")]
    public class CarExtras : BaseEntity
    {
        public int EquipmentId { get; set; }
        public int ExtrasAdvertId { get; set; }

        [ForeignKey("EquipmentId")]
        public virtual AdditionalEquipment AdditionalEquipment { get; set; }

        [ForeignKey("ExtrasAdvertId")]
        public virtual Advertisement Advertisement { get; set; }
    }
}
