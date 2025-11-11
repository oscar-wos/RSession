using RSession.API.Structs;

namespace RSession.API.Contracts.Core;

public interface IServerService
{
    SessionsServer? Server { get; }
    void Init();
    void HandleMapLoad(string mapName);
}
