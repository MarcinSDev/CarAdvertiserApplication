using CarAdvertiser.DTO.BaseEntities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO
{
    public class AppRole : BaseEntity, IRole<int>
    {
        public AppRole()
        {
            Users = new HashSet<AppUserRole>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<AppUserRole> Users { get; set; }
    }
}