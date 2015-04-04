namespace Labo.WebCrawler.Core.Filters
{
    using System;

    public sealed class FuncUriCrawlFilter : IUriCrawlFilter
    {
        private readonly Func<Uri, bool> m_Func;

        public FuncUriCrawlFilter(Func<Uri, bool> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            m_Func = func;
        }

        public bool MustCrawlUri(Uri uri)
        {
            return m_Func(uri);
        }
    }
}