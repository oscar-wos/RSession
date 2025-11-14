using Microsoft.Extensions.DependencyInjection;
using RSession.Contracts.Database;
using RSession.Contracts.Event;
using RSession.Services.Database;
using RSession.Services.Event;

namespace RSession.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabases(this IServiceCollection services)
    {
        _ = services.AddSingleton<IDatabaseFactory, DatabaseFactory>();
        _ = services.AddSingleton<IPostgresService, PostgresService>();
        _ = services.AddSingleton<ISqlService, SqlService>();

        return services;
    }

    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        _ = services.AddSingleton<IEventListener, OnClientDisconnectedService>();
        _ = services.AddSingleton<IEventListener, OnClientSteamAuthorizeService>();
        _ = services.AddSingleton<IEventListener, OnSteamAPIActivatedService>();

        return services;
    }
}
