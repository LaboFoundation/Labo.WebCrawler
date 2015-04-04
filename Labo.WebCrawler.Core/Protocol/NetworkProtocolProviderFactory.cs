namespace Labo.WebCrawler.Core.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Labo.WebCrawler.Core.Protocol.Exceptions;

    public sealed class NetworkProtocolProviderFactory : INetworkProtocolProviderFactory
    {
        private readonly IUrlProtocolParser m_UrlProtocolParser;
        private readonly IDictionary<string, INetworkProtocolProvider> m_NetworkProtocolProviders;

        public NetworkProtocolProviderFactory(IUrlProtocolParser urlProtocolParser)
        {
            m_UrlProtocolParser = urlProtocolParser;

            m_NetworkProtocolProviders = new Dictionary<string, INetworkProtocolProvider>(StringComparer.OrdinalIgnoreCase);
        }

        public INetworkProtocolProvider CreateProvider(Uri uri)
        {
            string protocol = m_UrlProtocolParser.Parse(uri);
            lock (m_NetworkProtocolProviders)
            {
                INetworkProtocolProvider networkProtocolProvider;
                if (m_NetworkProtocolProviders.TryGetValue(protocol, out networkProtocolProvider))
                {
                    return networkProtocolProvider;
                }
            }
           
            throw new NetworkProtocolProviderFactoryException(string.Format(CultureInfo.CurrentCulture, "invalid protocol: '{0}'", protocol));
        }

        public void RegisterProvider(string protocol, INetworkProtocolProvider provider)
        {
            lock (m_NetworkProtocolProviders)
            {
                m_NetworkProtocolProviders[protocol] = provider;
            }
        }
    }
}
