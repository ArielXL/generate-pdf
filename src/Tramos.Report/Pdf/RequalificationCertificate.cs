﻿using System;
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
    public class RequalificationCertificate
    {
        private const float heightBorder = 410;
        private const float widthBorder = 340;

        private const float left = 40;
        private const float right = 411;
        private const float bottom = 75;

        private const int countItemByPage = 2;
        // private const int countItemByPage = 4;

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
            //var s = Assembly.GetExecutingAssembly().GetManifestResourceNames();
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

        private static void WritePage(PdfWriterManager pdfWriter, IEnumerable<PdfData> pdfData,
            int startIndex, int countItemByPage, Image escudoImage)
        {
            List<PdfData> list = pdfData
                        .Skip(startIndex)
                        .Take(countItemByPage)
                        .ToList();

            WriteBorders(pdfWriter, list);
            WriteTopDocumentPage(pdfWriter, list);
            WriteBottomDocumentPage(pdfWriter, list, escudoImage);
        }

        private static void WriteTopDocumentPage(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            WriteHeaderEEVCInfo(pdfWriter, pdfData);
            WriteEEVCInfo(pdfWriter, pdfData);
            WriteNoteText(pdfWriter, pdfData);
            WriteFirmasText(pdfWriter, pdfData);
        }

        private static void WriteBottomDocumentPage(PdfWriterManager pdfWriter, IList<PdfData> pdfData, Image escudoImage)
        {
            WriteImagesPage(pdfWriter, pdfData, escudoImage);
            WriteCertificadoNo(pdfWriter, pdfData);
            WriteNamePerson(pdfWriter, pdfData);
            WriteIdentifyData(pdfWriter, pdfData);
            WritePosition(pdfWriter, pdfData);
            WriteInfoText(pdfWriter, pdfData);
            WriteRestDataText(pdfWriter, pdfData);
        }

        private static void WriteImagesPage(PdfWriterManager pdfWriter, IList<PdfData> pdfData, Image escudoImage)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        WriteTableWithImage(pdfWriter, escudoImage, left + 11, bottom + heightBorder / 2 - 65, widthBorder - 21);
                        break;
                    case "Right":
                        WriteTableWithImage(pdfWriter, escudoImage, right + 11, bottom + heightBorder / 2 - 65, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteTableWithImage(PdfWriterManager pdfWriter, Image escudoImage, float x, float y, float width)
        {
            ReportHelper.WriteTableWithImage(pdfWriter, GetTextsStartDocument(), escudoImage, 8, x, y, width, FontFactory.CreateFontBoldText());
        }

        private static IEnumerable<string> GetTextsStartDocument()
        {
            yield return ConstantText.MinisterioText;
            yield return ConstantText.EscuelaText;
        }

        private static void WriteBorders(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        WriteBorder(pdfWriter, left, bottom, widthBorder, heightBorder, 1);
                        WriteBorder(pdfWriter, left + 5, bottom + 5, widthBorder - 10, heightBorder - 10, 3);
                        WriteBorder(pdfWriter, left + 10, bottom + (heightBorder - 10) / 2 + 10, widthBorder - 20, (heightBorder - 10) / 2 - 10, 1);
                        WriteBorder(pdfWriter, left + 10, bottom + 10, widthBorder - 20, (heightBorder - 10) / 2 - 10, 1);
                        WriteBorder(pdfWriter, left + 5, bottom + 5 + (heightBorder - 10) / 2, widthBorder - 10, 1, 3);
                        break;
                    case "Right":
                        WriteBorder(pdfWriter, right, bottom, widthBorder, heightBorder, 1);
                        WriteBorder(pdfWriter, right + 5, bottom + 5, widthBorder - 10, heightBorder - 10, 3);
                        WriteBorder(pdfWriter, right + 10, bottom + (heightBorder - 10) / 2 + 10, widthBorder - 20, (heightBorder - 10) / 2 - 10, 1);
                        WriteBorder(pdfWriter, right + 10, bottom + 10, widthBorder - 20, (heightBorder - 10) / 2 - 10, 1);
                        WriteBorder(pdfWriter, right + 5, bottom + 5 + (heightBorder - 10) / 2, widthBorder - 10, 1, 3);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteBorder(PdfWriterManager pdfWriter, float x, float y, float width, float height, float lineWidth)
        {
            pdfWriter.SetColorStroke(ColorConstants.BLACK);
            pdfWriter.SetColorFill(ColorConstants.BLACK);
            pdfWriter.SetLineWidth(lineWidth);
            pdfWriter.DrawRectangle(x, y, width, height);
            pdfWriter.Stroke();
        }

        private static void WriteCertificadoNo(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetCertificadoNoParagraphs(pdfData[i]), 1, left + 11, bottom + heightBorder / 2 - 75, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetCertificadoNoParagraphs(pdfData[i]), 1, right + 11, bottom + heightBorder / 2 - 75, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetCertificadoNoParagraphs(PdfData pdfData)
        {
            yield return ReportHelper.GetTextParagraph(ConstantText.CertificadoText + "  " + pdfData.No, false, false, FontFactory.CreateFontText(), 8, TextAlignment.CENTER);
        }

        private static void WriteNamePerson(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetNameParagraphs(pdfData[i]), 3, left + 11, bottom + heightBorder / 2 - 105, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetNameParagraphs(pdfData[i]), 3, right + 11, bottom + heightBorder / 2 - 105, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetNameParagraphs(PdfData pdfData)
        {
            yield return ReportHelper.GetTextParagraph(pdfData.Name, true, true, FontFactory.CreateFontBoldText(), 9, TextAlignment.CENTER);
            yield return ReportHelper.GetTextParagraph(pdfData.FirstLastName, true, true, FontFactory.CreateFontBoldText(), 9, TextAlignment.CENTER);
            yield return ReportHelper.GetTextParagraph(pdfData.SecondLastName, true, true, FontFactory.CreateFontBoldText(), 9, TextAlignment.CENTER);

            yield return ReportHelper.GetTextParagraph(ConstantText.NombreHeaderText, false, false, FontFactory.CreateFontText(), 8, TextAlignment.CENTER);
            yield return ReportHelper.GetTextParagraph(ConstantText.PrimerApellidoHeaderText, false, false, FontFactory.CreateFontText(), 8, TextAlignment.CENTER);
            yield return ReportHelper.GetTextParagraph(ConstantText.SegundoApellidoHeaderText, false, false, FontFactory.CreateFontText(), 8, TextAlignment.CENTER);
        }

        private static void WriteIdentifyData(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetIdentityParagraphs(pdfData[i]), 3, left + 11, bottom + heightBorder / 2 - 120, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetIdentityParagraphs(pdfData[i]), 3, right + 11, bottom + heightBorder / 2 - 120, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetIdentityParagraphs(PdfData pdfData)
        {
            yield return ReportHelper.GetParagraph(ConstantText.EdadText, pdfData.Age, true, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 8, TextAlignment.CENTER);
            yield return ReportHelper.GetParagraph(ConstantText.CanetIdentidadText, pdfData.IdentityCard, true, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 8, TextAlignment.CENTER);
            yield return ReportHelper.GetParagraph(ConstantText.LicenciaConducionText, pdfData.CardDriver, true, true, FontFactory.CreateFontText(), FontFactory.CreateFontBoldText(), 8, TextAlignment.CENTER);
        }

        private static void WritePosition(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetPositionParagraphs(), 1, left + 11, bottom + heightBorder / 2 - 134, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetPositionParagraphs(), 1, right + 11, bottom + heightBorder / 2 - 134, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetPositionParagraphs()
        {
            yield return ReportHelper.GetParagraph(
                    ConstantText.CargoDesempenaText,
                    string.Empty, false, true, 
                    FontFactory.CreateFontText(),
                    FontFactory.CreateFontBoldText(),
                    8,
                    TextAlignment.LEFT);
        }

        private static void WriteInfoText(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetInfoParagraphs(), 1, left + 11, bottom + heightBorder / 2 - 158, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetInfoParagraphs(), 1, right + 11, bottom + heightBorder / 2 - 158, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetInfoParagraphs()
        {
            yield return ReportHelper.GetParagraph(
                ConstantText.AdicionalText,
                string.Empty,
                false, true, 
                FontFactory.CreateFontText(),
                FontFactory.CreateFontBoldText(),
                8,
                TextAlignment.LEFT);
        }

        private static void WriteRestDataText(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetRestDataParagraphs(pdfData[i]), 2, left + 11, bottom + heightBorder / 2 - 174, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetRestDataParagraphs(pdfData[i]), 2, right + 11, bottom + heightBorder / 2 - 174, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetRestDataParagraphs(PdfData pdfData)
        {
            yield return ReportHelper.GetTomoFolioParagraph(
                pdfData, true, 4,
                FontFactory.CreateFontText(),
                FontFactory.CreateFontBoldText(),
                8, TextAlignment.LEFT);

            yield return ReportHelper.GetParagraph(
                ConstantText.FechaExpedidoText,
                pdfData.ExpidetionDate.ToString("dd/MM/yy"), true, true, 
                FontFactory.CreateFontText(),
                FontFactory.CreateFontBoldText(),
                8, TextAlignment.RIGHT);
        }

        private static void WriteHeaderEEVCInfo(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetHeaderEEVCInfoParagraphs(), 1, left + 11, bottom + heightBorder - 30, widthBorder - 21);
                        break;

                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetHeaderEEVCInfoParagraphs(), 1, right + 11, bottom + heightBorder - 30, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetHeaderEEVCInfoParagraphs()
        {
            yield return ReportHelper.GetTextParagraph(ConstantText.DatosDeLaEEVCText, true, false, FontFactory.CreateFontBoldText(), 10, TextAlignment.CENTER);
        }

        private static void WriteEEVCInfo(PdfWriterManager pdfWriter, IList<PdfData> pdfData)
        {
            for (int i = 0; i < pdfData.Count && i < 2; i++)
            {           
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetEEVCInfoParagraphs(pdfData[i]), 1, left + 11, bottom + heightBorder - 92, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetEEVCInfoParagraphs(pdfData[i]), 1, right + 11, bottom + heightBorder - 92, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetEEVCInfoParagraphs(PdfData pdfData)
        {
            yield return ReportHelper.GetParagraphWhitoutUnderline(
                ConstantText.EEVCProvincialText, 
                pdfData.ProvincialEEVC, false, false, 
                FontFactory.CreateFontBoldText(), 
                FontFactory.CreateFontBoldText(), 9, 
                TextAlignment.CENTER
            );

            yield return ReportHelper.GetParagraphWhitoutUnderline(
                ConstantText.EEVCDirreccionText, 
                pdfData.AddressEEVC, false, false, 
                FontFactory.CreateFontBoldText(), 
                FontFactory.CreateFontBoldText(), 9, 
                TextAlignment.CENTER
            );

            yield return ReportHelper.GetParagraphWhitoutUnderline(
                ConstantText.EEVCAulaText, 
                pdfData.ClassroomEEVC, false, false, 
                FontFactory.CreateFontBoldText(), 
                FontFactory.CreateFontBoldText(), 9, 
                TextAlignment.CENTER
            );

            yield return ReportHelper.GetParagraphWhitoutUnderline(
                ConstantText.EEVCFechaVencimientoText, 
                pdfData.ExpirationDateEEVC.ToString("dd/MM/yy"), false, false, 
                FontFactory.CreateFontBoldText(), 
                FontFactory.CreateFontBoldText(), 9, 
                TextAlignment.CENTER
            );
        }

        private static void WriteNoteText(PdfWriterManager pdfWriter, IList<PdfData> people)
        {
            for (int i = 0; i < people.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoteParagraphs(), 1, left + 11, bottom + heightBorder - 150, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetNoteParagraphs(), 1, right + 11, bottom + heightBorder - 150, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetNoteParagraphs()
        {
            yield return ReportHelper.GetTextParagraph(ConstantText.DatosText, false, false, FontFactory.CreateFontText(), 9, TextAlignment.LEFT);
        }

        private static void WriteFirmasText(PdfWriterManager pdfWriter, IList<PdfData> people)
        {
            for (int i = 0; i < people.Count && i < 2; i++)
            {
                switch (Positions[i])
                {
                    case "Left":
                        ReportHelper.WriteDataTable(pdfWriter, GetFirmasParagraphs(), 2, left + 11, bottom + heightBorder - 170, widthBorder - 21);
                        break;
                    case "Right":
                        ReportHelper.WriteDataTable(pdfWriter, GetFirmasParagraphs(), 2, right + 11, bottom + heightBorder - 170, widthBorder - 21);
                        break;
                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Paragraph> GetFirmasParagraphs()
        {
            yield return ReportHelper.GetTextParagraph(ConstantText.FirmaSecretarioDocenteText, false, false, FontFactory.CreateFontText(), 9, TextAlignment.LEFT);
            yield return ReportHelper.GetTextParagraph(ConstantText.FirmaDirectorEEVCProvincialText, false, false, FontFactory.CreateFontText(), 9, TextAlignment.RIGHT);
        }

        private static readonly string[] Positions = new string[]
        {
            "Left",
            "Right",
        };
    }
}