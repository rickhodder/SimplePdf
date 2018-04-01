using System.Collections.Generic;
using SimplePdf.Common;

namespace SimplePdf.Lib
{
    public class DocumentGenerationResult : IResults
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
    }
}
