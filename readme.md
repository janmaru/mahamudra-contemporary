# Contemporary
![alt text](paul-engel-bx4JS1lGB0U-unsplash.jpg "Mahamudra Contemporary")
[Photo by Paul Engel on Unsplash]

A simple package that implements a non opinionated Task Parallel Library using also the Railway Oriented Programming, a powerful **Functional Programming** pattern.

## Definition

```c#
        public async Task<ConcurrentDictionary<Result<T, string>, M>> ExecuteAsyncResult<T, M>(IEnumerable<T> list, Func<T, Task<M>> f)
        {
            ConcurrentDictionary<Result<T, string>, M> dic = new ConcurrentDictionary<Result<T, string>, M>();

            var tasks = list.Select(async item =>
            {
                try
                {
                    var response = await f(item);
                    dic.TryAdd(new Success<T, string>(item), response);
                    _logger.LogInformation(String.Format(LogOkTemplate, DateTime.UtcNow.ToMilliseconds(), item.ToString(), Thread.CurrentThread.ManagedThreadId));
                }
                catch (Exception ex)
                { 
                    dic.TryAdd(new Failure<T, string>(ex.ToString()), default(M));
                    _logger.LogError(String.Format(LogErrTemplate, DateTime.UtcNow.ToMilliseconds(), item.ToString(), Thread.CurrentThread.ManagedThreadId, ex.ToString()));
                } 
            });

            await Task.WhenAll(tasks);

            return dic;
        }
```

## Test 

```c#
        [TestMethod]
        public async Task ExecuteAsyncResult_ShouldComputeSumOfFirstPrimesLessEqualToTenWithError_True()
        {
            var list = await _p.ExecuteAsyncResult<BaseEntity<int>, int>(_primes, SumAsyncWithException);

            var successList = list.Where(x => x.Key is Success<BaseEntity<int>, string>);
            Assert.AreEqual(successList.Sum(x => x.Value), 2 + 5 + 17);
            var failureList = list.Where(x => x.Key is Failure<BaseEntity<int>, string>);
            Assert.AreEqual(failureList.Count(), 1);
        } 
```

## Benchmark

Parallel versus Async.


|                Method |     Mean |       Error |   StdDev |   Median | Ratio | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------- |---------:|------------:|---------:|---------:|------:|------:|------:|------:|----------:|
| ExecuteParallelResult | 731.6 ms | 15,086.4 ms | 826.9 ms | 334.6 ms |  1.00 |     - |     - |     - | 307.29 KB |

|             Method |     Mean |    Error |   StdDev | Ratio | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------- |---------:|---------:|---------:|------:|------:|------:|------:|----------:|
| ExecuteAsyncResult | 206.5 ms | 243.0 ms | 13.32 ms |  1.00 |     - |     - |     - | 270.69 KB |
  