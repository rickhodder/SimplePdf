using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SimplePdf.Common;

namespace SimplePdf.Lib
{
    public class DocumentGenerationResult : IResults
    {
        public DocumentGenerationResult(Stream output)
        {
            Output = output;
            TextSharpDocument = new Document();
            Writer = PdfWriter.GetInstance(TextSharpDocument, Output);
        }

        public PdfWriter Writer { get; set; }

        public Document TextSharpDocument { get; set; }

        public Stream Output { get; set; }
        public bool Success { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
    }
}
