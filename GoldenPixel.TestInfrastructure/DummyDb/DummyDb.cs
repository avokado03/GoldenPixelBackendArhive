using GoldenPixel.TestInfrastructure.Properties;
using Npgsql;

namespace GoldenPixel.TestInfrastructure.DummyDb;

/// <summary>
/// Класс для работы с тестовой БД и запросами к ней
/// </summary>
public static class DummyDb
{
    private static string ServerConnectionString { get; set; }

    private static string DbName { get; set; }

    private static string DefaultConnectionString { get; set; }

    public static string DbConnectionString { get; private set; }

    public static void CreateDbIfNotExist()
    {
        if (string.IsNullOrWhiteSpace(ServerConnectionString))
            ServerConnectionString = GetServerConnectionString();

        if (string.IsNullOrWhiteSpace(DefaultConnectionString))
            DefaultConnectionString = CreateDefaultConnectionString();

        if (string.IsNullOrEmpty(DbConnectionString))
            DbConnectionString = CreateConnectionString();

        var dbQuery = GetCreateQueryFromFile("DbScript");
        ExecuteNonQuery(dbQuery, DefaultConnectionString);

        var tablesQuery = GetCreateQueryFromFile("TablesScript");
        ExecuteNonQuery(tablesQuery, DbConnectionString);
    }

    public static void DropDbIfExist()
    {
        var query = $"DROP DATABASE \"{DbName}\"";
        ExecuteNonQuery(query, DefaultConnectionString);
    }

    public static void TruncateTableIfExist(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentNullException("Имя таблицы пустое");

        if (!CheckIfTableExists(tableName))
            throw new ArgumentException("Имя таблицы неверно указано или не существует");

        var query = $@"TRUNCATE {tableName} RESTART IDENTITY CASCADE;";
        ExecuteNonQuery(query, DbConnectionString);
    }

    private static string CreateConnectionString()
    {
        if (string.IsNullOrEmpty(ServerConnectionString))
            throw new ArgumentNullException(nameof(ServerConnectionString));

        var dbNamePostfix = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        var dbName = "GpDb_" + dbNamePostfix;
        var connectionString = $"{ServerConnectionString}Database={dbName};";
        DbName = dbName;
        return connectionString;
    }

    private static string CreateDefaultConnectionString()
    {
        if (string.IsNullOrEmpty(ServerConnectionString))
            throw new ArgumentNullException(nameof(ServerConnectionString));

        var connectionString = $"{ServerConnectionString}Database=postgres;";
        return connectionString;
    }

    private static string GetServerConnectionString()
    {
        var serverConnection = Environment.GetEnvironmentVariable("PG_CONNECTION", EnvironmentVariableTarget.Machine);
        if (string.IsNullOrEmpty(serverConnection))
            throw new NullReferenceException("Нет переменной окружения подключения PG_CONNECTION");
        return serverConnection;
    }

    private static string GetCreateQueryFromFile(string fileName)
    {
        var query = Resources.ResourceManager.GetObject(fileName).ToString();

        if (string.IsNullOrEmpty(query))
            throw new NullReferenceException("Файл запросов пуст.");

        query = query.Replace("{DbName}", DbName);

        return query;
    }

    private static void ExecuteNonQuery(string query, string connectionString)
    {
        if (string.IsNullOrEmpty(query))
            throw new ArgumentNullException("Текст запроса пуст");

        if (string.IsNullOrEmpty(DbName))
            throw new ArgumentNullException("Имя БД пустое");

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            var command = new NpgsqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
        }
    }

    private static T? ExecuteScalar<T>(string query, string connectionString)
        where T : new()
    {
        if (string.IsNullOrEmpty(query))
            throw new ArgumentNullException("Текст запроса пуст");

        if (string.IsNullOrEmpty(DbName))
            throw new ArgumentNullException("Имя БД пустое");

        T? result = new();
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                var command = new NpgsqlCommand(query, connection);
                var res = command.ExecuteScalar();
                result = (T?)res;
            }
            catch (NullReferenceException ex)
            {
                throw new Exception($"Запрос не вернул значения \n {query}", ex);
            }
        }
        return result;
    }

    private static bool CheckIfTableExists(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentNullException("Имя таблицы пустое");

        var query =
$@"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '{tableName}') AS existence;";

        var result = ExecuteScalar<bool>(query, DbConnectionString);

        return result;
    }
}
