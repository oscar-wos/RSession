using RSession.Shared.Contracts;

namespace RSession.Contracts.Core;

internal interface IServerService : ISessionServerService
{
    void Initialize();
}
