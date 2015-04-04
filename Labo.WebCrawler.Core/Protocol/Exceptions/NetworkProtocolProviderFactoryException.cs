namespace Labo.WebCrawler.Core.Protocol.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NetworkProtocolProviderFactoryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkProtocolProviderFactoryException"/> class.
        /// </summary>
        public NetworkProtocolProviderFactoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkProtocolProviderFactoryException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public NetworkProtocolProviderFactoryException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkProtocolProviderFactoryException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NetworkProtocolProviderFactoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkProtocolProviderFactoryException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected NetworkProtocolProviderFactoryException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkProtocolProviderFactoryException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public NetworkProtocolProviderFactoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
