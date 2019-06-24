using System.ComponentModel.DataAnnotations.Schema;
using CarAdvertiser.DTO.BaseEntities;

namespace CarAdvertiser.DTO
{
    public class AppUserRole : BaseEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUserV2 User { get; set; }

        [ForeignKey("RoleId")]
        public virtual AppRole Role { get; set; }
    }
}