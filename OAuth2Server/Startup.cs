using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OAuth2Server.Startup))]
namespace OAuth2Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseNancy();
        }
    }
}
