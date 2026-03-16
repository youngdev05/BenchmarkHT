using System.Data;
using Microsoft.Data.Sqlite;

namespace OrmBenchmark.Data;

public static class DbHelper
{
    public static readonly string ConnectionString = "Data Source=benchmark.db";

    public static IDbConnection CreateConnection() => new SqliteConnection(ConnectionString);
}