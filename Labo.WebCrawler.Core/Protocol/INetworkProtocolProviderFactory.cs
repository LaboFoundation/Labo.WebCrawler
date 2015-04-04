namespace Labo.WebCrawler.Core.Protocol
{
    using System;

    public interface INetworkProtocolProviderFactory
    {
        INetworkProtocolProvider CreateProvider(Uri uri);

        void RegisterProvider(string protocol, INetworkProtocolProvider provider);
    }
}