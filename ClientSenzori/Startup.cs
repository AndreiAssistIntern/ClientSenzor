using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClientSenzori.Startup))]
namespace ClientSenzori
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
