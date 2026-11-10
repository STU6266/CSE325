
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace BlazingPizza;

public class OrderStore
{
    private readonly string databasePath;

    public OrderStore(IWebHostEnvironment environment)
    {
        databasePath = Path.Combine(
            environment.ContentRootPath,
            "orders.db");

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();

        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CreatedAt TEXT NOT NULL,
                OrderJson TEXT NOT NULL
            );
            """;

        command.ExecuteNonQuery();
    }

    private SqliteConnection OpenConnection()
    {
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath
        }.ToString();

        var connection = new SqliteConnection(connectionString);
        connection.Open();

        return connection;
    }

    public int SaveOrder(Order order)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO Orders (CreatedAt, OrderJson)
            VALUES ($createdAt, $orderJson);
            """;

        command.Parameters.AddWithValue(
            "$createdAt",
            order.CreatedTime.ToString("O"));

        command.Parameters.AddWithValue(
            "$orderJson",
            JsonSerializer.Serialize(order));

        command.ExecuteNonQuery();

        using var idCommand = connection.CreateCommand();
        idCommand.CommandText = "SELECT last_insert_rowid();";

        order.OrderId = Convert.ToInt32(
            idCommand.ExecuteScalar());

        return order.OrderId;
    }

    public IReadOnlyList<Order> GetAllOrders()
    {
        var orders = new List<Order>();

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT Id, OrderJson
            FROM Orders
            ORDER BY Id DESC;
            """;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var order = JsonSerializer.Deserialize<Order>(
                reader.GetString(1));

            if (order != null)
            {
                order.OrderId = reader.GetInt32(0);
                orders.Add(order);
            }
        }

        return orders;
    }
}
