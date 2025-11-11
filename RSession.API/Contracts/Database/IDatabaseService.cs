using RSession.API.Structs;

namespace RSession.API.Contracts.Database;

public interface IDatabaseService
{
    Task InitAsync();
    Task<SessionsAlias?> GetAliasAsync(int playerId);
    Task<SessionsMap> GetMapAsync(string mapName);
    Task<SessionsPlayer> GetPlayerAsync(ulong steamId);
    Task<SessionsServer> GetServerAsync(string serverIp, ushort serverPort);
    Task<SessionsSession> GetSessionAsync(int playerId, short serverId, string ip);
    Task InsertAliasAsync(int playerId, string name);
    Task InsertMessageAsync(
        int playerId,
        long sessionId,
        short teamNum,
        bool teamChat,
        string message
    );
    Task InsertRotationAsync(short serverId, short mapId);
    Task UpdateSessionsAsync(List<int> playerIds, List<long> sessionIds);
}
