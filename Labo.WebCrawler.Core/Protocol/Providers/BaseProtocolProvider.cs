namespace Labo.WebCrawler.Core.Protocol.Providers
{
    using System;
    using System.Net;

    using Labo.WebCrawler.Core.Content;

    public abstract class BaseProtocolProvider : INetworkProtocolProvider
    {
        public WebContentData GetWebContentData(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            WebContentData webContentData = null;
            WebException webException = null;
            Exception exception = null;

            WebResponse response = null;
            
            try
            {
                response = GetWebResponse(uri);
            }
            catch (WebException e)
            {
                webException = e;

                if (e.Response != null)
                {
                    response = e.Response;
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                if (response != null)
                {
                    webContentData = WebContentDataHelper.GetWebContentData(response);

                    response.Close();
                    response.Dispose();
                }
            }

            if (webContentData == null)
            {
                webContentData = new WebContentData(null, null, null, null);
            }

            webContentData.Exception = exception;
            webContentData.WebException = webException;

            return webContentData;
        }

        public WebContentInfo GetWebContentInfo(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            WebContentInfo webContentInfo = null;
            WebException webException = null;
            Exception exception = null;

            WebResponse response = null;

            try
            {
                webContentInfo = GetWebContentInfoInternal(uri);
            }
            catch (WebException e)
            {
                webException = e;

                if (e.Response != null)
                {
                    response = e.Response;
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                if (response != null)
                {
                    webContentInfo = GetWebContentInfoInternal(uri, response);

                    response.Close();
                    response.Dispose();
                }
            }

            if (webContentInfo == null)
            {
                webContentInfo = new WebContentInfo(null, null, -1, false, default(DateTime));
            }

            webContentInfo.Exception = exception;
            webContentInfo.WebException = webException;

            return webContentInfo;
        }

        protected static string GetMimeType(string contentType)
        {
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                return contentType.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            }

            return contentType;
        }

        protected abstract WebResponse GetWebResponse(Uri uri);

        protected abstract WebContentInfo GetWebContentInfoInternal(Uri uri);

        protected abstract WebContentInfo GetWebContentInfoInternal(Uri uri, WebResponse webResponse);     
    }
}