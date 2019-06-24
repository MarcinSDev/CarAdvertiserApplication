using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarAdvertiser.DTO.Interfaces;

namespace CarAdvertiser.DTO.BaseEntities
{
    public abstract class BaseEntity : IAuditableEntity
    {
        protected BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CreateUser { get; set; }

        public DateTime? LastModificationDate { get; set; }

        [StringLength(50)]
        public string LastModificationUser { get; set; }
    }
}
