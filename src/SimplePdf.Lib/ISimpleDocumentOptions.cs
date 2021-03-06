﻿using System;
using System.IO;
using iTextSharp.text.pdf;
using SimplePdf.Common;

namespace SimplePdf.Lib
{
    public interface ISimpleDocumentOptions : IOptions
    {
        Func<Stream> CreateOutputStream { get; set; }
        iTextSharp.text.Rectangle PageLayout { get; set; } 
        PdfPageEventHelper PageEventHelper { get; set; }
        bool AutoOpenDocument { get; set; }
        bool AutoCloseDocument { get; set; }
    }
}