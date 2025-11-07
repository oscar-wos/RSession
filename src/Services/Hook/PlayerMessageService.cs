using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Hook;
using Sessions.API.Contracts.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace Sessions.Services.Hook;

public sealed class PlayerMessageService(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<PlayerMessageService> logger
) : IPlayerMessageService
{
    private readonly ISwiftlyCore _core = core;
    private readonly ILogService _logService = logService;
    private readonly ILogger<PlayerMessageService> _logger = logger;

    public void OnClientMessage(in CUserMessageSayText2 msg)
    {
        int entity = msg.Entityindex;
        string message = msg.Param2;
        string messageName = msg.Messagename;

        if (_core.PlayerManager.GetPlayer(entity - 1) is not { } player)
        {
            _logService.LogWarning($"Player not found - {entity}", logger: _logger);
            return;
        }

        _logService.LogInformation(
            $"Message - {player.Controller.PlayerName}: {message} ({messageName})",
            logger: _logger
        );

        /*
            int entity = msg.Entityindex;
            string message = msg.Param2;
            string messageName = msg.Messagename;

            // Cstrike_Chat_All
            // Cstrike_Chat_AllSpec
            // Cstrike_Chat_Spec
            // Cstrike_Chat_T_Loc
            // Cstrike_Chat_CT_Loc
            */
    }
}
