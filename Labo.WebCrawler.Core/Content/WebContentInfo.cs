// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebContentInfo.cs" company="Labo">
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
//   Defines the WebContentInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.WebCrawler.Core.Content
{
    using System;
    using System.Net;

    [Serializable]
    public sealed class WebContentInfo
    {
        public Uri Uri { get; private set; }

        public string MimeType { get; private set; }

        public long ContentLength { get; private set; }

        public bool AcceptRanges { get; private set; }

        public bool IsRoot { get; set; }

        public bool IsExternal { get; set; }

        public Exception Exception { get; set; }

        public WebException WebException { get; set; }

        public DateTime LastModified { get; private set; }

        public WebContentInfo(Uri uri, string mimeType, long contentLength, bool acceptRanges, DateTime lastModified)
        {
            LastModified = lastModified;
            AcceptRanges = acceptRanges;
            ContentLength = contentLength;
            MimeType = mimeType;
            Uri = uri;
        }
    }
}
