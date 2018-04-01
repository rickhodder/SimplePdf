using System;
using System.Collections.Generic;

namespace SimplePdf.Lib
{
    public abstract class SimpleDocument<TDocumentOptions> : ISimpleDocument<TDocumentOptions>
        where TDocumentOptions : ISimpleDocumentOptions
    {
        public TDocumentOptions Options { get; set; }
        public SimpleDocumentContext<TDocumentOptions> Context { get; set; }

        protected SimpleDocument(TDocumentOptions options)
        {
            Options = options;
            CreateDocumentContext();
        }

        protected void CreateDocumentContext()
        {
            Context=new SimpleDocumentContext<TDocumentOptions>(Options);
        }

        public DocumentGenerationResult Execute()
        {
            Context.TextSharpDocument.SetPageSize(Options.PageLayout);

            if (Options.PageEventHelper != null)
            {
                Context.Writer.PageEvent = Options.PageEventHelper;
            }

            if (Options.AutoOpenDocument)
            {
                Context.TextSharpDocument.Open();
            }

            var generationResult = Generate();

            if (generationResult.Success && Options.AutoCloseDocument)
            {
                Context.TextSharpDocument.Close();
            }

            return generationResult;
        }

        protected abstract DocumentGenerationResult Generate();
    }
}