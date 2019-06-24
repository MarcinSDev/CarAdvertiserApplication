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
    [Table("Value.ListingTime")]
    public class ListingTime : ValueBaseEntity
    {
        [NotMapped]
        private new string Value { get; set; }

        [Column("Value")]
        public int AdvertLength { get; set; }

        public int Cost { get; set; }
    }
}
