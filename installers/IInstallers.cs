namespace dotnet_learning.installers
{
    public interface IInstallers
    {
        void InstallService(IServiceCollection services, IConfiguration configuration);
    }
}