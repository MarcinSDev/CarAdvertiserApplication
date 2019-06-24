using System;

namespace CarAdvertiser.Models
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string ReceivedDate { get; set; }
        public string SenderName { get; set; }
    }
}