using SwiftlyS2.Shared.ProtobufDefinitions;

namespace Sessions.API.Contracts.Hook;

public interface IPlayerMessageService
{
    void OnClientMessage(in CUserMessageSayText2 msg);
}
