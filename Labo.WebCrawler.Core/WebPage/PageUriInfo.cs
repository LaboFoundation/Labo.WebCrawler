namespace Labo.WebCrawler.Core.WebPage
{
    using System;

    public sealed class PageUriInfo
    {
        private readonly Uri m_Uri;

        private readonly bool m_IsInternal;

        private readonly Uri m_ParentUri;

        private readonly int m_Depth;

        public PageUriInfo(Uri uri, bool isInternal, Uri parentUri, int depth)
        {
            m_Uri = uri;
            m_IsInternal = isInternal;
            m_ParentUri = parentUri;
            m_Depth = depth;
        }

        public Uri Uri
        {
            get
            {
                return m_Uri;
            }
        }

        public bool IsInternal
        {
            get
            {
                return m_IsInternal;
            }
        }

        public Uri ParentUri
        {
            get
            {
                return m_ParentUri;
            }
        }

        public int Depth
        {
            get
            {
                return m_Depth;
            }
        }
    }
}