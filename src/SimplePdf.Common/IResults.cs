using System.Collections.Generic;

namespace SimplePdf.Common
{
    // Anything that can be a results class
    public interface IResults
    {
        bool Success { get; set; }
        List<string> Messages { get; set; }
        List<string> Errors { get; set; }
    }
}