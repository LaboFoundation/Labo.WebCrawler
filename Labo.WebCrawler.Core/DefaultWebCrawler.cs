// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebCrawler.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 Bora Akgun
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the WebCrawler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.WebCrawler.Core
{
    using System;
    using System.Net;

    using Labo.WebCrawler.Core.Configuration;
    using Labo.WebCrawler.Core.Content;
    using Labo.WebCrawler.Core.Filters;
    using Labo.WebCrawler.Core.Modules;
    using Labo.WebCrawler.Core.Normalizer;
    using Labo.WebCrawler.Core.Task;

    public sealed class DefaultWebCrawler : IWebCrawler
    {
        private readonly ICrawlConfiguration m_Configuration;

        private readonly IUriFrontier m_UriFrontier;
        private readonly ICrawledUriHistoryRepository m_CrawledUriHistoryRepository;
        private readonly IWebContentRetriever m_WebContentRetriever;
        private readonly IWebContentProcessorModuleFactory m_WebContentProcessorModuleFactory;
        private readonly IUriNormalizer m_UriNormalizer;
        private readonly IUriCrawlFilter[] m_UriCrawlFilters;

        private readonly IWebContentCrawlFilter[] m_ContentCrawlFilters;

        private IUriProcessorTaskManager m_UriProcessorTaskManager;

        private bool m_Running;

        public DefaultWebCrawler(
            ICrawlConfiguration configuration,
            IUriFrontier uriFrontier, 
            ICrawledUriHistoryRepository crawledUriHistoryRepository,
            IWebContentRetriever webContentRetriever,
            IWebContentProcessorModuleFactory webContentProcessorModuleFactory,
            IUriNormalizer uriNormalizer,
            IUriCrawlFilter[] uriCrawlFilters,
            IWebContentCrawlFilter[] contentCrawlFilters)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            m_Configuration = configuration;
            m_UriFrontier = uriFrontier;
            m_CrawledUriHistoryRepository = crawledUriHistoryRepository;
            m_WebContentRetriever = webContentRetriever;
            m_WebContentProcessorModuleFactory = webContentProcessorModuleFactory;
            m_UriNormalizer = uriNormalizer;
            m_UriCrawlFilters = uriCrawlFilters ?? new IUriCrawlFilter[0];
            m_ContentCrawlFilters = contentCrawlFilters ?? new IWebContentCrawlFilter[0];

            if (m_Configuration.HttpServicePointConnectionLimit > 0)
            {
                ServicePointManager.DefaultConnectionLimit = m_Configuration.HttpServicePointConnectionLimit;
            }
        }

        public void Crawl(Uri seedUri)
        {
            // TODO: Exception management
            // TODO: Publish events
            // TODO: Use sockets
            // TODO: Logging
            // TODO: Robots.txt url filter
            // TODO: Implement politeness policy
            // TODO: Test max depth and max urls

            if (seedUri == null)
            {
                throw new ArgumentNullException("seedUri");
            }

            string baseUrl = seedUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
            Uri baseUri = new Uri(baseUrl);

            m_UriProcessorTaskManager = new MultiThreadedUriProcessorTaskManager(m_Configuration.ThreadWorkerCount);

            UriFrontierEntry nextUriEntry = new UriFrontierEntry(seedUri, 0);

            m_UriProcessorTaskManager.Start();
            m_Running = true;

            do
            {
                if (!m_Running)
                {
                    break;
                }

                if (nextUriEntry.Depth < m_Configuration.MaxCrawlDepth && 
                    !m_CrawledUriHistoryRepository.IsUriCrawled(nextUriEntry.Uri))
                {
                    m_UriProcessorTaskManager.EnqueueDownloader(
                        new UriProcessorTask(
                            m_Configuration,
                            nextUriEntry,
                            baseUri,
                            m_UriFrontier,
                            m_CrawledUriHistoryRepository,
                            m_WebContentRetriever,
                            m_WebContentProcessorModuleFactory,
                            m_UriNormalizer,
                            m_UriCrawlFilters,
                            m_ContentCrawlFilters));
                }

                // Try to retrieve next uri while task queue is not empty or the task manager
                // is processing the tasks.
                do
                {
                    nextUriEntry = m_UriFrontier.GetNextUri();                    
                }
                while (nextUriEntry == null && CheckMaxUriCount() && (m_UriProcessorTaskManager.GetQueueLength() > 0 || m_UriProcessorTaskManager.IsWorking()));
            }
            while (nextUriEntry != null && CheckMaxUriCount());

            if (m_Running)
            {
                m_UriProcessorTaskManager.Finish(true);                
            }
        }

        public void Stop()
        {
            m_Running = false;

            if (m_UriProcessorTaskManager != null)
            {
                m_UriProcessorTaskManager.Finish(false);
            }
        }

        private bool CheckMaxUriCount()
        {
            return m_CrawledUriHistoryRepository.GetCrawledUriCount() < m_Configuration.MaxPagesToCrawl;
        }
    }
}