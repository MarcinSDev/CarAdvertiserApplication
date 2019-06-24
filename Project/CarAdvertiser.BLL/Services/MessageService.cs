using CarAdvertiser.BLL.Interfaces;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;

namespace CarAdvertiser.BLL.Services
{
    public class MessageService : Service<Messages>, IMessageService
    {
        public MessageService(IRepository<Messages> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }

        public void SetRead(int messageId)
        {
            Messages msg = FindById(messageId);
            msg.IsRead = true;
            Update(msg);
        }

        public void SetUnread(int messageId)
        {
            Messages msg = FindById(messageId);
            msg.IsRead = false;
            Update(msg);
        }
    }
}