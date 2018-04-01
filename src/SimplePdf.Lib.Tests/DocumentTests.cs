using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using iTextSharp.text;
using NUnit.Framework;
using SimplePdf.Lib.Builders;

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

        [Test]
        public void Generate_WhenCalled_ShowTableBuilder()
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, "expensereport.pdf");

            var doc = new ExpenseDocument2(new ExpenseDocument2Options
            {
                CreateOutputStream = () => File.Create(fileName),
                Expenses = new List<Expense>
                {
                    new Expense{Cost = 10.50m, Description = "Breakfast", ExpenseDate = DateTime.Now.Date.AddDays(-2)},
                    new Expense{Cost = 24.75m, Description = "Office Supplies", ExpenseDate = DateTime.Now.Date.AddDays(-1)},
                    new Expense{Cost = 100.50m, Description = "Breakfast", ExpenseDate = DateTime.Now.Date},
                }
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

    public class ExpenseDocument2Options : SimpleDocumentOptions
    {
        public string Message { get; set; }
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }

    public class ExpenseDocument2 : SimpleDocument<ExpenseDocument2Options>
    {
        public ExpenseDocument2(ExpenseDocument2Options options) : base(options)
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

            var builder = new SimpleTableBuilder<Expense>(() => new SimpleTableOptions<Expense>
            {
                Columns = new List<SimpleTableColumn<Expense>>
                {
                    new SimpleTableColumn<Expense>
                    {
                        Content = e=> e.Description,
                        Width = 10f,
                    },
                    new SimpleTableColumn<Expense>
                    {
                        Content = e=> $"{e.Cost:C}",
                        Width = 10f,
                    },
                    new SimpleTableColumn<Expense>
                    {
                        Content = e=> $"{e.ExpenseDate:yyyy MMMM dd}",
                        Width = 10f,
                    }
                }, 
                TotalWidth = 100f
                

            });

            Context.TextSharpDocument.Add(builder.Build(Options.Expenses));

            result.Success = true;
            return result;
        }
    }

    public class Expense
    {
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
