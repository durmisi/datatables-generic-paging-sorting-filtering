using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DataTablesGenerics.Startup))]
namespace DataTablesGenerics
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
