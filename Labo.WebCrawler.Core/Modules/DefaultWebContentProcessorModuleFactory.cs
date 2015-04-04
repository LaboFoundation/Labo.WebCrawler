namespace Labo.WebCrawler.Core.Modules
{
    using System.Collections.Generic;

    public sealed class DefaultWebContentProcessorModuleFactory : IWebContentProcessorModuleFactory
    {
        private readonly Dictionary<string, List<IWebContentProcessorModule>> m_Modules = new Dictionary<string, List<IWebContentProcessorModule>>();

        public IWebContentProcessorModule[] GetContentProcessorModules(string mimeType)
        {
            List<IWebContentProcessorModule> modules;
            if (m_Modules.TryGetValue(mimeType, out modules))
            {
                return modules.ToArray();
            }

            return new IWebContentProcessorModule[0];
        }

        public void RegisterContentProcessorModule(string mimeType, IWebContentProcessorModule contentProcessorModule)
        {
            List<IWebContentProcessorModule> modules;
            if (m_Modules.TryGetValue(mimeType, out modules))
            {
                modules.Add(contentProcessorModule);
            }
            else
            {
                m_Modules.Add(mimeType, new List<IWebContentProcessorModule> { contentProcessorModule });
            }
        }
    }
}