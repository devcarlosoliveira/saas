using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Saas.Web.Data.Interceptors;

public class SqliteOptimizationInterceptor : DbConnectionInterceptor
{
    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            @"
            PRAGMA journal_mode=WAL;
            PRAGMA synchronous=NORMAL;
            PRAGMA foreign_keys=ON;
            PRAGMA busy_timeout=5000;
            PRAGMA temp_store=MEMORY;
            PRAGMA cache_size=-2000;
            PRAGMA locking_mode=NORMAL;
        ";
        cmd.ExecuteNonQuery();
    }
}
