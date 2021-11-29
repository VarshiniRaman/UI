using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HP.Robotics.OSSCommentsUploadUI.Startup))]
namespace HP.Robotics.OSSCommentsUploadUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
