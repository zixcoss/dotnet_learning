namespace dotnet_learning.installers
{
    public class CorsInstaller : IInstallers
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                // Specific policy
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins("https://www.w3schools.com","http://localhost:7000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

                // Allow all 
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}