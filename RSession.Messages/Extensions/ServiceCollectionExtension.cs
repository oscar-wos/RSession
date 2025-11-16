using Microsoft.Extensions.DependencyInjection;
using RSession.Messages.Contracts.Core;
using RSession.Messages.Contracts.Database;
using RSession.Messages.Contracts.Log;
using RSession.Messages.Services.Core;
using RSession.Messages.Services.Database;
using RSession.Messages.Services.Event;
using RSession.Messages.Services.Log;

namespace RSession.Messages.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        _ = services.AddSingleton<IDatabaseFactory, DatabaseFactory>();
        _ = services.AddSingleton<PostgresService>();
        _ = services.AddSingleton<SqlService>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        _ = services.AddSingleton<ILogService, LogService>();
        _ = services.AddSingleton<IPlayerService, PlayerService>();

        _ = services.AddSingleton<OnDatabaseConfiguredService>();

        return services;
    }
}
