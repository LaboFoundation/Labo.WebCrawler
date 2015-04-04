namespace Labo.WebCrawler.Core.Content
{
    using System;
    using System.Net;
    using System.Text;

    [Serializable]
    public sealed class WebContentData
    {
        /// <summary>
        /// Gets a value indicating whether [has error].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has error]; otherwise, <c>false</c>.
        /// </value>
        public bool HasError
        {
            get
            {
                return Exception != null || WebException != null;
            }
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public byte[] Bytes { get; private set; }

        /// <summary>
        /// Gets the charset.
        /// </summary>
        /// <value>
        /// The charset.
        /// </value>
        public string Charset { get; private set; }

        /// <summary>
        /// Gets the encoding.
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets or sets the web exception.
        /// </summary>
        /// <value>
        /// The web exception.
        /// </value>
        public WebException WebException { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebContentData"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="charset">The charset.</param>
        /// <param name="bytes">The bytes.</param>
        public WebContentData(string text, Encoding encoding, string charset, byte[] bytes)
        {
            Text = text;
            Encoding = encoding;
            Charset = charset;
            Bytes = bytes;
        }
    }
}