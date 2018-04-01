using System;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using iTextSharp.text;
using NUnit.Framework;

namespace SimplePdf.Lib.Tests
{
    [TestFixture]
    public class DocumentTests
    {
        [Test]
        public void Generate_WhenCalled_ReturnsDocumentResult()
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, "expensereport.pdf");

            var doc = new ExpenseDocument(new ExpenseDocumentOptions
            {
                CreateOutputStream = ()=> File.Create(fileName)
            });

            var result = doc.Execute();

            if (result.Success)
            {
                Process.Start(fileName);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Debug.WriteLine(error);
                }
            }

            result.Success.Should().BeTrue();                       
        }

    }

    public class ExpenseDocumentOptions : SimpleDocumentOptions
    {
        public string Message { get; set; }
    }

    public class ExpenseDocument : SimpleDocument<ExpenseDocumentOptions>
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
}
