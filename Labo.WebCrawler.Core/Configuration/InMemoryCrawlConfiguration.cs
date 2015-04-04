namespace Labo.WebCrawler.Core.Configuration
{
    using System;
    using System.Globalization;

    public sealed class InMemoryCrawlConfiguration : ICrawlConfiguration
    {
        private readonly IWebCrawlerVersionProvider m_CrawlerVersionProvider;

        private int m_ThreadWorkerCount;
        public int ThreadWorkerCount
        {
            get
            {
                return m_ThreadWorkerCount;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value", "ThreadWorkerCount must be greater than 0");
                }

                m_ThreadWorkerCount = value;
            }
        }

        private string m_UserAgent;
        public string UserAgent
        {
            get
            {
                return m_UserAgent;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("UserAgent cannot be null or empty", "value");
                }

                m_UserAgent = FormatUserAgent(value);
            }
        }

        private string FormatUserAgent(string userAgent)
        {
            return userAgent.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) > -1 &&
                   userAgent.IndexOf("{{0}}", StringComparison.OrdinalIgnoreCase) == -1
                                    ? string.Format(CultureInfo.InvariantCulture, userAgent, m_CrawlerVersionProvider.GetVersion())
                                    : userAgent;
        }

        private int m_HttpServicePointConnectionLimit;
        public int HttpServicePointConnectionLimit
        {
            get
            {
                return m_HttpServicePointConnectionLimit;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value", "HttpServicePointConnectionLimit must be greater than 0");                    
                }

                m_HttpServicePointConnectionLimit = value;
            }
        }

        public bool IsHttpRequestAutoRedirectsEnabled { get; set; }

        private int m_HttpRequestMaxAutoRedirects;
        public int HttpRequestMaxAutoRedirects
        {
            get
            {
                return m_HttpRequestMaxAutoRedirects;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value", "HttpRequestMaxAutoRedirects must be greater than 0");
                }

                m_HttpRequestMaxAutoRedirects = value;
            }
        }

        public bool IsHttpRequestAutomaticDecompressionEnabled { get; set; }

        private int m_HttpRequestTimeoutInSeconds;
        public int HttpRequestTimeoutInSeconds
        {
            get
            {
                return m_HttpRequestTimeoutInSeconds;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "HttpRequestTimeoutInSeconds cannot be lower than 0");
                }

                m_HttpRequestTimeoutInSeconds = value;
            }
        }

        private int m_MaxCrawlDepth;
        public int MaxCrawlDepth
        {
            get
            {
                return m_MaxCrawlDepth;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "MaxCrawlDepth cannot be lower than 0");
                }

                m_MaxCrawlDepth = value;
            }
        }

        private int m_MaxPagesToCrawl;
        public int MaxPagesToCrawl
        {
            get
            {
                return m_MaxPagesToCrawl;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "MaxPagesToCrawl cannot be lower than 0");
                }

                m_MaxPagesToCrawl = value;
            }
        }

        public bool DoNotStorePageContent { get; set; }

        public bool IgnoreNoFollowAttributeForLinks { get; set; }

        public bool CrawlExternalUrls { get; set; }

        public bool DontStorePageContent { get; set; }

        public InMemoryCrawlConfiguration(IWebCrawlerVersionProvider crawlerVersionProvider)
        {
            m_CrawlerVersionProvider = crawlerVersionProvider;

            IsHttpRequestAutoRedirectsEnabled = true;
            IsHttpRequestAutomaticDecompressionEnabled = true;
            HttpRequestTimeoutInSeconds = 20;
            HttpRequestMaxAutoRedirects = 15;
            HttpServicePointConnectionLimit = 100;
            MaxCrawlDepth = 10;
            MaxPagesToCrawl = 1000;
            ThreadWorkerCount = 2;
            UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Labo Web Crawler v{0})";
        }
    }
}