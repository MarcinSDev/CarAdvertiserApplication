using System.Threading.Tasks;
using CarAdvertiser.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Identity
{
    public class EmailV2Service : IIdentityMessageService, IEmailService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.FromResult(0);
        }
    }
}