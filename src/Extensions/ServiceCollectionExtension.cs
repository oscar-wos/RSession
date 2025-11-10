using Microsoft.Extensions.DependencyInjection;
using Sessions.API.Contracts.Database;
using Sessions.API.Contracts.Hook;
using Sessions.API.Contracts.Schedule;
using Sessions.Services.Database;
using Sessions.Services.Hook;
using Sessions.Services.Schedule;

namespace Sessions.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabases(this IServiceCollection services)
    {
        _ = services.AddSingleton<IDatabaseFactory, DatabaseFactory>();
        _ = services.AddSingleton<IPostgresService, PostgresService>();
        _ = services.AddSingleton<ISqlService, SqlService>();

        return services;
    }

    public static IServiceCollection AddHooks(this IServiceCollection services)
    {
        _ = services.AddSingleton<IHookManager, HookManager>();
        _ = services.AddSingleton<IOnClientDisconnectedService, OnClientDisconnectedService>();
        _ = services.AddSingleton<IOnClientMessageService, OnClientMessageService>();
        _ = services.AddSingleton<IOnClientSteamAuthorizeService, OnClientSteamAuthorizeService>();
        _ = services.AddSingleton<IOnMapLoadService, OnMapLoadService>();
        _ = services.AddSingleton<IOnSteamAPIActivatedService, OnSteamAPIActivatedService>();

        return services;
    }

    public static IServiceCollection AddSchedules(this IServiceCollection services)
    {
        _ = services.AddSingleton<IScheduleManager, ScheduleManager>();
        _ = services.AddSingleton<IIntervalService, IntervalService>();

        return services;
    }
}
