namespace Labo.WebCrawler.Core.Protocol
{
    using System;
    using System.Net;

    public interface IWebRequestFactory
    {
        WebRequest CreateRequest(Uri uri);
    }
}
