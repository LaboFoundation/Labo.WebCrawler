namespace Labo.WebCrawler.Core
{
    using System;

    [Serializable]
    public sealed class UriFrontierEntry
    {
        public Uri Uri { get; private set; }

        public int Depth { get; private set; }

        public UriFrontierEntry(Uri uri, int depth)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (depth < 0)
            {
                throw new ArgumentOutOfRangeException("depth", "Uri depth cannot be less than 0.");
            }

            Uri = uri;
            Depth = depth;
        }
    }
}