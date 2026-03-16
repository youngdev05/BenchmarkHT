using Dapper;
using OrmBenchmark.Data;
using OrmBenchmark.Models;
using System.Data;
using System.Threading.Tasks;

namespace OrmBenchmark.Repositories;

public class DapperRepository
{
    private readonly IDbConnection _db;

    public DapperRepository()
    {
        _db = DbHelper.CreateConnection();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var sql = @"INSERT INTO Products (Name, Price, Category) 
                    VALUES (@Name, @Price, @Category); 
                    SELECT last_insert_rowid();";
        var id = await _db.ExecuteScalarAsync<int>(sql, product);
        product.Id = id;
        return product;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _db.QueryFirstOrDefaultAsync<Product>(
            "SELECT * FROM Products WHERE Id = @Id", 
            new { Id = id });
    }

    public async Task UpdateAsync(Product product)
    {
        await _db.ExecuteAsync(
            "UPDATE Products SET Name=@Name, Price=@Price, Category=@Category WHERE Id=@Id", 
            product);
    }

    public async Task DeleteAsync(int id)
    {
        await _db.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });
    }

    public async Task ClearAsync()
    {
        await _db.ExecuteAsync("DELETE FROM Products");
    }
}