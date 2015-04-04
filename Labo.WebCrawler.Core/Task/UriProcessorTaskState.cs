namespace Labo.WebCrawler.Core.Task
{
    public enum UriProcessorTaskState
    {
        Idle = 0,
        Working,
        Paused,
        Finished,
        Error
    }
}