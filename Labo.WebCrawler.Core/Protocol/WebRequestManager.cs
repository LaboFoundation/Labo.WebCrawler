namespace Labo.WebCrawler.Core.Protocol
{
    using System;
    using System.Net;

    public sealed class WebRequestManager : IWebRequestManager
    {
        private readonly IWebRequestFactory m_WebRequestFactory;

        public WebRequestManager(IWebRequestFactory webRequestFactory)
        {
            m_WebRequestFactory = webRequestFactory;
        }

        public WebRequest GetWebRequest(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            WebRequest webRequest = m_WebRequestFactory.CreateRequest(uri);

            return webRequest;
        }
    }
}