namespace Labo.WebCrawler.Core.WebPage
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class WebPageInfoCollection : List<WebPageInfo>
    {
        public WebPageInfoCollection()
            : base()
        {
        }

        public WebPageInfoCollection(IEnumerable<WebPageInfo> collection)
            : base(collection)
        {
        }

        public WebPageInfoCollection(int capacity)
            : base(capacity)
        {
        }
    }
}