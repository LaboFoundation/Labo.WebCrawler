namespace Labo.WebCrawler.Core.Filters
{
    using System;

    using Labo.WebCrawler.Core.Content;

    public sealed class FuncWebContentCrawlFilter : IWebContentCrawlFilter
    {
        private readonly Func<WebContentInfo, bool> m_Func;

        public FuncWebContentCrawlFilter(Func<WebContentInfo, bool> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            m_Func = func;
        }

        public bool MustCrawlContent(WebContentInfo contentInfo)
        {
            return m_Func(contentInfo);
        }
    }
}