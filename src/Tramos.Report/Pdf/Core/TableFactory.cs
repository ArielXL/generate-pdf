using iText.Layout.Borders;
using iText.Layout.Element;

namespace Tramos.Report.Pdf.Core
{
    public static class TableFactory
    {
        public static Table Create(int columns, float left, float bottom, float width)
        {
            float[] columnsWidth = new float[columns];
            for (int i = 0; i < columns; i++)
            {
                columnsWidth[i] = width / columns;
            }
            return new Table(columnsWidth)
                .SetFixedPosition(left, bottom, width)
                .SetBorder(Border.NO_BORDER);
        }
    }
}
