using Sessions.API.Structs;

namespace Sessions.API.Contracts.Core;

public interface IServerService
{
    SessionsServer? Server { get; }
    void Init();
}
