using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VidlyVideoRental.Startup))]
namespace VidlyVideoRental
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
