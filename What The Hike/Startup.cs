using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(What_The_Hike.Startup))]
namespace What_The_Hike
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
