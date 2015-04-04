namespace Labo.WebCrawler.Core.Configuration
{
    public interface ICrawlConfiguration
    {
        int ThreadWorkerCount { get; }

        string UserAgent { get; }

        int HttpServicePointConnectionLimit { get; }

        bool IsHttpRequestAutoRedirectsEnabled { get; }

        int HttpRequestMaxAutoRedirects { get; }

        bool IsHttpRequestAutomaticDecompressionEnabled { get; }

        int HttpRequestTimeoutInSeconds { get; }

        int MaxCrawlDepth { get; }

        int MaxPagesToCrawl { get; }

        bool DoNotStorePageContent { get; }

        bool IgnoreNoFollowAttributeForLinks { get; }

        bool CrawlExternalUrls { get; }
    }
}