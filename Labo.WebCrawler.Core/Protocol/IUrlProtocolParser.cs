namespace Labo.WebCrawler.Core.Protocol
{
    using System;

    public interface IUrlProtocolParser
    {
        string Parse(Uri uri);
    }
}