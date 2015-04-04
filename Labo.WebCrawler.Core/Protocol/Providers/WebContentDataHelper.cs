namespace Labo.WebCrawler.Core.Protocol.Providers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    using Labo.WebCrawler.Core.Content;

    internal static class WebContentDataHelper
    {
        public static WebContentData GetWebContentData(WebResponse response)
        {
            MemoryStream memoryStream = GetResponseRawData(response);

            string charset = GetCharsetFromHeaders(response);

            memoryStream.Seek(0, SeekOrigin.Begin);

            Encoding encoding = GetEncoding(charset);
            
            string contentString;
            using (StreamReader sr = new StreamReader(memoryStream, encoding))
            {
                contentString = sr.ReadToEnd();
            }

            return new WebContentData(contentString, encoding, charset, encoding.GetBytes(contentString));
        }

        private static string GetCharsetFromHeaders(WebResponse webResponse)
        {
            string charset = null;
            string contenType = webResponse.Headers["content-type"];
            if (contenType != null)
            {
                int ind = contenType.IndexOf("charset=", StringComparison.OrdinalIgnoreCase);
                if (ind != -1)
                {
                    charset = contenType.Substring(ind + 8);
                }
            }

            return charset;
        }

        private static Encoding GetEncoding(string charset)
        {
            Encoding utf8Encoding = Encoding.UTF8;
            if (charset != null)
            {
                try
                {
                    utf8Encoding = Encoding.GetEncoding(charset);
                }
                catch
                {
                    // TODO: Log
                }
            }

            return utf8Encoding;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private static MemoryStream GetResponseRawData(WebResponse webResponse)
        {
            MemoryStream rawData = new MemoryStream();

            using (Stream rs = webResponse.GetResponseStream())
            {
                byte[] buffer = new byte[1024];
                if (rs != null)
                {
                    int read = rs.Read(buffer, 0, buffer.Length);
                    while (read > 0)
                    {
                        rawData.Write(buffer, 0, read);
                        read = rs.Read(buffer, 0, buffer.Length);
                    }
                }
            }

            return rawData;
        }
    }
}