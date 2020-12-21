using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MastaneClinic.Startup))]
namespace MastaneClinic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
