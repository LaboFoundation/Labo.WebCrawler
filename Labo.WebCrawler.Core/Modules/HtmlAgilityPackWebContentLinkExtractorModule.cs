namespace Labo.WebCrawler.Core.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using HtmlAgilityPack;

    using Labo.WebCrawler.Core.Configuration;
    using Labo.WebCrawler.Core.Content;
    using Labo.WebCrawler.Core.Normalizer;

    public sealed class HtmlAgilityPackWebContentLinkExtractorModule : IHtmlWebContentLinkExtractorModule
    {
        private readonly ICrawlConfiguration m_CrawlConfiguration;

        private readonly IUriNormalizer m_UriNormalizer;

        private Uri[] m_ExtractedUris;

        public HtmlAgilityPackWebContentLinkExtractorModule(ICrawlConfiguration crawlConfiguration, IUriNormalizer uriNormalizer)
        {
            m_CrawlConfiguration = crawlConfiguration;
            m_UriNormalizer = uriNormalizer;
        }

        public Uri[] ExtractedUris
        {
            get
            {
                return m_ExtractedUris ?? (m_ExtractedUris = new Uri[0]);
            }
        }

        public void ProcessWebContent(IWebContent webContent)
        {
            if (webContent == null)
            {
                throw new ArgumentNullException("webContent");
            }

            if (webContent.ContentInfo == null)
            {
                throw new NullReferenceException("webContent.ContentInfo cannot be null");                
            }

            if (webContent.ContentInfo.MimeType == null || !webContent.ContentInfo.MimeType.Equals("text/html", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("webContent.ContentInfo.MimeType must be 'text/html'");
            }

            Debug.Assert(webContent.ContentData != null, "webContent.ContentData != null");
            Debug.Assert(webContent.ContentData.Text != null, "webContent.ContentData.Text != null");
            m_ExtractedUris = GetUrls(webContent.ContentData.Text, webContent.BaseUri);
        }

        private Uri[] GetUrls(string htmlContent, Uri baseUri)
        {
            if (htmlContent == null)
            {
                throw new ArgumentNullException("htmlContent");
            }

            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri");
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            HtmlNodeCollection anchors = doc.DocumentNode.SelectNodes("//a");
            if (anchors != null)
            {
                HashSet<Uri> urls = new HashSet<Uri>();

                foreach (HtmlNode anchor in anchors)
                {
                    string rel = anchor.GetAttributeValue("rel", string.Empty);
                    if (!m_CrawlConfiguration.IgnoreNoFollowAttributeForLinks && string.Equals(rel, "nofollow", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    string url = anchor.GetAttributeValue("href", string.Empty);
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        continue;
                    }

                    Uri uri = m_UriNormalizer.NormalizeUrl(url, baseUri);
                    if (uri != null)
                    {
                        urls.Add(uri);
                    }
                }

                return urls.ToArray();
            }

            return new Uri[0];
        }
    }
}