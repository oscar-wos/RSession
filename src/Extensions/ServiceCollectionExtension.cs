using Microsoft.Extensions.DependencyInjection;
using RSession.API.Contracts.Database;
using RSession.API.Contracts.Hook;
using RSession.API.Contracts.Schedule;
using RSession.Services.Database;
using RSession.Services.Hook;
using RSession.Services.Schedule;

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
