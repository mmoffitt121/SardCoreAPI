

using Microsoft.AspNetCore.JsonPatch.Internal;
using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Services.Tasks
{
    public class TaskService : IHostedService
    {
        ILogger<TaskService> _logger;
        IServiceScopeFactory _scopeFactory;
        public TaskService(ILogger<TaskService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _scopeFactory = serviceScopeFactory;
        }

        private Queue<SardTask> taskQueue = new Queue<SardTask>();
        private SardTask? currentTask;
        private CancellationTokenSource? cancellationSource;

        private List<SardTask> failedTasks = new List<SardTask>();

        public void Schedule(SardTask task)
        {
            taskQueue.Enqueue(task);
        }

        public void Cancel(string id)
        {
            foreach (SardTask task in taskQueue)
            {
                if (task.Id.Equals(id))
                {
                    task.Cancelled = true;
                    task.SetProgress(0, "Cancelled.");
                    if ((currentTask?.Id.Equals(id) ?? false) && cancellationSource != null)
                    {
                        cancellationSource.Cancel();
                    }
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (taskQueue.TryPeek(out currentTask))
                    {
                        if (currentTask.Cancelled == true)
                        {
                            taskQueue.Dequeue();
                            continue;
                        }
                        cancellationSource = new CancellationTokenSource();

                        try
                        {
                            await currentTask.Run(cancellationSource.Token, _scopeFactory);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString());
                        }
                        finally
                        {
                            taskQueue.Dequeue();
                            currentTask = null;
                            cancellationSource = null;
                        }
                    }

                    Task.Delay(5000, cancellationToken).Wait();
                }
            });
            
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            
        }

        public async Task<List<VisibleSardTask>> GetTasks(string worldLocation)
        {
            return taskQueue.Where(task => task.WorldLocation.Equals(worldLocation)).Select(task => new VisibleSardTask(task)).ToList();
        }
    }
}
