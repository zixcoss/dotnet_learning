namespace dotnet_learning.installers
{
    public static class InstallerExtension
    {
        public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x => 
            typeof(IInstallers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance).Cast<IInstallers>().ToList();
            installers.ForEach(installer => installer.InstallService(services, configuration));
        }
    }
}