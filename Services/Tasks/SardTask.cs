namespace SardCoreAPI.Services.Tasks
{
    public abstract class SardTask
    {
        public string Name { get; }
        public string Id { get; }
        public string WorldLocation { get; }
        public int Progress { get; set; }
        public string ProgressMessage { get; set; }
        public DateTime Submitted { get; set; }
        public DateTime CompletionDate { get; set; }
        public bool Cancelled { get; set; }
        public abstract Task Run(CancellationToken cancellationToken, IServiceScopeFactory serviceScopeFactory);
        public SardTask(string name, string worldLocation)
        {
            Id = Guid.NewGuid().ToString() + "_" + DateTime.Now;
            Name = name;
            Progress = 0;
            WorldLocation = worldLocation;
            Submitted = DateTime.Now;
            ProgressMessage = "Queued.";
        }
        public SardTask(SardTask task)
        {
            Name = task.Name;
            Id = task.Id;
            WorldLocation = task.WorldLocation;
            Progress = task.Progress;
            CompletionDate = task.CompletionDate;
            Cancelled = task.Cancelled;
            Submitted = task.Submitted;
            ProgressMessage = task.ProgressMessage;
        }

        public void SetProgress(int progress, string message)
        {
            ProgressMessage = message;
            Progress = progress;
        }
    }

    public class VisibleSardTask : SardTask
    {
        public VisibleSardTask(SardTask task) : base(task)
        {

        }

        public override async Task Run(CancellationToken cancellationToken, IServiceScopeFactory serviceScopeFactory)
        {
            // Do nothing
        }
    }
}
