namespace Labo.WebCrawler.Core.Modules
{
    using System;

    using Labo.WebCrawler.Core.Content;

    public sealed class ActionWebContentProcessorModule : IWebContentProcessorModule
    {
        private readonly Action<IWebContent> m_Action;

        public ActionWebContentProcessorModule(Action<IWebContent> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            m_Action = action;
        }

        public void ProcessWebContent(IWebContent webContent)
        {
            m_Action(webContent);
        }
    }
}