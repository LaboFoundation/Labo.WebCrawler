namespace Labo.WebCrawler.Core.Protocol.Providers
{
    using System;
    using System.Net;

    using Labo.WebCrawler.Core.Content;
    using Labo.WebCrawler.Core.Protocol;

    public sealed class HttpProtocolProvider : BaseProtocolProvider
    {
        private readonly IWebRequestManager m_WebRequestManager;

        public HttpProtocolProvider(IWebRequestManager webRequestManager)
        {
            if (webRequestManager == null)
            {
                throw new ArgumentNullException("webRequestManager");
            }

            m_WebRequestManager = webRequestManager;
        }

        protected override WebContentInfo GetWebContentInfoInternal(Uri uri)
        {
            WebRequest webRequest = m_WebRequestManager.GetWebRequest(uri);
            webRequest.Method = "HEAD";

            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            return GetWebContentInfoInternal(uri, httpWebResponse);
        }

        protected override WebContentInfo GetWebContentInfoInternal(Uri uri, WebResponse webResponse)
        {
            HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
            if (httpWebResponse == null)
            {
                throw new InvalidOperationException("webResponse must be typeof HttpWebResponse");
            }

            DateTime lastModified = httpWebResponse.LastModified;
            string mimeType = GetMimeType(httpWebResponse.ContentType);
            long contentLength = httpWebResponse.ContentLength;
            bool acceptRanges = string.Compare(httpWebResponse.Headers["Accept-Ranges"], "bytes", StringComparison.OrdinalIgnoreCase) == 0;
            HttpStatusCode httpStatusCode = httpWebResponse.StatusCode; // TODO: Store Http status code in extended properties
            return new WebContentInfo(uri, mimeType, contentLength, acceptRanges, lastModified);
        }

        protected override WebResponse GetWebResponse(Uri uri)
        {
            HttpWebRequest request = (HttpWebRequest)m_WebRequestManager.GetWebRequest(uri);
            return request.GetResponse();
        }
    }
}