namespace Syki.Back.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddRouting(options => options.LowercaseUrls = true);
    }

    public static void UseDomainExceptions(this IApplicationBuilder app)
    {
        app.UseMiddleware<DomainExceptionMiddleware>();
    }

    public static void UseControllers(this IApplicationBuilder app)
    {
        app.UseEndpoints(options =>
        {
            options.MapControllers();
        });
    }
}
