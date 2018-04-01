# SimplePdf
Utility classes for simplifying the generation of PDF files using ITextSharp

This is not meant to be the be-all, end-all PDF utility, but rather a set of utility classes/builders that simplify generating PDF files using the amazing ITextSharp library.

In the near future, builders that help with generating parts of documents will be coming: for example, you will be able to use a builder that in the Generate method that makes generating pdf tables much simpler.

Here's a simple example that shows creating a simple pdf file

First a document class with an options class is instantiated

``` c#
var pdfFileName = Path.Combine(Environment.CurrentDirectory, "expensereport.pdf");

var pdf = new ExpenseDocument(
                                new ExpenseDocumentOptions
                                {
                                    Message="Here are your expenses",
                                    CreateOutputStream = ()=> File.Create(pdfFileName)
                                });

var result = pdf.Execute();

if (result.Success)
{
    Process.Start(pdfFileName);
}
else
{
    foreach (var error in result.Errors)
    {
        Debug.WriteLine(error);
    }
}

public class ExpenseDocumentOptions : SimpleDocumentOptions
{
    // message to show in pdf
    public string Message {get;set;}
}

public class ExpenseDocument : AbstractSimpleDocument<ExpenseDocumentOptions>
{
    public ExpenseDocument(ExpenseDocumentOptions options) : base(options)
    {
    }
    protected override DocumentGenerationResult Generate()
    {
        var result = new DocumentGenerationResult();
        Context.TextSharpDocument.Add(
            new Paragraph
            {
                Options.Message
            });
        result.Success = true;
        return result;
    }
}
