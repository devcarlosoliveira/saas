using Microsoft.EntityFrameworkCore.Storage;

namespace Saas.Web.Data.Interceptors;

public class SqliteRetryingExecutionStrategy : ExecutionStrategy
{
    public SqliteRetryingExecutionStrategy(ExecutionStrategyDependencies deps)
        : base(deps, 5, TimeSpan.FromMilliseconds(200)) { }

    protected override bool ShouldRetryOn(Exception exception)
    {
        Console.WriteLine("Retrying because: " + exception.Message);
        return exception.Message.Contains("database is locked");
    }
}
