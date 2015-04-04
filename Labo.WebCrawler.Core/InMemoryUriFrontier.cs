namespace Labo.WebCrawler.Core
{
    using System.Collections.Concurrent;

    public sealed class InMemoryUriFrontier : IUriFrontier
    {
        private readonly ConcurrentQueue<UriFrontierEntry> m_Queue; 

        public InMemoryUriFrontier()
        {
            m_Queue = new ConcurrentQueue<UriFrontierEntry>();
        }

        public void AddUri(UriFrontierEntry entry)
        {
            m_Queue.Enqueue(entry);
        }

        public UriFrontierEntry GetNextUri()
        {
            UriFrontierEntry entry;
            m_Queue.TryDequeue(out entry);
            return entry;
        }
    }
}