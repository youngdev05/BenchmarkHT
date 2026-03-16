using System;
using BenchmarkDotNet.Running;
using OrmBenchmark.Benchmarks;
using OrmBenchmark.Data;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("🔧 Инициализация базы данных...");

using (var context = new AppDbContext(DbHelper.ConnectionString))
{
    context.Database.EnsureCreated();
    Console.WriteLine("База данных SQLite создана!");
}

Console.WriteLine("Запуск бенчмарков...");
Console.WriteLine("Это займет 2-5 минут...\n");

BenchmarkRunner.Run<CrudBenchmarks>();

Console.WriteLine("\nБенчмарки завершены!");
Console.WriteLine("Результаты в папке: BenchmarkDotNet.Artifacts/results/");