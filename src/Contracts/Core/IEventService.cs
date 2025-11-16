using RSession.Shared.Contracts;
using RSession.Shared.Structs;
using SwiftlyS2.Shared.Players;

namespace RSession.Contracts.Core;

internal interface IEventService : ISessionEventService
{
    void InvokeDatabaseConfigured(ISessionDatabaseService databaseService, string type);
    void InvokePlayerRegistered(IPlayer player, in SessionPlayer sessionPlayer);
    void InvokeServerRegistered(short serverId);
}
