# Contemporary
![alt text](paul-engel-bx4JS1lGB0U-unsplash.jpg "Mahamudra Contemporary")
[Photo by Paul Engel on Unsplash]

A simple package that implements a non opinionated Task Parallel Library using also the Railway Oriented Programming, a powerful **Functional Programming** pattern.

## Definition

```c#
    public ConcurrentDictionary<Result<T, string>, M> Executev2<T, M>(IEnumerable<T> list, Func<T, Task<M>> f)
    {
        ConcurrentDictionary<Result<T, string>, M> dic = new ConcurrentDictionary<Result<T, string>, M>();

        Parallel.ForEach(list, (item) =>
        {
            try
            {
                var response = Task.Run(async () => await f(item)).Result;
                dic.TryAdd(new Success<T, string>(item), response);
                if (_doLogging)
                    _logger.LogInformation($"Processing { item.ToString()} on thread {Thread.CurrentThread.ManagedThreadId}");
            }
            catch (Exception ex)
            {
                if (_doLogging)
                    _logger.LogError($"Processing { item.ToString()} with error: {ex.ToString()}");
                dic.TryAdd(new Failure<T, string>(ex.ToString()), default(M));
            }
        });
        return dic;
    }
```

## Test 

```c#
    [TestMethod]
    public void Executev2_ShouldComputeFirstPrimes_True()
    {
        var list = _p.Executev2<BaseEntity<int>, int>(_primes, SumWithException);
    
        var successList = list.Where(x => x.Key is Success<BaseEntity<int>, string>);
        Assert.AreEqual(successList.Sum(x => x.Value), 2 + 5 +  17);
     
        var failureList = list.Where(x => x.Key is Failure<BaseEntity<int>, string>);
        Assert.AreEqual(failureList.Count(), 1);
    }
```
  