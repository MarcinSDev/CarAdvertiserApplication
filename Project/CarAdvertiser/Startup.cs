using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarAdvertiser.Startup))]
namespace CarAdvertiser
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
