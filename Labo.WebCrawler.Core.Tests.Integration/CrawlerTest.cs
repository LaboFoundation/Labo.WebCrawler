namespace Labo.WebCrawler.Core.Tests.Integration
{
    using System;
    using System.Net;

    using Labo.WebCrawler.Core.Configuration;
    using Labo.WebCrawler.Core.Content;
    using Labo.WebCrawler.Core.Filters;
    using Labo.WebCrawler.Core.Modules;
    using Labo.WebCrawler.Core.Normalizer;
    using Labo.WebCrawler.Core.Protocol;
    using Labo.WebCrawler.Core.Protocol.Providers;

    using NUnit.Framework;

    [TestFixture]
    public sealed class CrawlerTest
    {
        [Test]
        public void A()
        {
            int htmlPagesCount = 0;
            int count404 = 0;
            InMemoryCrawlConfiguration crawlConfiguration = new InMemoryCrawlConfiguration(new DefaultWebCrawlerVersionProvider());
            DefaultUriNormalizer uriNormalizer = new DefaultUriNormalizer();
            DefaultWebContentProcessorModuleFactory contentProcessorModuleFactory = new DefaultWebContentProcessorModuleFactory();
            const string mimeType = "text/html";
            contentProcessorModuleFactory.RegisterContentProcessorModule(mimeType, new ActionWebContentProcessorModule(
                x =>
                    {
                        htmlPagesCount++;

                        if (x.ContentData.StatusCode == HttpStatusCode.NotFound)
                        {
                            count404++;
                        }
                    }));
            contentProcessorModuleFactory.RegisterContentProcessorModule(mimeType, new HtmlAgilityPackWebContentLinkExtractorModule(crawlConfiguration, uriNormalizer));

            NetworkProtocolProviderFactory networkProtocolProviderFactory = new NetworkProtocolProviderFactory(new UrlProtocolParser());
            WebRequestManager webRequestManager = new WebRequestManager(new WebRequestFactory(crawlConfiguration));
            networkProtocolProviderFactory.RegisterProvider("http", new HttpProtocolProvider(webRequestManager));
            networkProtocolProviderFactory.RegisterProvider("ftp", new FtpProtocolProvider(webRequestManager));

            InMemoryCrawledUriHistoryRepository inMemoryCrawledUriHistoryRepository = new InMemoryCrawledUriHistoryRepository();

            IWebCrawler webCrawler = new DefaultWebCrawler(
                crawlConfiguration,
                new InMemoryUriFrontier(),
                inMemoryCrawledUriHistoryRepository,
                new DefaultWebContentRetriever(networkProtocolProviderFactory), 
                contentProcessorModuleFactory, 
                uriNormalizer,
                new IUriCrawlFilter[0],
                new IWebContentCrawlFilter[0]);

            webCrawler.Crawl(new Uri("http://localhost:12439/"));
            htmlPagesCount.ToString();
        }
    }
}
