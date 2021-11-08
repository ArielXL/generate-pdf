using iText.Layout.Borders;
using iText.Layout.Element;

namespace Tramos.Report.Pdf.Core
{
    public static class CellFactory
    {
        public static Cell CreateSimpleCell(Paragraph paragraph)
        {
            return new Cell().Add(paragraph);
        }

        public static Cell CreateImageCell(Image image)
        {
            return new Cell().Add(image);
        }

        public static Cell CreateCellWhithBorder(Paragraph paragraph, Border borderType)
        {
            return CreateSimpleCell(paragraph)
                .SetBorder(borderType);
        }
    }
}
