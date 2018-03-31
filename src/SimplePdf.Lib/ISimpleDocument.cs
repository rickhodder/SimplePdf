using SimplePdf.Common;

namespace SimplePdf.Lib
{
    public interface ISimpleDocument<TDocumentOptions> : ISomething<ISimpleDocumentOptions> where TDocumentOptions: ISimpleDocumentOptions
    {
        TDocumentOptions Options { get; set; }
        DocumentGenerationResult Execute();
    }
}