using System;
using OrmBenchmark.Models;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Threading.Tasks;
using OrmBenchmark.Data;

namespace OrmBenchmark.Repositories;

public class AdoRepository
{
    private readonly string _connStr = DbHelper.ConnectionString;

    public async Task<Product> CreateAsync(Product product)
    {
        using var conn = new SqliteConnection(_connStr);
        await conn.OpenAsync();
        var cmd = new SqliteCommand(
            @"INSERT INTO Products (Name, Price, Category) 
              VALUES (@Name, @Price, @Category); 
              SELECT last_insert_rowid();", conn);
        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@Category", product.Category);
        var id = (int)(long)await cmd.ExecuteScalarAsync();
        product.Id = id;
        return product;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var conn = new SqliteConnection(_connStr);
        await conn.OpenAsync();
        var cmd = new SqliteCommand(
            "SELECT Id, Name, Price, Category FROM Products WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Product
            {
                Id = (int)(long)reader["Id"],
                Name = (string)reader["Name"],
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Category = (string)reader["Category"]
            };
        }
        return null;
    }

    public async Task UpdateAsync(Product product)
    {
        using var conn = new SqliteConnection(_connStr);
        await conn.OpenAsync();
        var cmd = new SqliteCommand(
            "UPDATE Products SET Name=@Name, Price=@Price, Category=@Category WHERE Id=@Id", conn);
        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@Category", product.Category);
        cmd.Parameters.AddWithValue("@Id", product.Id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = new SqliteConnection(_connStr);
        await conn.OpenAsync();
        var cmd = new SqliteCommand("DELETE FROM Products WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task ClearAsync()
    {
        using var conn = new SqliteConnection(_connStr);
        await conn.OpenAsync();
        var cmd = new SqliteCommand("DELETE FROM Products", conn);
        await cmd.ExecuteNonQueryAsync();
    }
}