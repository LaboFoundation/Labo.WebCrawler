namespace Labo.WebCrawler.Core
{
    using System.Reflection;

    public sealed class DefaultWebCrawlerVersionProvider : IWebCrawlerVersionProvider
    {
        public int GetVersion()
        {
            return Assembly.GetAssembly(typeof(IWebCrawlerVersionProvider)).GetName().Version.Major;
        }
    }
}