using Sessions.API.Contracts.Core;
using Sessions.API.Contracts.Database;
using Sessions.API.Structs;

namespace Sessions.Services.Core;

public sealed class PlayerService(IDatabaseFactory databaseFactory) : IPlayerService
{
    private readonly IDatabaseService _database = databaseFactory.Database;

    public Player? Player { get; private set; }

    public Session? Session { get; private set; }

    public async Task LoadPlayerAsync(ulong steamId)
    {
        Player = await _database.GetPlayerAsync(steamId);

        if (Player.HasValue)
        {
            await _database.UpdateSeenAsync(Player.Value.Id);
        }
    }
}
