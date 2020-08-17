﻿using Mahamudra.Result.Core.Patterns;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary
{
    public class ParallelFactory
    {
        private readonly ILogger _logger;
        private readonly bool _doLogging = true;
        public ParallelFactory()
        {
            _doLogging = false;
        }
        public ParallelFactory(ILogger logger)
        {
            this._logger = logger;
        }

        /// <summary>Executes an asynchronous function for each given T. If an exception is raised then it's logged and the execution interrupted.</summary>
        /// <typeparam name="T">Object Type processed by Parallel.ForEach and f. Must implement ToString() in order to log the id.</typeparam>
        /// <typeparam name="M">Return Type of the function f.</typeparam>
        /// <param name="list">The list of object to process. Actually a cursor.</param>
        /// <param name="f">A function f, with T as input, sinse it gives back a Task<M> is supposed to be async.</param>
        /// <returns></returns>
        public ConcurrentDictionary<T, M> ExecuteAsync<T, M>(IEnumerable<T> list, Func<T, Task<M>> f)
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

        /// <summary>Executes an asynchronous function for each given T. If an exception is raised then it's added to the dictionary of results as failure. The execution of the action is not interrupted.</summary>
        /// <typeparam name="T">Object Type processed by Parallel.ForEach and f. Must implement ToString() in order to log the id.</typeparam>
        /// <typeparam name="M">Return Type of the function f.</typeparam>
        /// <param name="list">The list of object to process. Actually a cursor.</param>
        /// <param name="f">A function f, with T as input, sinse it gives back a Task<M> is supposed to be async.</param>
        /// <returns></returns>
        public ConcurrentDictionary<Result<T, string>, M> ExecuteAsyncResult<T, M>(IEnumerable<T> list, Func<T, Task<M>> f)
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


        /// <summary>Executes a synchronous function for each given T. If an exception is raised then it's added to the dictionary of results as failure. The execution of the action is not interrupted.</summary>
        /// <typeparam name="T">Object Type processed by Parallel.ForEach and f. Must implement ToString() in order to log the id.</typeparam>
        /// <typeparam name="M">Return Type of the function f.</typeparam>
        /// <param name="list">The list of object to process. Actually a cursor.</param>
        /// <param name="f">A function f, with T as input, and M as output.</param>
        /// <returns></returns>
        public ConcurrentDictionary<Result<T, string>, M> ExecuteResult<T, M>(IEnumerable<T> list, Func<T, M> f)
        {
            ConcurrentDictionary<Result<T, string>, M> dic = new ConcurrentDictionary<Result<T, string>, M>();

            Parallel.ForEach(list, (item) =>
            {
                try
                {
                    var response = f(item);
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

        /// <summary>Executes a synchronous function for each given T. If an exception is raised then it's logged and the execution interrupted.</summary>
        /// <typeparam name="T">Object Type processed by Parallel.ForEach and f. Must implement ToString() in order to log the id.</typeparam>
        /// <typeparam name="M">Return Type of the function f.</typeparam>
        /// <param name="list">The list of object to process. Actually a cursor.</param>
        /// <param name="f">A function f, with T as input, and M as output.</param>
        /// <returns></returns>
        public ConcurrentDictionary<T, M> Execute<T, M>(IEnumerable<T> list, Func<T, M> f)
        {
            ConcurrentDictionary<T, M> dic = new ConcurrentDictionary<T, M>();

            Parallel.ForEach(list, (item) =>
            {
                try
                {
                    var response =  f(item);
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
    }
}
