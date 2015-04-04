namespace Labo.WebCrawler.Core.Content
{
    using System;

    /// <summary>
    /// The default web content.
    /// </summary>
    public sealed class DefaultWebContent : IWebContent
    {
        /// <summary>
        /// Gets the base URI.
        /// </summary>
        /// <value>
        /// The base URI.
        /// </value>
        public Uri BaseUri { get; private set; }

        /// <summary>
        /// Gets the content information.
        /// </summary>
        /// <value>
        /// The content information.
        /// </value>
        public WebContentInfo ContentInfo { get; private set; }

        /// <summary>
        /// Gets the content data.
        /// </summary>
        /// <value>
        /// The content data.
        /// </value>
        public WebContentData ContentData { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWebContent"/> class.
        /// </summary>
        /// <param name="baseUri">The base uri.</param>
        /// <param name="contentInfo">The content information.</param>
        /// <param name="contentData">The content data.</param>
        /// <exception cref="System.ArgumentNullException">
        /// contentInfo
        /// or
        /// contentData
        /// </exception>
        public DefaultWebContent(Uri baseUri, WebContentInfo contentInfo, WebContentData contentData)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri");
            }

            if (contentInfo == null)
            {
                throw new ArgumentNullException("contentInfo");
            }

            if (contentData == null)
            {
                throw new ArgumentNullException("contentData");
            }

            BaseUri = baseUri;
            ContentInfo = contentInfo;
            ContentData = contentData;
        }
    }
}