using RSession.Messages.Contracts.Core;
using RSession.Shared.Contracts;
using SwiftlyS2.Shared.Players;

namespace RSession.Messages.Services.Core;

internal sealed class PlayerService : IPlayerService
{
    private ISessionPlayerService? _playerService;
    private ISessionServerService? _serverService;

    public void Initialize(ISessionPlayerService playerService, ISessionServerService serverService)
    {
        _playerService = playerService;
        _serverService = serverService;
    }

    public void HandlePlayerMessage(
        IPlayer player,
        short teamNum,
        bool teamChat,
        string message
    ) { }
}
