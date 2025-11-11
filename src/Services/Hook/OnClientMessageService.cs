using Microsoft.Extensions.Logging;
using RSession.API.Contracts.Core;
using RSession.API.Contracts.Hook;
using RSession.API.Contracts.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace RSession.Services.Hook;

public sealed class OnClientMessageService(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<OnClientMessageService> logger,
    Lazy<IPlayerService> playerService
) : IOnClientMessageService
{
    private static readonly uint _cStrikeChatAllHash = MurmurHash2.HashString("Cstrike_Chat_All");

    private static readonly uint _cStrikeChatAllSpecHash = MurmurHash2.HashString(
        "Cstrike_Chat_AllSpec"
    );

    private readonly ISwiftlyCore _core = core;

    private readonly ILogService _logService = logService;
    private readonly ILogger<OnClientMessageService> _logger = logger;

    private readonly Lazy<IPlayerService> _playerService = playerService;

    public void OnClientMessage(in CUserMessageSayText2 msg)
    {
        int playerId = msg.Entityindex - 1;

        if (_core.PlayerManager.GetPlayer(playerId) is not { } player)
        {
            _logService.LogWarning($"Player not found - {playerId}", logger: _logger);
            return;
        }

        string message = msg.Param2;
        string messageName = msg.Messagename;

        short teamNum = player.Controller.TeamNum;
        bool teamChat = true;

        uint messageNameHash = MurmurHash2.HashString(messageName);

        if (messageNameHash == _cStrikeChatAllHash || messageNameHash == _cStrikeChatAllSpecHash)
        {
            teamChat = false;
        }

        _logService.LogDebug(
            $"Message - {player.Controller.PlayerName}: {message} ({messageName}) | Num: {teamNum} | Team: {teamChat}",
            logger: _logger
        );

        _playerService.Value.HandlePlayerMessage(player, teamNum, teamChat, message);
    }
}
