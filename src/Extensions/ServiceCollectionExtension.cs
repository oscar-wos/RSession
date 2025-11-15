using Microsoft.Extensions.DependencyInjection;
using RSession.Contracts.Core;
using RSession.Contracts.Database;
using RSession.Contracts.Event;
using RSession.Contracts.Log;
using RSession.Contracts.Schedule;
using RSession.Models.Config;
using RSession.Services.Core;
using RSession.Services.Database;
using RSession.Services.Event;
using RSession.Services.Log;
using RSession.Services.Schedule;
using RSession.Shared.Contracts;

namespace RSession.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddConfigs(this IServiceCollection services)
    {
        _ = services.AddOptionsWithValidateOnStart<DatabaseConfig>().BindConfiguration("database");
        _ = services.AddOptionsWithValidateOnStart<SessionConfig>().BindConfiguration("config");

        return services;
    }

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

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        _ = services.AddSingleton<ILogService, LogService>();
        _ = services.AddSingleton<IInterval, IntervalService>();

        _ = services.AddSingleton<SessionEventService>();

        _ = services.AddSingleton<IRSessionEventServiceInternal, SessionEventService>(
            serviceProvider => serviceProvider.GetRequiredService<SessionEventService>()
        );

        _ = services.AddSingleton<IRSessionEventService, SessionEventService>(serviceProvider =>
            serviceProvider.GetRequiredService<SessionEventService>()
        );

        _ = services.AddSingleton<SessionPlayerService>();

        _ = services.AddSingleton<IRSessionPlayerServiceInternal, SessionPlayerService>(
            serviceProvider => serviceProvider.GetRequiredService<SessionPlayerService>()
        );

        _ = services.AddSingleton<IRSessionPlayerService, SessionPlayerService>(serviceProvider =>
            serviceProvider.GetRequiredService<SessionPlayerService>()
        );

        _ = services.AddSingleton<SessionServerService>();

        _ = services.AddSingleton<IRSessionServerServiceInternal, SessionServerService>(
            serviceProvider => serviceProvider.GetRequiredService<SessionServerService>()
        );

        _ = services.AddSingleton<IRSessionServerService, SessionServerService>(serviceProvider =>
            serviceProvider.GetRequiredService<SessionServerService>()
        );

        return services;
    }
}
