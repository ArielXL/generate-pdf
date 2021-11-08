using System;
using System.IO;
using System.Linq;
using System.Reflection;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Layout.Element;
using System.Threading.Tasks;
using Tramos.Report.Pdf.Core;
using iText.Layout.Properties;
using Tramos.Report.Pdf.Utils;
using System.Collections.Generic;

namespace Tramos.Report.Pdf
{
    internal class SecondCertificateSummary
    {
        private const float heightBorder = 350;
        private const float widthBorder = 700;

        private const float left = 45;
        private const float right = 40;
        private const float bottom = 140;

        private static PdfWriterManager _pdfWriter;
        private const string escudoImagePath = "Tramos.Report.Pdf.AssemblyResources.escudo.png";

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

            WriteImages(left + 315, bottom + 275, 100);

            WriteHeadersFirstPage();

            WriteBorders(
               rectangleLeft: left + 20,
               rectangleTop: bottom + heightBorder - 223,
               rectangleRight: widthBorder - 40,
               rectangleBottom: 45,
               lineWidth: (float)0.25);

            WriteFirstFrameFirstPage();

            WriteBorders(
               rectangleLeft: left + 20,
               rectangleTop: bottom + heightBorder - 340,
               rectangleRight: widthBorder - 40,
               rectangleBottom: 110,
               lineWidth: (float)0.25);

            WriteSecondFrameFirstPage(certificate);
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
               rectangleLeft: left + 15,
               rectangleTop: bottom + heightBorder - 340,
               rectangleRight: widthBorder - 30,
               rectangleBottom: 280,
               lineWidth: (float)0.25);

            WriteMainParagraphSecondPage(certificate);

            WriteNoteSecondPage();

            WriteFooterSecondPage();
        }

        private static void WriteBorders(float rectangleLeft, float rectangleTop, float rectangleRight, float rectangleBottom, float lineWidth)
        {
            _pdfWriter.SetColorStroke(ColorConstants.BLACK);
            _pdfWriter.SetColorFill(ColorConstants.BLACK);
            _pdfWriter.SetLineWidth(lineWidth);
            _pdfWriter.DrawRectangle(rectangleLeft, rectangleTop, rectangleRight, rectangleBottom);
            _pdfWriter.Stroke();
        }

        private static void WriteImages(float x, float y, float width)
        {
            var escudoImageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(escudoImagePath);
            var escudoImage = ImageFactory.CreateImageScaled(escudoImageStream, 0.5f, 0.5f);
            ReportHelper.WriteTableWithImage(_pdfWriter, new List<string>(), escudoImage, 7, x, y, width, FontFactory.CreateFontBoldText());
        }

        private static void WriteHeadersFirstPage()
        {
            IEnumerable<Paragraph> header1 = GetHeader1();

            ReportHelper.WriteDataTable(_pdfWriter, header1,
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 120,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeader1()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.MinistryText, true, false, FontFactory.CreateFontText(), 12, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.SchoolText, true, false, FontFactory.CreateFontText(), 12, TextAlignment.CENTER);
            }

            IEnumerable<Paragraph> header2 = GetHeader2();

            ReportHelper.WriteDataTable(_pdfWriter, header2,
                columns: 5,
                x: left + 20,
                y: bottom + heightBorder - 160,
                width: widthBorder - 30);

            IEnumerable<Paragraph> GetHeader2()
            {
                yield return ReportHelper.GetTextParagraph(
                    new string(ConstantText.BlankText, 3), false, true, FontFactory.CreateFontText(), 5, TextAlignment.LEFT);

                yield return ReportHelper.GetTextParagraph(
                    new string(ConstantText.BlankText, 3), false, true, FontFactory.CreateFontText(), 5, TextAlignment.LEFT);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.CertificateText, true, false, FontFactory.CreateFontBoldText(), 15, TextAlignment.RIGHT);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.Number21Text, false, false, FontFactory.CreateFontText(), 15, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    new string(ConstantText.BlankText, 3), false, true, FontFactory.CreateFontText(), 5, TextAlignment.LEFT);
            }
        }
    
        private static void WriteFirstFrameFirstPage()
        {
            IEnumerable<Paragraph> paragraphs = GetFirstParagraph();

            ReportHelper.WriteDataTable(_pdfWriter, paragraphs,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 220,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GetFirstParagraph()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.PresentPortadorText, false, false, FontFactory.CreateFontText(), 10, TextAlignment.LEFT);
            }
        }

        private static void WriteSecondFrameFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstLine = GetFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 250,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GetFirstLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.ApproveText, certificate.Approve, false, false, FontFactory.CreateFontBoldText(), FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> secondLine = GetSecondLine();

            ReportHelper.WriteDataTable(_pdfWriter, secondLine,
              columns: 2,
              x: left + 30,
              y: bottom + heightBorder - 275,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetSecondLine()
            {
                List<string> texts = new List<string>() { ConstantText.DateText, ConstantText.PointsText };
                List<string> values = new List<string>() { certificate.Date.ToString("dd/MM/yy"), certificate.Points };

                yield return ReportHelper.GetParagraphWithSeparation(
                    texts, values, false, false, 50,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> thirdLine = GetThirdLine();

            ReportHelper.WriteDataTable(_pdfWriter, thirdLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 303,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetThirdLine()
            {
                List<string> texts = new List<string>() { ConstantText.ExaminerText, ConstantText.EEVCText, ConstantText.TCPText };
                List<string> values = new List<string>() { certificate.Examiner, "", "" };
                
                yield return ReportHelper.GetParagraphWithSeparationForThree(
                    texts, values, false, false, 58, 32, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );

                WriteBorders(
                    rectangleLeft: left + 465,
                    rectangleTop: bottom + heightBorder - 299,
                    rectangleRight: widthBorder - 690,
                    rectangleBottom: 10,
                    lineWidth: 1);

                WriteBorders(
                    rectangleLeft: left + 583,
                    rectangleTop: bottom + heightBorder - 299,
                    rectangleRight: widthBorder - 690,
                    rectangleBottom: 10,
                    lineWidth: 1);
            }

            IEnumerable<Paragraph> fourthLine = GetFourthLine();

            ReportHelper.WriteDataTable(_pdfWriter, fourthLine,
              columns: 1,
              x: left + 123,
              y: bottom + heightBorder - 315,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetFourthLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.IdentificationNumberText, false, false, FontFactory.CreateFontText(), 10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> fifthLine = GetFifthLine();

            ReportHelper.WriteDataTable(_pdfWriter, fifthLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 335,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetFifthLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.ExperimentationNumberText, 
                    certificate.ExperimentationNumber, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.RIGHT
                );
            }
        }

        private static void WriteHeadersSecondPage()
        {
            IEnumerable<Paragraph> paragraphs1 = GetHeaderParagraphs1();

            ReportHelper.WriteDataTable(_pdfWriter, paragraphs1,
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 45,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeaderParagraphs1()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.MinistryText, true, false, FontFactory.CreateFontText(), 12, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.SchoolText, true, false, FontFactory.CreateFontText(), 12, TextAlignment.CENTER);
            }
        }

        private static void WriteMainParagraphSecondPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstLine = GetFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 80,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetFirstLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.CompleteEEVCText, true, false, FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> secondLine = GetSecondLine();

            ReportHelper.WriteDataTable(_pdfWriter, secondLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 100,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetSecondLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.DispatcherDateText, 
                    certificate.ExpidetionDate.ToString("dd/MM/yy"), false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }

            IEnumerable<Paragraph> thirdLine = GetThirdLine();

            ReportHelper.WriteDataTable(_pdfWriter, thirdLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 120,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetThirdLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.ValidDateText, 
                    certificate.ValidDate.ToString("dd/MM/yy"), false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }

            IEnumerable<Paragraph> fourthLine = GetFourthLine();

            ReportHelper.WriteDataTable(_pdfWriter, fourthLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 140,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetFourthLine()
            {
                List<string> texts = new List<string>() { ConstantText.ProvincialEEVCText, ConstantText.MunicipalClassroomText };
                List<string> values = new List<string>() { certificate.ProvincialEEVC, certificate.MunicipalClassroom };
                
                yield return ReportHelper.GetParagraphWithSeparation(
                    texts, values, false, false, 50, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }

            IEnumerable<Paragraph> fifthLine = GetFifthLine();

            ReportHelper.WriteDataTable(_pdfWriter, fifthLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 160,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetFifthLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.NumberNumberplateText, 
                    certificate.NumberNumberplate, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }

            IEnumerable<Paragraph> sixthLine = GetSixthLine();

            ReportHelper.WriteDataTable(_pdfWriter, sixthLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 180,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetSixthLine()
            {
                string fullName = $"{certificate.Name} {certificate.FirstLastName} {certificate.SecondLastName}";

                yield return ReportHelper.GetParagraph(
                    ConstantText.NameLastNameStudentText, 
                    fullName, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }

            IEnumerable<Paragraph> seventhLine = GetSeventhLine();

            ReportHelper.WriteDataTable(_pdfWriter, seventhLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 200,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetSeventhLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.IdentityCardText, 
                    certificate.IdentityCard, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }

            IEnumerable<Paragraph> eighthLine = GetEighthLine();

            ReportHelper.WriteDataTable(_pdfWriter, eighthLine,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 220,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetEighthLine()
            {
                List<string> texts = new List<string>() { ConstantText.TomeText, ConstantText.FolioText, ConstantText.BookText };
                List<string> values = new List<string>() { certificate.Tomo, certificate.Folio, certificate.Book };
                
                yield return ReportHelper.GetParagraphWithSeparationForThree(
                    texts, values, false, false, 50, 50, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }
        }

        private static void WriteNoteSecondPage()
        {
            IEnumerable<Paragraph> note = GetNote();

            ReportHelper.WriteDataTable(_pdfWriter, note,
              columns: 1,
              x: left + 30,
              y: bottom + heightBorder - 260,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetNote()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.OtherNoteText, false, false, FontFactory.CreateFontText(), 10, TextAlignment.LEFT);
            }
        }

        private static void WriteFooterSecondPage()
        {
            IEnumerable<Paragraph> firstLine = GetFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 2,
              x: left + 30,
              y: bottom + heightBorder - 310,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetFirstLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    new string(ConstantText.EmptyText, 80), false, true, FontFactory.CreateFontText(), 5, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    new string(ConstantText.EmptyText, 100), false, true, FontFactory.CreateFontText(), 5, TextAlignment.CENTER);
            }

            IEnumerable<Paragraph> secondLine = GetSecondLine();

            ReportHelper.WriteDataTable(_pdfWriter, secondLine,
              columns: 2,
              x: left + 30,
              y: bottom + heightBorder - 325,
              width: widthBorder - 60);

            IEnumerable<Paragraph> GetSecondLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.DocentSecretary, false, false, FontFactory.CreateFontText(), 10, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.FirmeDirectorText, false, false, FontFactory.CreateFontText(), 10, TextAlignment.CENTER);
            }
        }
    }
}
