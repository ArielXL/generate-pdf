using System;
using System.IO;
using System.Linq;
using iText.Kernel.Geom;
using System.Reflection;
using iText.Kernel.Colors;
using iText.Layout.Element;
using System.Threading.Tasks;
using Tramos.Report.Pdf.Core;
using iText.Layout.Properties;
using Tramos.Report.Pdf.Utils;
using System.Collections.Generic;

namespace Tramos.Report.Pdf
{
    public class PsychophysiologicalExam
    {
        private const float heightBorder = 220;
        private const float widthBorder = 340;

        private const float left = 40;
        private const float right = 411;
        private const float bottom = 40;
        private const float top = 321;

        private const int countItemByPage = 4;

        private const string escudoImagePath = "Tramos.Report.Pdf.AssemblyResources.escudo.png";

        public static Task<byte[]> TryGenerateReport(IEnumerable<PdfData> pdfData)
        {
            try
            {
                var stream = new MemoryStream();

                GenerateReport(stream, pdfData);

                stream.Close();
                return Task.FromResult(stream.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        private static void GenerateReport(Stream stream, IEnumerable<PdfData> pdfData)
        {
            var escudoImageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(escudoImagePath);
            var escudoImage = ImageFactory.CreateImageScaled(escudoImageStream, 0.25f, 0.25f);

            var pdfWriter = new PdfWriterManager(stream, PageSize.LETTER.Rotate());
            pdfWriter.AddMargin(1, 1, 1, 1);

            WritePage(pdfWriter, pdfData, 0, countItemByPage, escudoImage);

            for (int i = countItemByPage; i < pdfData.Count(); i += countItemByPage)
            {
                pdfWriter.AddNewPage();

                WritePage(pdfWriter, pdfData, i, countItemByPage, escudoImage);
            }

            pdfWriter.Close();
        }

        private static void WritePage(PdfWriterManager pdfWriter, IEnumerable<PdfData> pdfData, int startIndex, int countItemByPage, Image escudoImage)
        {
            var list = pdfData.Skip(startIndex).Take(countItemByPage).ToList();
            WriteBorders(pdfWriter, list);
            WriteImagesPage(pdfWriter, list, escudoImage);
            WriteNames(pdfWriter, list);
            WriteIdentifyData(pdfWriter, list);
            WritePositionData(pdfWriter, list);
            WriteNoMatriculaAndFechaVencimientoData(pdfWriter, list);
            WriteNoteDataText(pdfWriter, list);
            WriteRestDataText(pdfWriter, list);
        }

        private static void WriteImagesPage(PdfWriterManager pdfWriter, IList<PdfData> pdfData, Image escudoImage)
        {
            for (int i = 0; i < pdfData.Count && i < 4; i++)
            {
                switch (Positions[i])
                {
                    case "TopLeft":
                        WriteTableWithImage(pdfData[i], pdfWriter, escudoImage, left + 11, top + heightBorder - 91, widthBorder - 21);
                        break;
                    case "TopRight":
                        WriteTableWithImage(pdfData[i], pdfWriter, escudoImage, right + 11, top + heightBorder - 91, widthBorder - 21);
                        break;
                    case "BottomLeft":
                        WriteTableWithImage(pdfData[i], pdfWriter, escudoImage, left + 11, bottom + heightBorder - 91, widthBorder - 21);
                        break;
                    case "BottomRight":
                        WriteTableWithImage(pdfData[i], pdfWriter, escudoImage, right + 11, bottom + heightBorder - 91, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteTableWithImage(PdfData pdfData, PdfWriterManager pdfWriter, Image escudoImage, float x, float y, float width)
        {
            ReportHelper.WriteTableWithImage(pdfWriter, GetTextsStartDocument(pdfData), escudoImage, 7, x, y, width, FontFactory.CreateFontBoldText());
        }

        private static IEnumerable<string> GetTextsStartDocument(PdfData pdfData)
        {
            yield return ConstantText.MinistryText;
            yield return ConstantText.SchoolText;
            yield return ConstantText.ProvinciaText;
            yield return ConstantText.CertificadoText + "  " + pdfData.No;
        }

        private static void WriteBorders(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 4; i++)
            {
                switch (Positions[i])
                {
                    case "TopLeft":
                        WriteBorder(pdfWriter, left, top, widthBorder, heightBorder, 1);
                        WriteBorder(pdfWriter, left + 5, top + 5, widthBorder - 10, heightBorder - 10, 3);
                        WriteBorder(pdfWriter, left + 10, top + 10, widthBorder - 20, heightBorder - 20, 1);
                        break;
                    case "TopRight":
                        WriteBorder(pdfWriter, right, top, widthBorder, heightBorder, 1);
                        WriteBorder(pdfWriter, right + 5, top + 5, widthBorder - 10, heightBorder - 10, 3);
                        WriteBorder(pdfWriter, right + 10, top + 10, widthBorder - 20, heightBorder - 20, 1);
                        break;
                    case "BottomLeft":
                        WriteBorder(pdfWriter, left, bottom, widthBorder, heightBorder, 1);
                        WriteBorder(pdfWriter, left + 5, bottom + 5, widthBorder - 10, heightBorder - 10, 3);
                        WriteBorder(pdfWriter, left + 10, bottom + 10, widthBorder - 20, heightBorder - 20, 1);
                        break;
                    case "BottomRight":
                        WriteBorder(pdfWriter, right, bottom, widthBorder, heightBorder, 1);
                        WriteBorder(pdfWriter, right + 5, bottom + 5, widthBorder - 10, heightBorder - 10, 3);
                        WriteBorder(pdfWriter, right + 10, bottom + 10, widthBorder - 20, heightBorder - 20, 1);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteNames(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 4; i++)
            {
                switch (Positions[i])
                {
                    case "TopLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetNameParagraphs(pdfData[i]), 3, left + 17, top + heightBorder - 111, widthBorder - 35);
                        break;
                    case "TopRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetNameParagraphs(pdfData[i]), 3, right + 17, top + heightBorder - 111, widthBorder - 35);
                        break;
                    case "BottomLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetNameParagraphs(pdfData[i]), 3, left + 17, bottom + heightBorder - 111, widthBorder - 35);
                        break;
                    case "BottomRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetNameParagraphs(pdfData[i]), 3, right + 17, bottom + heightBorder - 111, widthBorder - 35);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteIdentifyData(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 4; i++)
            {
                switch (Positions[i])
                {
                    case "TopLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetIdentityParagraphs(pdfData[i], TextAlignment.RIGHT), 2, left + 17, top + heightBorder - 125, widthBorder - 35);
                        break;
                    case "TopRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetIdentityParagraphs(pdfData[i], TextAlignment.RIGHT), 2, right + 17, top + heightBorder - 125, widthBorder - 35);
                        break;
                    case "BottomLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetIdentityParagraphs(pdfData[i], TextAlignment.RIGHT), 2, left + 17, bottom + heightBorder - 125, widthBorder - 35);
                        break;
                    case "BottomRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetIdentityParagraphs(pdfData[i], TextAlignment.RIGHT), 2, right + 17, bottom + heightBorder - 125, widthBorder - 35);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteNoMatriculaAndFechaVencimientoData(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 4; i++)
            {
                switch (Positions[i])
                {
                    case "TopLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoMatriculaAndFechaVencimientoParagraphs(pdfData[i]), 2, left + 17, top + heightBorder - 148, widthBorder - 35);
                        break;
                    case "TopRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoMatriculaAndFechaVencimientoParagraphs(pdfData[i]), 2, right + 17, top + heightBorder - 148, widthBorder - 35);
                        break;
                    case "BottomLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoMatriculaAndFechaVencimientoParagraphs(pdfData[i]), 2, left + 17, bottom + heightBorder - 148, widthBorder - 35);
                        break;
                    case "BottomRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoMatriculaAndFechaVencimientoParagraphs(pdfData[i]), 2, right + 17, bottom + heightBorder - 148, widthBorder - 35);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WritePositionData(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 4; i++)
            {
                switch (Positions[i])
                {
                    case "TopLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetPositionParagraphs(TextAlignment.LEFT), 1, left + 17, top + heightBorder - 135, widthBorder - 25);
                        break;
                    case "TopRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetPositionParagraphs(TextAlignment.LEFT), 1, right + 17, top + heightBorder - 135, widthBorder - 25);
                        break;
                    case "BottomLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetPositionParagraphs(TextAlignment.LEFT), 1, left + 17, bottom + heightBorder - 135, widthBorder - 25);
                        break;
                    case "BottomRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetPositionParagraphs(TextAlignment.LEFT), 1, right + 17, bottom + heightBorder - 135, widthBorder - 25);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteNoteDataText(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 4; i++)
            {
                switch (Positions[i])
                {
                    case "TopLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoteParagraphs(TextAlignment.LEFT), 1, left + 13, top + heightBorder - 166, widthBorder - 25);
                        break;
                    case "TopRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoteParagraphs(TextAlignment.LEFT), 1, right + 13, top + heightBorder - 166, widthBorder - 25);
                        break;
                    case "BottomLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoteParagraphs(TextAlignment.LEFT), 1, left + 13, bottom + heightBorder - 166, widthBorder - 25);
                        break;
                    case "BottomRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoteParagraphs(TextAlignment.LEFT), 1, right + 13, bottom + heightBorder - 166, widthBorder - 25);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteRestDataText(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 4; i++)
            {
                switch (Positions[i])
                {
                    case "TopLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetRestDataParagraphs(pdfData[i]), 2, left + 17, top + heightBorder - 190, widthBorder - 35);
                        break;
                    case "TopRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetRestDataParagraphs(pdfData[i]), 2, right + 17, top + heightBorder - 190, widthBorder - 35);
                        break;
                    case "BottomLeft":
                        ReportHelper.WriteDataTable(pdfWriter, GetRestDataParagraphs(pdfData[i]), 2, left + 17, bottom + heightBorder - 190, widthBorder - 35);
                        break;
                    case "BottomRight":
                        ReportHelper.WriteDataTable(pdfWriter, GetRestDataParagraphs(pdfData[i]), 2, right + 17, bottom + heightBorder - 190, widthBorder - 35);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetNameParagraphs(PdfData pdfData)
        {
            yield return ReportHelper.GetParagraph(ConstantText.NombreText, pdfData.Name, false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.LEFT);
            yield return ReportHelper.GetParagraph(ConstantText.PrimerApellidoText, pdfData.FirstLastName, false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.CENTER);
            yield return ReportHelper.GetParagraph(ConstantText.SegundoApellidoText, pdfData.SecondLastName, false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.RIGHT);
        }

        private static IEnumerable<Paragraph> GetIdentityParagraphs(PdfData pdfData, TextAlignment textAlignment)
        {
            yield return ReportHelper.GetParagraph(ConstantText.CanetIdentidadHeaderText, pdfData.IdentityCard, true, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, textAlignment);
            yield return ReportHelper.GetParagraph(ConstantText.LicenciaConducionHeaderText, pdfData.CardDriver, true, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, textAlignment);
        }

        private static IEnumerable<Paragraph> GetPositionParagraphs(TextAlignment textAlignment)
        {
            yield return ReportHelper.GetParagraph(ConstantText.CargoDesempenaText, string.Empty, false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, textAlignment);
        }

        private static IEnumerable<Paragraph> GetNoteParagraphs(TextAlignment textAlignment)
        {
            yield return ReportHelper.GetParagraph(ConstantText.ExamPsicofisiologicoText, string.Empty, false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, textAlignment);
        }

        private static IEnumerable<Paragraph> GetNoMatriculaAndFechaVencimientoParagraphs(PdfData pdfData)
        {
            yield return ReportHelper.GetParagraph(ConstantText.NoMatriculaText, pdfData.RegistrationNumber, false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.LEFT);
            yield return ReportHelper.GetParagraph(ConstantText.FechaVencimientoText, pdfData.ExpirationDate.ToString("dd/MM/yy"), false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.RIGHT);
        }

        private static IEnumerable<Paragraph> GetRestDataParagraphs(PdfData pdfData)
        {
            yield return ReportHelper.GetTomoFolioParagraph(pdfData, false, 4, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.LEFT);
            yield return ReportHelper.GetParagraph(ConstantText.FechaExpedidoText, pdfData.ExpidetionDate.ToString("dd/MM/yy"), false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.RIGHT);
            yield return ReportHelper.GetParagraph(ConstantText.FirmaJefeEvaluacionText, string.Empty, false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.LEFT);
            yield return ReportHelper.GetParagraph(ConstantText.FirmaDirectorText, string.Empty, false, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 7, TextAlignment.RIGHT);
        }

        private static void WriteBorder(PdfWriterManager pdfWriter, float x, float y, float width, float height, float lineWidth)
        {
            pdfWriter.SetColorStroke(ColorConstants.BLACK);
            pdfWriter.SetColorFill(ColorConstants.BLACK);
            pdfWriter.SetLineWidth(lineWidth);
            pdfWriter.DrawRectangle(x, y, width, height);
            pdfWriter.Stroke();
        }

        private static readonly string[] Positions = new string[]
        {
            "TopLeft",
            "TopRight",
            "BottomLeft",
            "BottomRight",
        };
    }
}
