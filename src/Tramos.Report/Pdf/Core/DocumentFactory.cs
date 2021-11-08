using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using System.IO;

namespace Tramos.Report.Pdf.Core
{
    public static class DocumentFactory
    {
        public static Document Create(Stream stream)
        {
            return new Document(new PdfDocument(new PdfWriter(stream)), PageSize.A4.Rotate(), false);
        }

        public static Document CreateWithPageSize(Stream stream, PageSize size)
        {
            return new Document(new PdfDocument(new PdfWriter(stream)), size, false);
        }
    }
}
