using System.Collections.Generic;

namespace SimplePdf.Common
{
    public interface IResults
    {
        bool Success { get; set; }
        List<string> Messages { get; set; }
        List<string> Errors { get; set; }
    }
}