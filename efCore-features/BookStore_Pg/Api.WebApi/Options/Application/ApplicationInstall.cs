
namespace Api.WebApi.Options.Application;

public static class ApplicationInstall
{
    public static ApplicationOptions AddApplicationOptions(this IServiceCollection service, IConfiguration configuration)
    {
        var appOption = new ApplicationOptions();
        configuration.Bind(nameof(ApplicationOptions), appOption);
        service.AddSingleton(appOption);
        return appOption;
    }
}