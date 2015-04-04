namespace Labo.WebCrawler.Core.Normalizer
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;

    public sealed class DefaultUriNormalizer : IUriNormalizer
    {
        /// <summary>
        /// Normalizes the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <returns>The normalized uri.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// uri
        /// or
        /// baseUri
        /// </exception>
        public Uri NormalizeUri(Uri uri, Uri baseUri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri");
            }

            return NormalizeUrl(uri.ToString(), baseUri);
        }

        /// <summary>
        /// Normalizes the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <returns>Returns the normalized uri. If the url is null, empty or starts with # returns null.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// url
        /// or
        /// baseUri
        /// </exception>
        public Uri NormalizeUrl(string url, Uri baseUri)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri");
            }

            if (string.IsNullOrWhiteSpace(url) || url.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            url = url.Split('#')[0];

            if (url.IndexOf("?", StringComparison.OrdinalIgnoreCase) == -1
                && !url.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                string lastUrlPart = url.Split('/').LastOrDefault();
                if (lastUrlPart != null && lastUrlPart.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    url = string.Format(CultureInfo.InvariantCulture, "{0}/", url);
                }
            }

            /*
             *  http://msdn.microsoft.com/en-us/library/system.uri(v=vs.110).aspx
             *  The Uri properties return a canonical data representation in escaped encoding, with all characters with Unicode values greater than 127 replaced with their hexadecimal equivalents. To put the URI in canonical form, the Uri constructor performs the following steps:
             *  Converts the URI scheme to lowercase.
             *  Converts the host name to lowercase.
             *  If the host name is an IPv6 address, the canonical IPv6 address is used. ScopeId and other optional IPv6 data are removed.
             *  Removes default and empty port numbers.
             *  Canonicalizes the path for hierarchical URIs by compacting sequences such as /./, /../, //, including escaped representations. Note that there are some schemes for which escaped representations are not compacted.
             *  For hierarchical URIs, if the host is not terminated with a forward slash (/), one is added.
             *  By default, any reserved characters in the URI are escaped in accordance with RFC 2396. This behavior changes if International Resource Identifiers or International Domain Name parsing is enabled in which case reserved characters in the URI are escaped in accordance with RFC 3986 and RFC 3987.
             *  As part of canonicalization in the constructor for some schemes, escaped representations are compacted. The schemes for which URI will compact escaped sequences include the following: file, http, https, net.pipe, and net.tcp. For all other schemes, escaped sequences are not compacted. For example: if you percent encode the two dots ".." as "%2E%2E" then the URI constructor will compact this sequence for some schemes. 
             */
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);
                if (!uri.IsAbsoluteUri)
                {
                    uri = new Uri(baseUri, uri);
                }

                UriBuilder uriBuilder = new UriBuilder(uri);

                /*
                 * Limiting protocols. Limiting different application layer protocols. For example, the ghttpsh scheme could
                 * be replaced with ghttph. Example:
                 * https://www.example.com/ -> http://www.example.com/
                 */
                if (uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
                {
                    uriBuilder.Scheme = Uri.UriSchemeHttp;
                    uriBuilder.Port = -1;
                }

                uriBuilder.Query = uriBuilder.Query.Replace("&amp;", "&").TrimStart('?');

                /*
                 * Sorting the query parameters. Some web pages use more than one query parameter in the URL. A
                 * normalizer can sort the parameters into alphabetical order (with their values), and reassemble the URL.
                 *  Example:
                 *  http://www.example.com/display?lang=en&article=fred ¨
                 *  http://www.example.com/display?article=fred&lang=en
                 *  However, the order of parameters in a URL may be significant (this is not defined by the standard) and a
                 *  web server may allow the same variable to appear multiple times.
                 */
                NameValueCollection queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
                string[] orderedKeys = queryString.Keys.Cast<string>().OrderBy(x => x).ToArray();

                uriBuilder.Query = queryString.Count == 0 ? string.Empty : ToQueryString(orderedKeys, queryString);

                return uriBuilder.Uri;
            }

            return null;
        }

        private static string ToQueryString(string[] orderedKeys, NameValueCollection nvc)
        {
            StringBuilder queryStringBuilder = new StringBuilder();
            for (int i = 0; i < orderedKeys.Length; i++)
            {
                string key = orderedKeys[i];
                string[] values = nvc.GetValues(key);

                if (values != null)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        string value = values[j];
                        queryStringBuilder.Append(key);
                        queryStringBuilder.Append("=");
                        queryStringBuilder.Append(value);
                        queryStringBuilder.Append("&");
                    }
                }
            }

            return queryStringBuilder.ToString().TrimEnd('&');
        }
    }
}