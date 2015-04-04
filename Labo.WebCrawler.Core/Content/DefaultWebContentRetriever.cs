namespace Labo.WebCrawler.Core.Content
{
    using System;

    using Labo.WebCrawler.Core.Protocol;

    public sealed class DefaultWebContentRetriever : IWebContentRetriever
    {
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;

        public DefaultWebContentRetriever(INetworkProtocolProviderFactory networkProtocolProviderFactory)
        {
            m_NetworkProtocolProviderFactory = networkProtocolProviderFactory;
        }

        public WebContentInfo RetrieveWebContentInfo(Uri uri)
        {
            INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(uri);
            return networkProtocolProvider.GetWebContentInfo(uri);
        }

        public WebContentData RetrieveWebContentData(Uri uri)
        {
            INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(uri);
            return networkProtocolProvider.GetWebContentData(uri);
        }
    }
}