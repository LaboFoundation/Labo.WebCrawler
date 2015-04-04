namespace Labo.WebCrawler.Core.Task
{
    internal interface IUriProcessorTaskManager
    {
        bool IsWorking();

        int GetQueueLength();

        void EnqueueDownloader(IUriProcessorTask task);

        void Start();

        void Finish(bool waitForAllWorkers);
    }
}