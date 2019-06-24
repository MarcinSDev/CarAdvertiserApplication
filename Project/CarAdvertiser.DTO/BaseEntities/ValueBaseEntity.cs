using System.ComponentModel.DataAnnotations;
using CarAdvertiser.DTO.Interfaces;

namespace CarAdvertiser.DTO.BaseEntities
{
    public abstract class ValueBaseEntity : BaseEntity, IValueEntity
    {
        [Required]
        [StringLength(50)]
        public string Value { get; set; }
    }
}