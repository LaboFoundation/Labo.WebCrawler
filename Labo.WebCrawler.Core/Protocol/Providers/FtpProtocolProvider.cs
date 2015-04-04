namespace Labo.WebCrawler.Core.Protocol.Providers
{
    using System;
    using System.Net;

    using Labo.WebCrawler.Core.Content;
    using Labo.WebCrawler.Core.Protocol;

    public sealed class FtpProtocolProvider : BaseProtocolProvider
    {
        private readonly IWebRequestManager m_WebRequestManager;

        public FtpProtocolProvider(IWebRequestManager webRequestManager)
        {
            m_WebRequestManager = webRequestManager;
        }

        protected override WebContentInfo GetWebContentInfoInternal(Uri uri)
        {
            long contentLength;
            string mimeType;
            DateTime lastModified;

            FtpWebRequest request = (FtpWebRequest)m_WebRequestManager.GetWebRequest(uri);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                contentLength = response.ContentLength;
                mimeType = GetMimeType(response.ContentType);
            }

            request = (FtpWebRequest)m_WebRequestManager.GetWebRequest(uri);
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                lastModified = response.LastModified;

                FtpStatusCode ftpStatusCode = response.StatusCode; // TODO: Store ftp status code in extended properties
            }

            return new WebContentInfo(uri, mimeType, contentLength, true, lastModified);
        }

        protected override WebContentInfo GetWebContentInfoInternal(Uri uri, WebResponse webResponse)
        {
            return null;
        }

        protected override WebResponse GetWebResponse(Uri uri)
        {
            FtpWebRequest request = (FtpWebRequest)m_WebRequestManager.GetWebRequest(uri);

            request.Method = WebRequestMethods.Ftp.DownloadFile;

            return request.GetResponse();
        }
    }
}
