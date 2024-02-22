using WebAggregator.Infrastructure.Factories;
using WebAggregator.Infrastructure.Factories.Interfaces;
using WebAggregator.Infrastructure.Logic;
using WebAggregator.Infrastructure.Logic.Interfaces;
using WebAggregator.Repository;
using WebAggregator.Repository.Interfaces;

namespace WebAggregator.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureFactories(this IServiceCollection services)
    {
        services.AddScoped<IWebPageRepositoryFactory, WebPageRepositoryFactory>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IWebPageRepository, WebPageRepository>();
    }

    public static void ConfigureLogic(this IServiceCollection services)
    {
        services.AddScoped<IWebPageBusinessLogic, WebPageBusinessLogic>();
    }
}
