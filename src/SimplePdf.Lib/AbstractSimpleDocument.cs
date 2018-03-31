using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SimplePdf.Lib
{

    public abstract class AbstractSimpleDocument<TDocumentOptions> : ISimpleDocument<TDocumentOptions>
        where TDocumentOptions : ISimpleDocumentOptions
    {
        public TDocumentOptions Options { get; set; }

        protected AbstractSimpleDocument(TDocumentOptions options)
        {
            Options = options;
        }

        public DocumentGenerationResult Execute()
        {
            var generationResult =
                new DocumentGenerationResult(Options.CreateOutputStream());

            generationResult.TextSharpDocument.SetPageSize(Options.PageLayout);

            //var writer = generationResult.CreatePdfWriter();

            if (Options.PageEventHelper != null)
            {
                generationResult.Writer.PageEvent = Options.PageEventHelper;
            }

            if (Options.AutoOpenDocument)
            {
                generationResult.TextSharpDocument.Open();
            }

            Generate(generationResult);

            if (generationResult.Success && Options.AutoCloseDocument)
            {
                generationResult.TextSharpDocument.Close();
            }

            return generationResult;
        }

        protected abstract void Generate(DocumentGenerationResult result);
    }
}