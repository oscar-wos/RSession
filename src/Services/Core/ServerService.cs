using Sessions.API.Contracts.Core;
using Sessions.API.Structs;

namespace Sessions.Services.Core;

public sealed class ServerService : IServerService
{
    public Server? Server => throw new NotImplementedException();

    public Map? Map => throw new NotImplementedException();
}
