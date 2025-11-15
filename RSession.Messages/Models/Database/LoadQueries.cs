namespace RSession.Messages.Models.Database;

internal abstract class LoadQueries
{
    protected abstract string CreateMessages { get; }

    public IEnumerable<string> GetLoadQueries()
    {
        yield return CreateMessages;
    }
}
