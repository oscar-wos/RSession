using RSession.Messages.Contracts.Core;
using RSession.Shared.Contracts;
using SwiftlyS2.Shared.Players;

namespace RSession.Messages.Services.Core;

internal sealed class PlayerService : IPlayerService
{
    private ISessionPlayerService? _sessionPlayerService;
    private ISessionServerService? _sessionServerService;

    public void Initialize(
        ISessionPlayerService sessionPlayerService,
        ISessionServerService sessionServerService
    )
    {
        _sessionPlayerService = sessionPlayerService;
        _sessionServerService = sessionServerService;
    }

    public void HandlePlayerMessage(
        IPlayer player,
        short teamNum,
        bool teamChat,
        string message
    ) { }
}
