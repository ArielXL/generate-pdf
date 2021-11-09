using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;
using Tramos.Report.Pdf.Core;

namespace Tramos.Report.Pdf.Utils
{
    public static class ReportHelper
    {
        public static void WriteDataTable(PdfWriterManager pdfWriter,
            IEnumerable<KeyValuePair<string, string>> data,
            int columns,
            float x,
            float y,
            float width,
            PdfFont fontBold,
            TextAlignment textAlignment)
        {
            var tableData = TableFactory.Create(columns, x, y, width);

            foreach (var item in data)
            {
                Paragraph paragraph = ParagraphFactory.CreateEmpty(fontBold, 7, 1, textAlignment);
                paragraph.Add(new Text(item.Key));
                paragraph.Add(new Text(" "));
                paragraph.Add(new Text(item.Value.ToUpper())
                                         .SetFont(fontBold)
                                         .SetItalic()
                                         .SetUnderline());
                tableData.AddCell(CellFactory.CreateCellWhithBorder(paragraph, Border.NO_BORDER));
            }

            pdfWriter.AddTable(tableData);
        }

        public static void WriteDataTable(PdfWriterManager pdfWriter,
            IEnumerable<Paragraph> paragraphs, int columns, float x, float y, float width)
        {
            Table tableData = TableFactory.Create(columns, x, y, width);

            foreach (Paragraph paragraph in paragraphs)
            {
                tableData.AddCell(CellFactory.CreateCellWhithBorder(paragraph, Border.NO_BORDER));
            }

            pdfWriter.AddTable(tableData);
        }

        public static Paragraph GetTomoFolioParagraph(Certificate certificate, bool isUpper, int separation, PdfFont font, PdfFont fontBold, float fontSize, TextAlignment textAlignment)
        {
            var paragraph = ParagraphFactory.CreateEmpty(fontBold, fontSize, 1, textAlignment);
            var textTomo = isUpper ? ConstantText.TomoText.ToUpper() : ConstantText.TomoText;
            paragraph.Add(new Text($"{textTomo}   ")).SetFont(font);
            paragraph.Add(new Text(certificate.Tomo.ToUpper()).SetFont(fontBold).SetItalic().SetUnderline());

            var separator = string.Empty;
            for (int i = 0; i < separation; i++)
            {
                separator += " ";
            }
            paragraph.Add(new Text(separator));
            var textFolio = isUpper ? ConstantText.FolioText.ToUpper() : ConstantText.FolioText;
            paragraph.Add(new Text($"{textFolio}   ")).SetFont(font);
            paragraph.Add(new Text(certificate.Folio.ToUpper()).SetFont(fontBold).SetItalic().SetUnderline());

            return paragraph;
        }

        public static Paragraph GetParagraphWithSeparation(List<string> texts, List<string> values, bool isUpperText, bool isUpperValue, int separation, PdfFont font, PdfFont fontBold, float fontSize, TextAlignment textAlignment)
        {
            var paragraph = ParagraphFactory.CreateEmpty(fontBold, fontSize, 1, textAlignment);

            var text1 = isUpperText ? texts[0].ToUpper() : texts[0];
            paragraph.Add(new Text($"{text1}")).SetFont(font);
            var t1 = isUpperValue ? values[0].ToUpper() : values[0];
            paragraph.Add(new Text(t1).SetFont(fontBold).SetItalic().SetUnderline());

            var separator = string.Empty;
            for (int i = 0; i < separation; i++)
            {
                separator += " ";
            }
            paragraph.Add(new Text(separator));

            var text2 = isUpperText ? texts[1].ToUpper() : texts[1];
            paragraph.Add(new Text($"{text2}")).SetFont(font);
            var t2 = isUpperValue ? values[1].ToUpper() : values[1];
            paragraph.Add(new Text(t2).SetFont(fontBold).SetItalic().SetUnderline());

            return paragraph;
        }

        public static Paragraph GetParagraph(string text, string value, bool isUpperText, bool isUpperValue, PdfFont font, PdfFont fontBold, float fontSize, TextAlignment textAlignment)
        {
            var paragraph = ParagraphFactory.CreateEmpty(fontBold, fontSize, 1, textAlignment);
            text = isUpperText ? text.ToUpper() : text;
            paragraph.Add(
                new Text($"{text} "))
                .SetFont(font);

            value = isUpperValue ? value.ToUpper() : value;
            paragraph.Add(
                new Text(value)
                .SetFont(fontBold)
                .SetItalic()
                .SetUnderline());

            return paragraph;
        }

        public static Paragraph GetTextParagraph(string text, bool isUpper, bool isUnderline, PdfFont font, float fontSize, TextAlignment textAlignment)
        {
            Paragraph paragraph = ParagraphFactory.CreateEmpty(font, fontSize, 1, textAlignment);
            text = isUpper ? text.ToUpper() : text;
            paragraph
                .Add(new Text($"{text}"))
                /*.SetFont(Font.FontBoldText)*/;

            if (isUnderline)
            {
                paragraph.SetItalic().SetUnderline();
            }

            return paragraph;
        }

        public static void WriteTableWithImage(
            PdfWriterManager pdfWriter, IEnumerable<string> texts, Image escudoImage,
            float fontSize, float x, float y, float width, PdfFont fontBold)
        {
            var tableData = new Table(1)
                .SetFixedPosition(x, y, width)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER);

            tableData.AddCell(
                CellFactory.CreateImageCell(
                    escudoImage.SetHorizontalAlignment(HorizontalAlignment.CENTER))
                .SetBorder(Border.NO_BORDER));

            foreach (var text in texts)
            {
                tableData.AddCell(
                    CellFactory.CreateSimpleCell(
                        ParagraphFactory.Create(text, fontBold, fontSize, 1, TextAlignment.CENTER))
                    .SetBorder(Border.NO_BORDER).SetHorizontalAlignment(HorizontalAlignment.CENTER));
            }

            pdfWriter.AddTable(tableData);
        }
    }
}
