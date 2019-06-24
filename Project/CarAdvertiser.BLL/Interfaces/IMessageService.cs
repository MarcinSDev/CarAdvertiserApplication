using CarAdvertiser.DTO;

namespace CarAdvertiser.BLL.Interfaces
{
    public interface IMessageService : IService<Messages>
    {
        void SetRead(int messageId);
        void SetUnread(int messageId);
    }
}