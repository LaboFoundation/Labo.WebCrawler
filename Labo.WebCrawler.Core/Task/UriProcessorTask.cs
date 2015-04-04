namespace Labo.WebCrawler.Core.Task
{
    using System;

    using Labo.WebCrawler.Core.Configuration;
    using Labo.WebCrawler.Core.Content;
    using Labo.WebCrawler.Core.Filters;
    using Labo.WebCrawler.Core.Modules;
    using Labo.WebCrawler.Core.Normalizer;

    public sealed class UriProcessorTask : IUriProcessorTask
    {
        private readonly ICrawlConfiguration m_CrawlConfiguration;

        private readonly UriFrontierEntry m_UriEntry;

        private readonly Uri m_BaseUri;

        private readonly IUriFrontier m_UriFrontier;
        private readonly ICrawledUriHistoryRepository m_CrawledUriHistoryRepository;
        private readonly IWebContentRetriever m_WebContentInfoRetriever;
        private readonly IWebContentProcessorModuleFactory m_WebContentProcessorModuleFactory;
        private readonly IUriNormalizer m_UriNormalizer;
        private readonly IUriCrawlFilter[] m_UriCrawlFilters;

        private readonly IWebContentCrawlFilter[] m_ContentCrawlFilters;

        public UriProcessorTask(
            ICrawlConfiguration crawlConfiguration,
            UriFrontierEntry uriEntry,
            Uri baseUri,
            IUriFrontier uriFrontier, 
            ICrawledUriHistoryRepository crawledUriHistoryRepository, 
            IWebContentRetriever webContentRetriever, 
            IWebContentProcessorModuleFactory webContentProcessorModuleFactory, 
            IUriNormalizer uriNormalizer,
            IUriCrawlFilter[] uriCrawlFilters,
            IWebContentCrawlFilter[] contentCrawlFilters)
        {
            m_CrawlConfiguration = crawlConfiguration;
            m_UriEntry = uriEntry;
            m_BaseUri = baseUri;
            m_UriFrontier = uriFrontier;
            m_CrawledUriHistoryRepository = crawledUriHistoryRepository;
            m_WebContentInfoRetriever = webContentRetriever;
            m_WebContentProcessorModuleFactory = webContentProcessorModuleFactory;
            m_UriNormalizer = uriNormalizer;
            m_UriCrawlFilters = uriCrawlFilters ?? new IUriCrawlFilter[0];
            m_ContentCrawlFilters = contentCrawlFilters ?? new IWebContentCrawlFilter[0];
        }

        public void SetError(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void SetState(UriProcessorTaskState state)
        {
            throw new NotImplementedException();
        }

        public void Process()
        {
            // TODO: Exception handling
            Uri uri = m_UriEntry.Uri;
            if (m_CrawledUriHistoryRepository.IsUriCrawled(uri))
            {
                return;
            }

            bool isExternal = !m_BaseUri.IsBaseOf(uri);

            if (isExternal && !m_CrawlConfiguration.CrawlExternalUrls)
            {
                return;
            }

            WebContentInfo webContentInfo = m_WebContentInfoRetriever.RetrieveWebContentInfo(uri);
            webContentInfo.IsExternal = isExternal;
            webContentInfo.IsRoot = m_BaseUri == uri;

            // TODO: check webcontentinfo errors

            if (SkipCrawlContent(webContentInfo))
            {
                return;
            }

            WebContentData webContentData = m_WebContentInfoRetriever.RetrieveWebContentData(uri);
            if (webContentData.HasError)
            {
                // TODO: Log error
                // TODO: Retry operation
                return;
            }

            IWebContentProcessorModule[] processorModules = m_WebContentProcessorModuleFactory.GetContentProcessorModules(webContentInfo.MimeType);
            IWebContent webContent = new DefaultWebContent(m_BaseUri, webContentInfo, webContentData);

            for (int i = 0; i < processorModules.Length; i++)
            {
                IWebContentProcessorModule webContentProcessorModule = processorModules[i];

                IHtmlWebContentLinkExtractorModule htmlWebContentLinkExtractorModule = webContentProcessorModule as IHtmlWebContentLinkExtractorModule;
                if (htmlWebContentLinkExtractorModule != null)
                {
                    ProcessWebContentLinks(htmlWebContentLinkExtractorModule, webContent);
                }
                else
                {
                    webContentProcessorModule.ProcessWebContent(webContent);
                }
            }

            m_CrawledUriHistoryRepository.AddUri(uri);
        }

        private bool SkipCrawlContent(WebContentInfo webContentInfo)
        {
            for (int i = 0; i < m_ContentCrawlFilters.Length; i++)
            {
                IWebContentCrawlFilter webContentCrawlFilter = m_ContentCrawlFilters[i];
                if (!webContentCrawlFilter.MustCrawlContent(webContentInfo))
                {
                    // TODO: Log
                    return true;
                }
            }

            return false;
        }

        private void ProcessWebContentLinks(IHtmlWebContentLinkExtractorModule htmlWebContentLinkExtractorModule, IWebContent webContent)
        {
            htmlWebContentLinkExtractorModule.ProcessWebContent(webContent);                        

            Uri[] extractedUris = htmlWebContentLinkExtractorModule.ExtractedUris;
            if (extractedUris != null)
            {
                for (int i = 0; i < extractedUris.Length; i++)
                {
                    Uri extractedUri = extractedUris[i];
                    Uri normalizedUri = m_UriNormalizer.NormalizeUri(extractedUri, m_BaseUri);

                    if (normalizedUri == null)
                    {
                        continue;
                    }

                    bool mustCrawlUri = MustCrawlUri(normalizedUri);

                    if (mustCrawlUri && normalizedUri != m_UriEntry.Uri)
                    {
                        if (!m_CrawledUriHistoryRepository.IsUriCrawled(normalizedUri))
                        {
                            m_UriFrontier.AddUri(new UriFrontierEntry(normalizedUri, m_UriEntry.Depth + 1));
                        }
                    }
                }
            }
        }

        private bool MustCrawlUri(Uri uri)
        {
            bool mustCrawlUri = true;
            if (m_UriCrawlFilters != null)
            {
                for (int j = 0; j < m_UriCrawlFilters.Length; j++)
                {
                    // Uri filter uri'yi değiştirecek birşey de olabilir, araştırmak lazım.
                    IUriCrawlFilter uriCrawlFilter = m_UriCrawlFilters[j];
                    if (!uriCrawlFilter.MustCrawlUri(uri))
                    {
                        mustCrawlUri = false;
                        // TODO: Log
                        break;
                    }
                }
            }

            return mustCrawlUri;
        }
    }
}