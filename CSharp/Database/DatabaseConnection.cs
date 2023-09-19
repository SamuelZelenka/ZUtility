using System.Text;
using MySql.Data.MySqlClient;

public class DatabaseConnection : IDisposable
{
	private MySqlConnection _connection;

	public void Insert(string tableName, IQueryCreator query)
	{
		Insert(tableName, query.ToParameters());
	}

	public void Insert(string tableName, params (string key, string value)[] parameters)
	{
		var command = InsertCommandBuilder(tableName, parameters);
		ExecQueryCommand(command);
	}

	private MySqlCommand InsertCommandBuilder(string tableName, params (string key, string value)[] parameters)
	{
		string query = QueryBuilder(tableName, parameters);
		MySqlCommand command = new MySqlCommand(query, _connection);

		for (int i = 0; i < parameters.Length; i++)
		{
			command.Parameters.AddWithValue($"@Value{i}" , parameters[i].value);
		}
		return command;
	}

	//Hacky Test Query. Replace with standardized query procedure
	private string QueryBuilder(string tableName, params (string key, string value)[] parameters)
	{
		StringBuilder columnBuilder = new StringBuilder();
		StringBuilder rowBuilder = new StringBuilder();
		
		columnBuilder.Append("INSERT INTO ");
		columnBuilder.Append(tableName);
		columnBuilder.Append("(");

		rowBuilder.Append("VALUES (");

		for (int i = 0; i < parameters.Length; i++)
		{
			columnBuilder.Append(parameters[i].key);
			rowBuilder.Append("@Value");
			rowBuilder.Append(i);

			if (i == parameters.Length - 1)
				break;

			columnBuilder.Append(", ");
			rowBuilder.Append(", ");
		}

		columnBuilder.Append(") ");
		rowBuilder.Append(")");
		columnBuilder.Append(rowBuilder);

		return columnBuilder.ToString();
	}

	public void Open()
	{
		if (_connection.State != System.Data.ConnectionState.Open)
		{
			_connection.Open();
		}
	}

	public void Close()
	{
		if (_connection.State != System.Data.ConnectionState.Closed)
		{
			_connection.Close();
		}
	}

	public void Dispose()
	{
		Close();
		GC.SuppressFinalize(this); // Don't dispose again in destructor
	}

	public void ExecQueryCommand(MySqlCommand SqlCommand)
	{
		try
		{
			Open();
			SqlCommand.ExecuteNonQuery();
			Console.WriteLine("Connection Open");
		}
		catch (Exception exception)
		{
			Console.WriteLine("DATABASE CONNECTION ERROR: " + exception.Message);
		}
		finally
		{
			Close();
			Console.WriteLine("Connection Close");
		}
	}

	public DatabaseConnection(string adress, string database, string username, string password)
	{
		string _sqlConnectionString =
			$"Server={adress};" +
			$"Database={database};" +
			$"User={username};" +
			$"Password={password};";

		_connection = new MySqlConnection(_sqlConnectionString);
	}

	~DatabaseConnection()
	{
		Dispose(); // Ensure connection is closed.
	}
}
