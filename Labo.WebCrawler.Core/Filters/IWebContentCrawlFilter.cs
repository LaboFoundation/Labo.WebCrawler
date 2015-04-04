namespace Labo.WebCrawler.Core.Filters
{
    using Labo.WebCrawler.Core.Content;

    public interface IWebContentCrawlFilter
    {
        bool MustCrawlContent(WebContentInfo contentInfo);
    }
}