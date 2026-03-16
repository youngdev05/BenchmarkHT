```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.6199/23H2/2023Update/SunValley3)
AMD Ryzen 5 3550H with Radeon Vega Mobile Gfx, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.204
  [Host]     : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2
  Job-KKTTMB : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
| Method           | Mean        | Error      | StdDev    | Median      | Allocated |
|----------------- |------------:|-----------:|----------:|------------:|----------:|
| &#39;EF Core Read&#39;   |    53.48 μs |   7.238 μs |  21.23 μs |    45.80 μs |     920 B |
| &#39;ADO.NET Read&#39;   |   253.02 μs |  23.626 μs |  68.54 μs |   232.05 μs |    2968 B |
| &#39;Dapper Read&#39;    |   299.12 μs |  28.991 μs |  83.65 μs |   292.60 μs |    2864 B |
| &#39;ADO.NET Update&#39; | 1,282.69 μs |  68.764 μs | 193.95 μs | 1,259.90 μs |    2376 B |
| &#39;ADO.NET Delete&#39; | 1,334.50 μs |  92.070 μs | 271.47 μs | 1,251.70 μs |    1752 B |
| &#39;ADO.NET Create&#39; | 1,372.68 μs |  73.370 μs | 210.51 μs | 1,346.60 μs |    2784 B |
| &#39;Dapper Delete&#39;  | 1,386.68 μs |  88.804 μs | 261.84 μs | 1,327.45 μs |    1904 B |
| &#39;Dapper Update&#39;  | 1,386.71 μs |  89.079 μs | 257.01 μs | 1,331.40 μs |    3368 B |
| &#39;Dapper Create&#39;  | 1,546.02 μs |  84.076 μs | 242.58 μs | 1,486.35 μs |    4480 B |
| &#39;EF Core Delete&#39; | 1,806.14 μs |  93.079 μs | 267.06 μs | 1,738.30 μs |   12336 B |
| &#39;EF Core Update&#39; | 2,230.67 μs | 181.948 μs | 527.87 μs | 2,106.80 μs |   46016 B |
| &#39;EF Core Create&#39; | 2,640.26 μs | 197.785 μs | 576.95 μs | 2,534.40 μs |   79152 B |
