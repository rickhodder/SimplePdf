using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SimplePdf.Lib
{
    public class SimpleDocumentContext<TDocumentOptions> where TDocumentOptions : ISimpleDocumentOptions
    {
        public TDocumentOptions Options { get; set; }
        public PdfWriter Writer { get; set; }
        public Document TextSharpDocument { get; set; }
        public Stream Output { get; set; }

        public SimpleDocumentContext(TDocumentOptions options)
        {            
            Initialize(options);
        }

        private void Initialize(TDocumentOptions options)
        {
            Options = options;
            Output = Options.CreateOutputStream();
            TextSharpDocument = new Document();
            Writer = PdfWriter.GetInstance(TextSharpDocument, Output);
        }
    }
}