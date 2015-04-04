namespace Labo.WebCrawler.Core.WebPage
{
    using System;
    using System.Text;

    [Serializable]
    public sealed class PageContent
    {
        public byte[] Bytes { get; set; }

        public string Charset { get; set; }

        public Encoding Encoding { get; set; }

        public string Text { get; set; }
    }
}