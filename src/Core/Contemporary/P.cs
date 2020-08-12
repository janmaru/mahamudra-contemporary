using Mahamudra.Result.Core.Patterns;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary
{
    public class P
    {
        private readonly ILogger _logger;
        private readonly bool _doLogging = true;
        public P()
        {
            _doLogging = false;
        }
        public P(ILogger logger)
        {
            this._logger = logger;
        }
 
        public ConcurrentDictionary<T, M> Execute<T, M>(IEnumerable<T> list, Func<T, Task<M>> f)
        {
            ConcurrentDictionary<T, M> dic = new ConcurrentDictionary<T, M>();

            Parallel.ForEach(list, (item) =>
            {
                try
                {
                    var response = Task.Run(async () => await f(item)).Result; 
                    dic.TryAdd(item, response);
                    if (_doLogging)
                        _logger.LogInformation($"Processing { item.ToString()} on thread {Thread.CurrentThread.ManagedThreadId}");
                }
                catch (Exception ex)
                {
                    if (_doLogging)
                        _logger.LogError($"Processing { item.ToString()} with error: {ex.ToString()}");
                    throw ex;
                }
            });
            return dic;
        }
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
    }
}
