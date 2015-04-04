namespace Labo.WebCrawler.Core.WebPage
{
    using System;
    using System.Net;

    [Serializable]
    public sealed class WebPageInfo
    {
        public int ID { get; set; }

        public PageContent Content { get; set; }

        public string ContentType { get; set; }

        public Uri Uri { get; set; }

        public bool IsRoot { get; set; }

        public bool IsExternal { get; set; }

        public WebException WebException { get; set; }

        public Exception Exception { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public DateTime CrawlDate { get; set; }

        public WebPageInfo(Uri uri)
        {
            Uri = uri;
        }
    }
}