namespace Labo.WebCrawler.Core.Protocol
{
    using System;

    using Labo.WebCrawler.Core.Content;

    public interface INetworkProtocolProvider
    {
        WebContentInfo GetWebContentInfo(Uri uri);

        WebContentData GetWebContentData(Uri uri);
    }
}