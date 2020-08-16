using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BingCC.Startup))]
namespace BingCC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
