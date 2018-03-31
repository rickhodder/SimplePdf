using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SimplePdf.Lib
{
    public class SimpleDocumentOptions : ISimpleDocumentOptions
    {
        public Func<Stream> CreateOutputStream { get; set; }
        public Stream OutputStream { get; set; }
        public Rectangle PageLayout { get; set; } = PageSize.LETTER;
        public PdfPageEventHelper PageEventHelper { get; set; }
        public bool AutoOpenDocument { get; set; } = true;
        public bool AutoCloseDocument { get; set; } = true;
        public PdfWriter CreatePdfWriter(DocumentGenerationResult generationResult)
        {
            return PdfWriter.GetInstance(generationResult.TextSharpDocument, OutputStream);
        }
    }
}