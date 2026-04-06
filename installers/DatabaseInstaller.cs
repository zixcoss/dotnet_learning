using dotnet_learning.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_learning.installers
{
    public class DatabaseInstaller : IInstallers
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionSQLServer"))
            );
        }
    }
}