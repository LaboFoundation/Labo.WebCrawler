namespace Labo.WebCrawler.Core.Task
{
    using System;

    public interface IUriProcessorTask
    {
        void Process();

        void SetError(Exception ex);

        void SetState(UriProcessorTaskState state);
    }
}