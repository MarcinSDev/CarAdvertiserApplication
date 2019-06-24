using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(IdentityMessage message);
    }
}