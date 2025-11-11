using SwiftlyS2.Shared.ProtobufDefinitions;

namespace RSession.API.Contracts.Hook;

public interface IOnClientMessageService
{
    void OnClientMessage(in CUserMessageSayText2 msg);
}
