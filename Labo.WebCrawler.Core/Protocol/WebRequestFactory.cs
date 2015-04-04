namespace Labo.WebCrawler.Core.Protocol
{
    using System;
    using System.Net;

    using Labo.WebCrawler.Core.Configuration;

    public sealed class WebRequestFactory : IWebRequestFactory
    {
        private readonly ICrawlConfiguration m_Config;

        public WebRequestFactory(ICrawlConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            m_Config = config;
        }

        public WebRequest CreateRequest(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = m_Config.IsHttpRequestAutoRedirectsEnabled;
            request.UserAgent = m_Config.UserAgent;
            request.Accept = "*/*";

            if (m_Config.HttpRequestMaxAutoRedirects > 0)
            {
                request.MaximumAutomaticRedirections = m_Config.HttpRequestMaxAutoRedirects;
            }

            if (m_Config.IsHttpRequestAutomaticDecompressionEnabled)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            if (m_Config.HttpRequestTimeoutInSeconds > 0)
            {
                request.Timeout = m_Config.HttpRequestTimeoutInSeconds * 1000;
            }

            return request;
        }
    }
}