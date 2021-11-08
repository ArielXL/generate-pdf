using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;

namespace Tramos.Report.Pdf.Core
{
    public class PdfWriterManager
    {
        private readonly Document document;
        // private readonly PdfCanvas pdfCurrentCanvas;

        public PdfWriterManager(Stream stream)
        {
            document = DocumentFactory.Create(stream);
            document.GetPdfDocument().AddNewPage();
            //pdfCurrentCanvas = new PdfCanvas(GetLastPage);
        }

        public PdfWriterManager(Stream stream, PageSize pageSize)
        {
            document = DocumentFactory.CreateWithPageSize(stream, pageSize);
            document.GetPdfDocument().AddNewPage();
            //pdfCurrentCanvas = new PdfCanvas(GetLastPage);
        }

        public PdfPage GetLastPage { get => document.GetPdfDocument().GetLastPage(); }

        public PdfCanvas GetCurrentCanvas { get => new PdfCanvas(GetLastPage); }

        public void AddNewPage()
        {
            document.GetPdfDocument().AddNewPage();
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            //pdfCurrentCanvas = new PdfCanvas(GetLastPage);
        }

        public void AddMargin(float topMargin, float rightMargin, float bottonMargin, float leftMargin)
        {
            document.SetMargins(topMargin, rightMargin, bottonMargin, leftMargin);
        }

        public void Close()
        {
            document.Close();
        }

        public void DrawRectangle(float rectangleLeft, float rectangleTop, float rectangleRight, float rectangleBottom)
        {
            PdfCanvas pdfCanvas = GetCurrentCanvas
                .Rectangle(rectangleLeft, rectangleTop, rectangleRight, rectangleBottom);
            pdfCanvas.Release();

        }

        public void SetColorStroke(Color color)
        {
            PdfCanvas pdfCanvas = GetCurrentCanvas.SetStrokeColor(color);
            pdfCanvas.Release();
        }

        public void SetColorFill(Color color)
        {
            PdfCanvas pdfCanvas = GetCurrentCanvas.SetFillColor(color);
            pdfCanvas.Release();
        }

        public void SetLineWidth(float lineWidth)
        {
            PdfCanvas pdfCanvas = GetCurrentCanvas.SetLineWidth(lineWidth);
            pdfCanvas.Release();
        }

        public void Stroke()
        {
            PdfCanvas pdfCanvas = GetCurrentCanvas.Stroke();
            pdfCanvas.Release();
        }

        public void Fill()
        {
            PdfCanvas pdfCanvas = GetCurrentCanvas.Fill();
            pdfCanvas.Release();
        }

        public void AddTable(Table table) => document.Add(table);
    }
}
