using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    "Expense Document"
                });
            result.Success = true;
            return result;
        }
    }
}
