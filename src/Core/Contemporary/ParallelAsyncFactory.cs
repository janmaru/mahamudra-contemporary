﻿using Mahamudra.Result.Core.Patterns;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary
{
    public class ParallelAsyncFactory
    {
        private readonly ILogger _logger;
        private readonly bool _doLogging = true;
        public ParallelAsyncFactory()
        {
            _doLogging = false;
        }
        public ParallelAsyncFactory(ILogger logger)
        {
            this._logger = logger;
        }

        /// <summary>Executes an asynchronous function for each given T. If an exception is raised then it's logged and the execution interrupted.</summary>
        /// <typeparam name="T">Object Type processed by for each async and f. Must implement ToString() in order to log the id.</typeparam>
        /// <typeparam name="M">Return Type of the function f.</typeparam>
        /// <param name="list">The list of object to process. Actually a cursor.</param>
        /// <param name="f">A function f, with T as input, sinse it gives back a Task<M> is supposed to be async.</param>
        /// <returns></returns>
        public async Task<ConcurrentDictionary<T, M>> ExecuteAsync<T, M>(IEnumerable<T> list, Func<T, Task<M>> f)
        {
            ConcurrentDictionary<T, M> dic = new ConcurrentDictionary<T, M>();

            var tasks = list.Select(async item =>
            {
                try
                {
                    var response = await f(item);
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

            await Task.WhenAll(tasks);

            return dic;
        }

        /// <summary>Executes an asynchronous function for each given T. If an exception is raised then it's added to the dictionary of results as failure. The execution of the action is not interrupted.</summary>
        /// <typeparam name="T">Object Type processed by Parallel.ForEach and f. Must implement ToString() in order to log the id.</typeparam>
        /// <typeparam name="M">Return Type of the function f.</typeparam>
        /// <param name="list">The list of object to process. Actually a cursor.</param>
        /// <param name="f">A function f, with T as input, sinse it gives back a Task<M> is supposed to be async.</param>
        /// <returns></returns>
        public async Task<ConcurrentDictionary<Result<T, string>, M>> ExecuteAsyncResult<T, M>(IEnumerable<T> list, Func<T, Task<M>> f)
        {
            ConcurrentDictionary<Result<T, string>, M> dic = new ConcurrentDictionary<Result<T, string>, M>();

            var tasks = list.Select(async item =>
            {
                try
                {
                    var response = await f(item);
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

            await Task.WhenAll(tasks);

            return dic;
        }
    }
}