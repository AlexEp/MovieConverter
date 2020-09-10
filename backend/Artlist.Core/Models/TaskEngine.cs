using Artlist.Core.Models.ProcessTasks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Artlist.Core.Models
{
    public class TaskEngine: ITaskEngine
    {
        private BlockingCollection<IProcessExecutorRequest> blockingCollection = new BlockingCollection<IProcessExecutorRequest>(new ConcurrentQueue<IProcessExecutorRequest>(), 100);
        private CancellationTokenSource tokenSource;
        private IList<Task> ProcessTasks;

        private readonly ILogger<TaskEngine> _logger;
        private readonly int _maxTasks;
        private readonly object syncLock = new object();

        public TaskEngine(ILogger<TaskEngine> logger,int maxTasks)
        {
            _logger = logger;
            _maxTasks = maxTasks;

            ProcessTasks = new List<Task>();
        }


        public void Start()
        {
            lock (syncLock) {
                if (ProcessTasks.Count == 0)
                {
                    tokenSource = new CancellationTokenSource();
                    var cancellationToken = tokenSource.Token;


                    for (int i = 0; i < _maxTasks; i++)
                    {
                        Task producerTask = Task.Run(() => ExecutePtocedur(cancellationToken));
                        ProcessTasks.Add(producerTask);
                    }
                }
            }
        
        }

        private void ExecutePtocedur(CancellationToken cancellationToken)
        {
            // IsCompleted == (IsAddingCompleted && Count == 0)
            while (!cancellationToken.IsCancellationRequested)
            {
                IProcessExecutorRequest nextItem = null;
                try
                {
                    if (!blockingCollection.TryTake(out nextItem, 5000, cancellationToken))
                    {
                       // _logger.LogDebug("No items . {ManagedThreadId}", Thread.CurrentThread.ManagedThreadId);
                    }
                    else
                    {
                        _logger.LogInformation("Execute Task {task}", Thread.CurrentThread.ManagedThreadId);
                        nextItem.Execute();
                    }
                }

                catch (OperationCanceledException)
                {
                    _logger.LogError("Tasking canceled.");
                    break;
                }

               // Thread.Sleep(500);
            }
        }

        public void Stop()
        {
            lock (syncLock)
            {
                if (ProcessTasks.Count > 0)
                {
                    tokenSource.Cancel();
                    ProcessTasks.Clear() ;
                }
            }
        }

        public void AddTask(IProcessExecutorRequest processExecutor)
        {
            // Cancellation causes OCE. We know how to handle it.
            bool isSuccessed = false;
            try
            {
                // A shorter timeout causes more failures.

                isSuccessed = blockingCollection.TryAdd(processExecutor, 500);
            }
            catch (OperationCanceledException)
            {

                Console.WriteLine("Add loop canceled.");
                // Let other threads know we're done in case
                // they aren't monitoring the cancellation token.
                blockingCollection.CompleteAdding();
            }
            catch (Exception exp)
            {
                _logger.LogError("Error on addint task to task executor", exp, processExecutor);
            }
        }
    }

    public interface ITaskEngine {
        public void Start();
        public void Stop();
        public void AddTask(IProcessExecutorRequest processExecutor);
    }
}
