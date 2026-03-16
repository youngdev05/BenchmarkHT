using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrmBenchmark.Data;
using OrmBenchmark.Models;
using OrmBenchmark.Repositories;

namespace OrmBenchmark.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RPlotExporter]
public class CrudBenchmarks
{
    private EfRepository _efRepo = null!;
    private DapperRepository _dapperRepo = null!;
    private AdoRepository _adoRepo = null!;
    private AppDbContext _context = null!;
    private int _createdId;

    [GlobalSetup]
    public async Task Setup()
    {
        _context = new AppDbContext(DbHelper.ConnectionString);
    
        _context.Database.EnsureCreated();
    
        _efRepo = new EfRepository(_context);
        _dapperRepo = new DapperRepository();
        _adoRepo = new AdoRepository();

        await _efRepo.ClearAsync();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _efRepo.ClearAsync();
        _context.Dispose();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        var p = new Product { Name = "Temp", Price = 1, Category = "T" };
        var res = _efRepo.CreateAsync(p).GetAwaiter().GetResult();
        _createdId = res.Id;
    }

    [Benchmark(Description = "EF Core Create")]
    public async Task EfCreate()
    {
        var p = new Product { Name = "EF", Price = 1, Category = "A" };
        await _efRepo.CreateAsync(p);
    }

    [Benchmark(Description = "Dapper Create")]
    public async Task DapperCreate()
    {
        var p = new Product { Name = "Dap", Price = 1, Category = "A" };
        await _dapperRepo.CreateAsync(p);
    }

    [Benchmark(Description = "ADO.NET Create")]
    public async Task AdoCreate()
    {
        var p = new Product { Name = "Ado", Price = 1, Category = "A" };
        await _adoRepo.CreateAsync(p);
    }

    [Benchmark(Description = "EF Core Read")]
    public async Task<Product?> EfRead()
    {
        return await _efRepo.GetByIdAsync(_createdId);
    }

    [Benchmark(Description = "Dapper Read")]
    public async Task<Product?> DapperRead()
    {
        return await _dapperRepo.GetByIdAsync(_createdId);
    }

    [Benchmark(Description = "ADO.NET Read")]
    public async Task<Product?> AdoRead()
    {
        return await _adoRepo.GetByIdAsync(_createdId);
    }

    [Benchmark(Description = "EF Core Update")]
    public async Task EfUpdate()
    {
        var p = new Product { Id = _createdId, Name = "Updated", Price = 200, Category = "B" };
        await _efRepo.UpdateAsync(p);
    }

    [Benchmark(Description = "Dapper Update")]
    public async Task DapperUpdate()
    {
        var p = new Product { Id = _createdId, Name = "Updated", Price = 200, Category = "B" };
        await _dapperRepo.UpdateAsync(p);
    }

    [Benchmark(Description = "ADO.NET Update")]
    public async Task AdoUpdate()
    {
        var p = new Product { Id = _createdId, Name = "Updated", Price = 200, Category = "B" };
        await _adoRepo.UpdateAsync(p);
    }

    [Benchmark(Description = "EF Core Delete")]
    public async Task EfDelete()
    {
        await _efRepo.DeleteAsync(_createdId);
    }

    [Benchmark(Description = "Dapper Delete")]
    public async Task DapperDelete()
    {
        await _dapperRepo.DeleteAsync(_createdId);
    }

    [Benchmark(Description = "ADO.NET Delete")]
    public async Task AdoDelete()
    {
        await _adoRepo.DeleteAsync(_createdId);
    }
}