using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Interext.Startup))]
namespace Interext
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
