namespace Labo.WebCrawler.Core.Protocol
{
    using System;

    public sealed class UrlProtocolParser : IUrlProtocolParser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public string Parse(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return uri.Scheme.ToLowerInvariant();
        }
    }
}