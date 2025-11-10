using SwiftlyS2.Shared.ProtobufDefinitions;

namespace Sessions.API.Contracts.Hook;

public interface IOnClientMessageService
{
    void OnClientMessage(in CUserMessageSayText2 msg);
}
