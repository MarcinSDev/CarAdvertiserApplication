using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.DTO.BaseEntities;

namespace CarAdvertiser.DTO
{
    [Table("Image")]
    public class Image : BaseEntity
    {
        public int AdvertisementId { get; set; }

        [Required]
        [StringLength(50)]
        public string ImageName { get; set; }

        [StringLength(50)]
        public string ImageAlt { get; set; }

        [StringLength(50)]
        public string ContentType { get; set; }

        public byte[] ImageData { get; set; }

        public virtual Advertisement Advertisement { get; set; }

    }
}
