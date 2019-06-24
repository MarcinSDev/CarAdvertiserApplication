using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface ISmsService
    {
        Task SendAsync(IdentityMessage message);
    }
}