using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DAWProiect.Startup))]
namespace DAWProiect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
