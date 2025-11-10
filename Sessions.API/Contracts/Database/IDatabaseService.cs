using Sessions.API.Structs;

namespace Sessions.API.Contracts.Database;

public interface IDatabaseService
{
    Task InitAsync();
    Task<SessionsAlias?> GetAliasAsync(int playerId);
    Task<SessionsMap> GetMapAsync(string mapName);
    Task<SessionsPlayer> GetPlayerAsync(ulong steamId);
    Task<SessionsServer> GetServerAsync(string serverIp, ushort serverPort);
    Task<SessionsSession> GetSessionAsync(int playerId, int serverId, string ip);
    Task InsertAliasAsync(int playerId, string name);
    Task InsertMessageAsync(
        int playerId,
        long sessionId,
        short teamNum,
        bool teamChat,
        string message
    );
    Task UpdateSessionsAsync(IEnumerable<int> playerIds, IEnumerable<long> sessionIds);
}
