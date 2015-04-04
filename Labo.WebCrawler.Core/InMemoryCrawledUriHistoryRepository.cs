namespace Labo.WebCrawler.Core
{
    using System;
    using System.Collections.Generic;

    public class InMemoryCrawledUriHistoryRepository : ICrawledUriHistoryRepository
    {
        private readonly SortedSet<string> m_Urls;

        public InMemoryCrawledUriHistoryRepository()
        {
            m_Urls = new SortedSet<string>();
        }

        public bool IsUriCrawled(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            lock (m_Urls)
            {
                return m_Urls.Contains(uri.ToString());                
            }
        }

        public void AddUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            lock (m_Urls)
            {
                m_Urls.Add(uri.ToString());                
            }
        }

        public int GetCrawledUriCount()
        {
            lock (m_Urls)
            {
                return m_Urls.Count;
            }
        }
    }
}