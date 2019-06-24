using CarAdvertiser.DTO.BaseEntities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO
{
    public class AppUserV2 : BaseEntity, IUser<int>
    {
        private string _username;
        private string _email;

        public AppUserV2()
        {
            Advertisements = new HashSet<Advertisement>();
            Bookings = new HashSet<Booking>();
            WantedCars = new HashSet<WantedCar>();
            Roles = new HashSet<AppUserRole>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName
        {
            get => _username;
            set => _username = value.Trim().ToLower();
        }

        [EmailAddress]
        [StringLength(256)]
        public string Email
        {
            get => _email;
            set => _email = value.Trim().ToLower();
        }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Fullname { get; set; }
        
        public virtual ICollection<Advertisement> Advertisements { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<WantedCar> WantedCars { get; set; }
        public virtual ICollection<AppUserRole> Roles { get; set; }
    }
}