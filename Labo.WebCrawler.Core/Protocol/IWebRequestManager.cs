namespace Labo.WebCrawler.Core.Protocol
{
    using System;
    using System.Net;

    public interface IWebRequestManager
    {
        WebRequest GetWebRequest(Uri uri);
    }
}