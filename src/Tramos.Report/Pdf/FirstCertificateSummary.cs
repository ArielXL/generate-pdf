﻿using System;
using System.IO;
using System.Linq;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Layout.Element;
using Tramos.Report.Pdf.Core;
using System.Threading.Tasks;
using Tramos.Report.Pdf.Utils;
using iText.Layout.Properties;
using System.Collections.Generic;

namespace Tramos.Report.Pdf
{
    internal class FirstCertificateSummary
    {
        private const float heightBorder = 350;
        private const float widthBorder = 700;

        private const float left = 45;
        private const float right = 40;
        private const float bottom = 140;

        private static PdfWriterManager _pdfWriter;

        internal static Task<byte[]> TryGeneratePdf(IEnumerable<Certificate> certificates)
        {
            try
            {
                var stream = new MemoryStream();

                GenerateReport(stream, certificates);

                stream.Close();
                return Task.FromResult(stream.ToArray());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw exception;
            }
        }

        private static void GenerateReport(Stream stream, IEnumerable<Certificate> certificates)
        {
            _pdfWriter = new PdfWriterManager(stream, PageSize.LETTER.Rotate());

            List<Certificate> certificatesList = certificates.ToList();
            for(int i = 0; i < certificatesList.Count; i++)
            {
                GenerateReportFirstPage(certificatesList[i]);
                _pdfWriter.AddNewPage();
                GenerateReportSecondPage(certificatesList[i]);
                if(i != certificatesList.Count - 1)
                    _pdfWriter.AddNewPage();
            }

            _pdfWriter.Close();
        }

        private static void GenerateReportFirstPage(Certificate certificate)
        {
            WriteBorders(
               rectangleLeft: left,
               rectangleTop: bottom,
               rectangleRight: widthBorder,
               rectangleBottom: heightBorder,
               lineWidth: 3);
            
            WriteHeadersFirstPage(certificate);

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 180,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 50,
               lineWidth: (float)0.25);

            WriteFirstFrameFirstPage();

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 245,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 50,
               lineWidth: 2);

            WriteSecondFrameFirstPage(certificate);

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 288,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 30,
               lineWidth: (float)0.25);

            WriteThirdFrameFirstPage(certificate);

            WriteFooterFirstPage();
        }

        private static void GenerateReportSecondPage(Certificate certificate)
        {
            WriteBorders(
               rectangleLeft: left,
               rectangleTop: bottom,
               rectangleRight: widthBorder,
               rectangleBottom: heightBorder,
               lineWidth: 3);
            
            WriteHeadersSecondPage();

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 105,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 40,
               lineWidth: (float)0.75);

            WriteFirstFrameSecondPage(certificate);

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 165,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 50,
               lineWidth: (float)0.75);

            WriteSecondFrameSecondPage();

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 340,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 165,
               lineWidth: (float)0.75);

            WriteThirdFrameSecondPage(certificate);
        }

        private static void WriteBorders(float rectangleLeft, float rectangleTop, float rectangleRight, float rectangleBottom, float lineWidth)
        {
            _pdfWriter.SetColorStroke(ColorConstants.BLACK);
            _pdfWriter.SetColorFill(ColorConstants.BLACK);
            _pdfWriter.SetLineWidth(lineWidth);
            _pdfWriter.DrawRectangle(rectangleLeft, rectangleTop, rectangleRight, rectangleBottom);
            _pdfWriter.Stroke();
        }

        private static void WriteHeadersFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> header1 = GetHeader1();

            ReportHelper.WriteDataTable(_pdfWriter, header1,
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 45,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeader1()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.SchoolText, true, false, FontFactory.CreateFontText(), 12, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.MinistryText, true, false, FontFactory.CreateFontText(), 12, TextAlignment.CENTER);
            }

            IEnumerable<Paragraph> header2 = GetHeader2();

            ReportHelper.WriteDataTable(_pdfWriter, header2,
                columns: 3,
                x: left + 20,
                y: bottom + heightBorder - 60,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeader2()
            {
                yield return ReportHelper.GetTextParagraph(
                   new string(ConstantText.BlankText, 5), false, false, FontFactory.CreateFontBoldText(), 5, TextAlignment.CENTER);

                yield return ReportHelper.GetParagraph(
                    ConstantText.NumberPlusStripText,
                    certificate.No, false, false,
                    FontFactory.CreateFontText(),
                    FontFactory.CreateFontText(),
                    11, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.RPRText, false, false, FontFactory.CreateFontText(), 11, TextAlignment.CENTER);
            }

            IEnumerable<Paragraph> header3 = GetHeader3();

            ReportHelper.WriteDataTable(_pdfWriter, header3,
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 125,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeader3()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.CertificateText, true, false, FontFactory.CreateFontBoldText(), 50, TextAlignment.CENTER);
            }
        }

        private static void WriteFirstFrameFirstPage()
        {
            IEnumerable<Paragraph> paragraph = GetFirstParagraph();

            ReportHelper.WriteDataTable(_pdfWriter, paragraph,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 173,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetFirstParagraph()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.CarrierText, true, false, FontFactory.CreateFontText(), 10, TextAlignment.LEFT);
            }
        }

        private static void WriteSecondFrameFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstLine = GetFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 217,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetFirstLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.ApproveText, true, false, FontFactory.CreateFontBoldText(), 11, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> secondLine = GetSecondLine();

            ReportHelper.WriteDataTable(_pdfWriter, secondLine,
              columns: 2,
              x: left + 20,
              y: bottom + heightBorder - 238,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetSecondLine()
            {
                List<string> texts = new List<string>() { ConstantText.DateText, ConstantText.PtosText };
                List<string> values = new List<string>() { certificate.Date.ToString("dd/MM/yy"), certificate.Ptos };

                yield return ReportHelper.GetParagraphWithSeparation(
                    texts, values, false, false, 50,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }
        }

        private static void WriteThirdFrameFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstLine = GetFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 280,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetFirstLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.RecalificationCodeText, certificate.RecalificationCode, false, false, FontFactory.CreateFontBoldText(), FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);
            }
        }

        private static void WriteFooterFirstPage()
        {
            IEnumerable<Paragraph> footer = GetFooter();

            ReportHelper.WriteDataTable(_pdfWriter, footer,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 340,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetFooter()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.NoteText, false, false, FontFactory.CreateFontText(), 10, TextAlignment.LEFT);
            }
        }

        private static void WriteHeadersSecondPage()
        {
            IEnumerable<Paragraph> header = GetHeader();

            ReportHelper.WriteDataTable(_pdfWriter, header,
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 45,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeader()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.SchoolText, true, false, FontFactory.CreateFontText(), 12, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.MinistryText, true, false, FontFactory.CreateFontText(), 12, TextAlignment.CENTER);
            }
        }

        private static void WriteFirstFrameSecondPage(Certificate certificate)
        {
            IEnumerable<Paragraph> paragraph = GetFirstParagraphs();

            ReportHelper.WriteDataTable(_pdfWriter, paragraph,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 100,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetFirstParagraphs()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.DispatcherDateText, certificate.ExpidetionDate.ToString("dd/MM/yy"), false, false, FontFactory.CreateFontBoldText(), FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);

                yield return ReportHelper.GetParagraph(
                    ConstantText.ValidUntilText, certificate.ValidUntil.ToString("dd/MM/yy"), false, false, FontFactory.CreateFontBoldText(), FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);
            }
        }

        private static void WriteSecondFrameSecondPage()
        {
            IEnumerable<Paragraph> paragraph = GetSecondParagraphs();

            ReportHelper.WriteDataTable(_pdfWriter, paragraph,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 147,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetSecondParagraphs()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.EEVCText, false, false, FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);
            }
        }

        private static void WriteThirdFrameSecondPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstLine = GetFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 2,
              x: left + 20,
              y: bottom + heightBorder - 195,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetFirstLine()
            {
                List<string> texts = new List<string>() { ConstantText.NumberNumberplateText, ConstantText.GroupText };
                List<string> values = new List<string>() { certificate.NumberNumberplate, certificate.Group };

                yield return ReportHelper.GetParagraphWithSeparation(
                    texts, values, false, false, 50,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> secondLine = GePersonalInfoSecondLine();

            ReportHelper.WriteDataTable(_pdfWriter, secondLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 215,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GePersonalInfoSecondLine()
            {
                string fullName = $"{certificate.Name} {certificate.FirstLastName} {certificate.SecondLastName}";

                yield return ReportHelper.GetParagraph(
                    ConstantText.NameLastNameStudentText, fullName, false, false, FontFactory.CreateFontBoldText(), FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> thirdLine = GetThirdLine();

            ReportHelper.WriteDataTable(_pdfWriter, thirdLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 235,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetThirdLine()
            {
                List<string> texts = new List<string>() { ConstantText.IdentityCardText, ConstantText.HashtagDrivingLicenseText };
                List<string> values = new List<string>() { certificate.IdentityCard, certificate.CardDriver };

                yield return ReportHelper.GetParagraphWithSeparation(
                    texts, values, false, false, 50,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> fourthLine = GetFourthLine();

            ReportHelper.WriteDataTable(_pdfWriter, fourthLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 255,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetFourthLine()
            {
                List<string> texts = new List<string>() { ConstantText.TomeText, ConstantText.FolioText, ConstantText.BookText };
                List<string> values = new List<string>() { certificate.Tomo, certificate.Folio, certificate.Book };

                yield return ReportHelper.GetParagraphWithSeparationForThree(
                    texts, values, false, false, 50, 50, 
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> fifthLine = GetFifthLine();

            ReportHelper.WriteDataTable(_pdfWriter, fifthLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 298,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GetFifthLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.OtherNoteText, false, false, FontFactory.CreateFontText(), 10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> sixth = GetSixthLine();

            ReportHelper.WriteDataTable(_pdfWriter, sixth,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 335,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GetSixthLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    new string(ConstantText.EmptyText, 50), false, true, FontFactory.CreateFontText(), 5, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.SignatureStampSecretaryText, false, false, FontFactory.CreateFontText(), 10, TextAlignment.CENTER);
            }
        }
    }
}
