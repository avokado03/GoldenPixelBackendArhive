using GoldenPixel.TestInfrastructure.Properties;
using Npgsql;

namespace GoldenPixel.TestInfrastructure.DummyDb;

public static class DummyDb
{
	private static string ServerConnectionString { get; set; }

	private static string DbName { get; set; }

	public static string DbConnectionString { get; private set; }

	public static void CreateDbIfNotExist()
	{
		if (string.IsNullOrWhiteSpace(ServerConnectionString))
			ServerConnectionString = GetServerConnectionString();

		if (string.IsNullOrEmpty(DbConnectionString))
			DbConnectionString = CreateConnectionString();

		var query = GetCreateQueryFromFile();
		ExecuteNonQuery(query);
	}

	public static void DropDbIfExist()
	{
		var query = $"DROP DATABASE IF EXISTS {DbName}";
		ExecuteNonQuery(query);
	}

	public static void TruncateTableIfExist(string tableName)
	{
		if (string.IsNullOrEmpty(tableName))
			throw new ArgumentNullException("Имя таблицы пустое");

		if (!CheckIfTableExists(tableName))
			throw new ArgumentException("Имя таблицы неверно указано или не существует");

		var query = $@"TRUNCATE {tableName} RESTART IDENTITY CASCADE;";
		ExecuteNonQuery(query);
	}

	private static string CreateConnectionString()
	{
		if (string.IsNullOrEmpty(ServerConnectionString))
			throw new ArgumentNullException(nameof(ServerConnectionString));

		var dbNamePostfix = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
		var dbName = "GpDb_"+dbNamePostfix;
		var connectionString = $"{ServerConnectionString};Database:{dbName}";
		DbName = dbName;
		return connectionString;
	}

	private static string GetServerConnectionString()
	{
		var serverConnection = Environment.GetEnvironmentVariable("PG_CONNECTION");
		if (string.IsNullOrEmpty(serverConnection))
			throw new NullReferenceException("Нет переменной окружения подключения PG_CONNECTION");
		return serverConnection;
	}

	private static string GetCreateQueryFromFile()
	{
		var manager = Resources.ResourceManager.GetObject("DbScript");
		if (manager is null)
			throw new NullReferenceException("Файл запросов не найден");

		string query = Resources.DbScript;
		if (string.IsNullOrEmpty(query))
			throw new NullReferenceException("Файл запросов пуст.");

		query = query.Replace("{DbName}", DbConnectionString);

		return query;
	}

	private static void ExecuteNonQuery(string query)
	{
		if (string.IsNullOrEmpty(query))
			throw new ArgumentNullException("Текст запроса пуст");

		if (string.IsNullOrEmpty(DbName))
			throw new ArgumentNullException("Имя БД пустое");

		using var connection = new NpgsqlConnection(DbConnectionString);
		connection.Open();
		var command = new NpgsqlCommand(query, connection);
		int rowsAffected = command.ExecuteNonQuery();
		connection.Close();
		if (rowsAffected == -1)
			throw new Exception($"Ошибка БД при выполнении запроса \n {query}");
	}

	private static T? ExecuteScalar<T>(string query)
		where T : new()
	{
		if (string.IsNullOrEmpty(query))
			throw new ArgumentNullException("Текст запроса пуст");

		if (string.IsNullOrEmpty(DbName))
			throw new ArgumentNullException("Имя БД пустое");

		T result = new();
		using var connection = new NpgsqlConnection(DbConnectionString);
		try
		{
			connection.Open();
			var command = new NpgsqlCommand(query, connection);
			result = (T)command.ExecuteScalar();
		}
		catch (NullReferenceException ex)
		{
			throw new Exception($"Запрос не вернул значения \n {query}", ex);
		}
		finally
		{
			connection.Close();
		}
		return result;
	}

	private static bool CheckIfTableExists(string tableName)
	{
		if (string.IsNullOrEmpty(tableName))
			throw new ArgumentNullException("Имя таблицы пустое");

		var query =
$@"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '{tableName}') AS existence;";

		var result = ExecuteScalar<bool>(query);

		return result;
	}
}

