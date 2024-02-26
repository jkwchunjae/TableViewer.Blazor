using Microsoft.Extensions.DependencyInjection;

namespace TableViewerBlazor.Service;

public static class TableViewerExtensions
{
    public static IServiceCollection AddTableViewer(this IServiceCollection services)
    {
        services.AddScoped<DateTimeService>();
        return services;
    }
}