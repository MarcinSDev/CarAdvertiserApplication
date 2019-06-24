using CarAdvertiser.DTO.BaseEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAdvertiser.DTO
{
    [Table("Messages")]
    public class Messages : BaseEntity
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        [Required]
        public string MessageContent { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("SenderId")]
        public virtual AppUserV2 MessageSender { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual AppUserV2 MessageReceiver { get; set; }
    }
}
