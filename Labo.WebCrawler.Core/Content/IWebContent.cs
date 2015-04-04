// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWebContent.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 Bora Akgun
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The web content interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.WebCrawler.Core.Content
{
    using System;

    /// <summary>
    /// The web content interface.
    /// </summary>
    public interface IWebContent
    {
        /// <summary>
        /// Gets the content information.
        /// </summary>
        /// <value>
        /// The content information.
        /// </value>
        WebContentInfo ContentInfo { get; }

        /// <summary>
        /// Gets the content data.
        /// </summary>
        /// <value>
        /// The content data.
        /// </value>
        WebContentData ContentData { get; }

        /// <summary>
        /// Gets the base URI.
        /// </summary>
        /// <value>
        /// The base URI.
        /// </value>
        Uri BaseUri { get; }
    }
}