using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Tramos.Report.Pdf.Core
{
    public static class ParagraphFactory
    {
        public static Paragraph Create(string text, PdfFont font, float fontsize, float leading, TextAlignment alignement)
        {
            return new Paragraph(text)
                .SetFont(font)
                .SetFontSize(fontsize)
                .SetMultipliedLeading(leading)
                .SetTextAlignment(alignement);
        }

        public static Paragraph CreateEmpty() => new Paragraph();

        public static Paragraph CreateEmpty(PdfFont font, float fontsize, float leading, TextAlignment alignement)
        {
            return CreateEmpty()
                .SetFont(font)
                .SetFontSize(fontsize)
                .SetMultipliedLeading(leading)
                .SetTextAlignment(alignement);
        }
    }
}
