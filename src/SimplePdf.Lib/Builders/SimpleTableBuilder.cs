using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SimplePdf.Common;

namespace SimplePdf.Lib.Builders
{
    /// <summary>
    /// Encapsulates many default options for <see cref="PdfPTable"/>s. Options can be overridden before passing to builder
    /// </summary>
    /// <typeparam name="TDataClass">Class that will be rendered as rows of table</typeparam>
    public class SimpleTableOptions<TDataClass> : IOptions
    {
        public List<SimpleTableColumn<TDataClass>> Columns { get; set; } = new List<SimpleTableColumn<TDataClass>>();
        public iTextSharp.text.Font Font { get; set; } = FontFactory.GetFont("Times-Roman", 8, new BaseColor(Color.Black));
        public Color HeaderBackgroundColor { get; set; } = Color.White;
        public Color HeaderForegroundColor { get; set; } = Color.Black;
        public Color CellBackgroundColor { get; set; } = Color.White;
        public Color CellForegroundColor { get; set; } = Color.Black;
        public Color FooterBackgroundColor { get; set; } = Color.White;
        public Color FooterForegroundColor { get; set; } = Color.Black;
        public bool ShowColumnHeadings { get; set; } = true;
        public float TotalWidth { get; set; }
        public int HorizontalAlignment { get; set; } = Element.ALIGN_LEFT;
        public bool LockedWidth { get; set; } = true;
        public float SpacingBefore { get; set; } = 5f;
        public float SpacingAfter { get; set; } = 5f;
        public float CellPadding { get; set; } = 5f;
        public bool ShowCellBorders { get; set; } = true;
    }

    public class SimpleTableCell
    {
        public int HorizontalAlignment { get; set; } = Element.ALIGN_LEFT;
        public int VerticalAlignment { get; set; } = Element.ALIGN_MIDDLE;
        public Color BackgroundColor { get; set; } = Color.White;
        public Color ForegroundColor { get; set; } = Color.Black;
        public string Style { get; set; }

        public SimpleTableCell()
        {
        }

        /// <summary>
        /// In essence, this creates a cell based on the prototype of the
        /// a column.
        /// </summary>
        /// <param name="column">Column to clone the attributes from</param>
        public SimpleTableCell(SimpleTableColumn column)
        {
            var cell = (SimpleTableCell)column;
            HorizontalAlignment = cell.HorizontalAlignment;
            VerticalAlignment = cell.VerticalAlignment;
            BackgroundColor = cell.BackgroundColor;
            ForegroundColor = cell.ForegroundColor;
            Style = cell.Style;
        }
    }

    public class SimpleTableColumn : SimpleTableCell
    {
    }

    public class SimpleTableColumn<TDataClass> : SimpleTableColumn
    {
        public string HeaderText { get; set; }
        public float Width { get; set; }
        public Func<TDataClass, string> Content { get; set; }
        public Action<TDataClass, SimpleTableCell> CellFormat { get; set; }
        public Func<TDataClass, iTextSharp.text.Image> ImageContent { get; set; }
        public Func<TDataClass, PdfPTable> TableContent { get; set; }
    }

    
    public class SimpleTableBuilder<TDataClass> : ISomethingWithOptions<SimpleTableOptions<TDataClass>>
    {
        private readonly SimpleTableOptions<TDataClass> _options;
        private PdfPTable _table;

        public SimpleTableBuilder(Func<SimpleTableOptions<TDataClass>> options)
        {
            _options = options();
        }
        
        public PdfPTable Build(IEnumerable<TDataClass> data)
        {
            CreatePdfPTable();
            SetColumnWidths();
            CreateColumnHeaders();
            CreateRows(data);
            return _table;
        }

        public void CreatePdfPTable()
        {
            _table = new PdfPTable(_options.Columns.Count)
            {
                HorizontalAlignment = _options.HorizontalAlignment,
                TotalWidth = _options.TotalWidth,
                LockedWidth = _options.LockedWidth
            };
        }

        public void SetColumnWidths()
        {
            var widths = new float[_options.Columns.Count];
            for (var index = 0; index < _options.Columns.Count; index++)
            {
                widths[index] = _options.Columns[index].Width;
            }
            _table.SetWidths(widths);
        }

        public void CreateColumnHeaders()
        {
            if (!_options.ShowColumnHeadings)
            {
                return;
            }

            foreach (var header in _options.Columns)
            {
                // create a cell from the characteristics of its column

                var modifiedCell = new SimpleTableCell(header)
                {
                    BackgroundColor = _options.HeaderBackgroundColor,
                    ForegroundColor = _options.HeaderForegroundColor,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Style = "bold"
                };

                var cell = CreateCell(header.HeaderText, modifiedCell);
                _table.AddCell(cell);
            }
            // show header row at top of every page
            _table.HeaderRows = 1; // todo make this an option that defaults to 1
        }

        public void CreateRows(IEnumerable<TDataClass> data)
        {
            foreach (var result in data)
            {
                foreach (var column in _options.Columns)
                {
                    var cell = new SimpleTableCell(column);

                    if (column.CellFormat != null)
                    {
                        column.CellFormat(result, cell);
                    }

                    if (column.Content != null)
                    {
                        _table.AddCell(CreateCell(column.Content(result), cell));
                    }
                    else if (column.ImageContent != null)
                    {
                        _table.AddCell(CreateCell(column.ImageContent(result), cell));
                    }
                    else if (column.TableContent != null)
                    {
                        _table.AddCell(CreateCell(column.TableContent(result), cell));
                    }
                }
            }
        }

        // TODO - these CreateCells could be refactored to the Strategy pattern

        public PdfPCell CreateCell(string contents, SimpleTableCell tableCell)
        {
            var font = _options.Font;
            if (!string.IsNullOrEmpty(tableCell.Style))
            {
                font.SetStyle(tableCell.Style);
            }

            var cell = new PdfPCell(new Phrase(contents, font))
            {
                BackgroundColor = new BaseColor(tableCell.BackgroundColor),
                HorizontalAlignment = tableCell.HorizontalAlignment,
                VerticalAlignment = tableCell.VerticalAlignment,
                Padding = _options.CellPadding
            };

            if (!_options.ShowCellBorders)
            {
                cell.BorderWidth = 0;
            }
            return cell;
        }

        public PdfPCell CreateCell(iTextSharp.text.Image contents, SimpleTableCell tableCell)
        {
            var font = _options.Font;

            if (!string.IsNullOrEmpty(tableCell.Style))
            {
                font.SetStyle(tableCell.Style);
            }

            var cell = new PdfPCell(contents)
            {
                BackgroundColor = new BaseColor(tableCell.BackgroundColor),
                HorizontalAlignment = tableCell.HorizontalAlignment,
                VerticalAlignment = tableCell.VerticalAlignment,
                Padding = _options.CellPadding
            };

            if (!_options.ShowCellBorders)
            {
                cell.BorderWidth = 0;
            }

            return cell;
        }

        public PdfPCell CreateCell(PdfPTable contents, SimpleTableCell tableCell)
        {
            var font = _options.Font;
            if (!string.IsNullOrEmpty(tableCell.Style))
            {
                font.SetStyle(tableCell.Style);
            }

            var cell = new PdfPCell(contents)
            {
                BackgroundColor = new BaseColor(tableCell.BackgroundColor),
                HorizontalAlignment = tableCell.HorizontalAlignment,
                VerticalAlignment = tableCell.VerticalAlignment,
                Padding = _options.CellPadding
            };

            if (!_options.ShowCellBorders)
            {
                cell.BorderWidth = 0;
            }

            return cell;
        }    
    }
    
}