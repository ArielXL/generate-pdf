using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace Tramos.Report.Pdf.Core
{
    public class FontFactory
    {
        public static PdfFont CreateFontText() => PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        public static PdfFont CreateFontBoldText() => PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
    }
}
