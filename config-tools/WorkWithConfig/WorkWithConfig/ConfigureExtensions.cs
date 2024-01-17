using WorkWithConfig.ConfigureOptions;

namespace WorkWithConfig;

public static class ConfigureExtensions
{
    public static ApplicationOptions RegisterApplicationOptions(this IServiceCollection service, IConfiguration configuration)
    {
        var appOption = new ApplicationOptions();
        configuration.Bind(nameof(ApplicationOptions), appOption);
        service.AddSingleton(appOption);
        return appOption;
    }
}