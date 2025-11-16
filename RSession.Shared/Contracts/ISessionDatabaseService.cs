using System.Data.Common;

namespace RSession.Shared.Contracts;

public interface ISessionDatabaseService
{
    Task<DbConnection> GetConnectionAsync();
}
