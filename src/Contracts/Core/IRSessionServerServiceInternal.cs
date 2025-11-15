using RSession.Shared.Contracts;

namespace RSession.Contracts.Core;

internal interface IRSessionServerServiceInternal : IRSessionServerService
{
    void Initialize();
}
