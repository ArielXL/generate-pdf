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
using iText.IO.Image;


namespace Tramos.Report.Pdf
{
    internal class PotentialAssessmentSummary
    {
        private const float heightBorder = 400;
        private const float widthBorder = 700;

        private const float left = 45;
        private const float right = 40;
        private const float bottom = 110;

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
            GenerateReportFirstPage(certificatesList[0]);
            _pdfWriter.AddNewPage();
            GenerateReportFirstPage(certificatesList[1]);
            // for(int i = 0; i < certificatesList.Count; i++)
            // {
            //     GenerateReportFirstPage(certificatesList[i]);
            //     _pdfWriter.AddNewPage();
            //     // GenerateReportSecondPage(certificatesList[i]);
            //     if(i != certificatesList.Count - 1)
            //         _pdfWriter.AddNewPage();
            // }

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

            WriteImages(left + 5, bottom + 340, 50);

            WriteHeadersFirstPage(certificate);

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 180,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 35,
               lineWidth: (float)0.25);

            WriteSchoolInfoFirstPage(certificate);

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 245,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 50,
               lineWidth: (float)0.25);

            WriteEvaluationInfoFirstPage(certificate);

            WriteBorders(
               rectangleLeft: left + 10,
               rectangleTop: bottom + heightBorder - 375,
               rectangleRight: widthBorder - 20,
               rectangleBottom: 115,
               lineWidth: (float)0.25);

            WritePersonalInfoFirstPage(certificate);
        }

        private static void GenerateReportSecondPage(Certificate certificate)
        {
            WriteBorders(
               rectangleLeft: left,
               rectangleTop: bottom,
               rectangleRight: widthBorder,
               rectangleBottom: heightBorder,
               lineWidth: 3);
            
            WriteHeadersSecondPage(certificate);

            int height = 120;
            for(int i = 0; i < 12; i++)
            {
                WriteLineSecondPage(certificate, height);
                height += 20;
            }

            WriteFooterSecondPage(certificate);
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
            string path = "../Tramos.Report/Pdf/AssemblyResources/tramos.png";
            var image = new Image(ImageDataFactory.Create(path));
            image.Scale(0.35f, 0.35f);
            ReportHelper.WriteTableWithImage(_pdfWriter, new List<string>(), image, 7, x, y, width, FontFactory.CreateFontBoldText());
        }

        private static void WriteHeadersFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> header1 = GetHeader1();

            ReportHelper.WriteDataTable(_pdfWriter, header1,
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 65,
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
                columns: 2,
                x: left + 20,
                y: bottom + heightBorder - 80,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeader2()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.NumberPlusStripText, false, false, FontFactory.CreateFontText(), 11, TextAlignment.RIGHT);

                yield return ReportHelper.GetTextParagraph(
                    certificate.No, false, true, FontFactory.CreateFontText(), 11, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> header3 = GetHeader3();

            ReportHelper.WriteDataTable(_pdfWriter, header3,
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 135,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeader3()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.SummaryFirstLineText, true, false, FontFactory.CreateFontText(), 20, TextAlignment.CENTER);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.SummarySecondLineText, true, false, FontFactory.CreateFontText(), 20, TextAlignment.CENTER);
            }
        }

        private static void WriteSchoolInfoFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> paragraphs = GetSchoolnfoParagraphs();

            ReportHelper.WriteDataTable(_pdfWriter, paragraphs,
              columns: 2,
              x: left + 20,
              y: bottom + heightBorder - 170,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetSchoolnfoParagraphs()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.SchoolOfText,
                    certificate.SchoolOf, false, false,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }
        }

        private static void WriteEvaluationInfoFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstParagraph = GetEvaluationInfoFirstParagraph();

            ReportHelper.WriteDataTable(_pdfWriter, firstParagraph,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 220,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetEvaluationInfoFirstParagraph()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.DispatcherDateText,
                    certificate.ExpidetionDate.ToString("dd/MM/yy"), false, false,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> secondParagraph = GetEvaluationInfoSecondParagraph();

            ReportHelper.WriteDataTable(_pdfWriter, secondParagraph,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 240,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GetEvaluationInfoSecondParagraph()
            {
                // revisar esto
                // Console.WriteLine($"x = {left + 20} , y = {bottom + heightBorder - 240}");
                // Console.WriteLine($"{left + 20 + ConstantText.EvaluationTypeText.Length + certificate.EvaluationType.Length}");
                int start = 150;
                // Console.WriteLine($"start = {start}");
                int separation = start - ((int)left + 20 + ConstantText.EvaluationTypeText.Length + certificate.EvaluationType.Length);
                // Console.WriteLine($"sep = {separation}");
                List<string> texts = new List<string>() { ConstantText.EvaluationTypeText, ConstantText.NextEvaluationText };
                List<string> values = new List<string>() { certificate.EvaluationType, certificate.NextEvaluation };

                yield return ReportHelper.GetParagraphWithSeparation1(
                    texts, values, false, false, separation,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }
        }

        private static void WritePersonalInfoFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstLine = GePersonalInfoFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 285,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GePersonalInfoFirstLine()
            {
                List<string> texts = new List<string>() { ConstantText.NumberNumberplateText, ConstantText.TomeText, ConstantText.MyFolioText };
                List<string> values = new List<string>() { certificate.NumberNumberplate, certificate.Tomo, certificate.Folio };

                yield return ReportHelper.GetParagraphWithSeparationForThree(
                    texts, values, false, false, 50, 50, 
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> secondLine = GePersonalInfoSecondLine();

            ReportHelper.WriteDataTable(_pdfWriter, secondLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 310,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GePersonalInfoSecondLine()
            {
                string fullName = $"{certificate.Name} {certificate.FirstLastName} {certificate.SecondLastName}";
                List<string> texts = new List<string>() { ConstantText.NameLastNameStudentText, ConstantText.AgeText };
                List<string> values = new List<string>() { fullName, certificate.Age };

                yield return ReportHelper.GetParagraphWithSeparation(
                    texts, values, false, false, 50,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> thirdLine = GePersonalInfoThirdLine();

            ReportHelper.WriteDataTable(_pdfWriter, thirdLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 335,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GePersonalInfoThirdLine()
            {
                List<string> texts = new List<string>() { ConstantText.IdentityCardText, ConstantText.DrivingLicenseText };
                List<string> values = new List<string>() { certificate.IdentityCard, certificate.CardDriver };

                yield return ReportHelper.GetParagraphWithSeparation(
                    texts, values, false, false, 50,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> fourthLine = GePersonalInfoFourthLine();

            ReportHelper.WriteDataTable(_pdfWriter, fourthLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 360,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GePersonalInfoFourthLine()
            {
                List<string> texts = new List<string>() { ConstantText.WorkCenterText, ConstantText.DrivingVehicleText };
                List<string> values = new List<string>() { certificate.WorkCenter, certificate.DrivingVehicle };

                yield return ReportHelper.GetParagraphWithSeparation(
                    texts, values, false, false, 30,
                    FontFactory.CreateFontBoldText(),
                    FontFactory.CreateFontBoldText(),
                    10, TextAlignment.LEFT);
            }
        }

        private static void WriteHeadersSecondPage(Certificate certificate)
        {
            IEnumerable<Paragraph> header1 = GetHeader1();

            ReportHelper.WriteDataTable(_pdfWriter, header1,
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 65,
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
                columns: 1,
                x: left + 20,
                y: bottom + heightBorder - 95,
                width: widthBorder - 10);

            IEnumerable<Paragraph> GetHeader2()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.PerfilText, true, true, FontFactory.CreateFontBoldText(), 15, TextAlignment.CENTER);
            }
        }

        private static void WriteLineSecondPage(Certificate certificate, int height)
        {
            IEnumerable<Paragraph> paragraphs = GetLinesParagraphs();

            ReportHelper.WriteDataTable(_pdfWriter, paragraphs,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - height,
              width: widthBorder - 30);

            IEnumerable<Paragraph> GetLinesParagraphs()
            {
                yield return ReportHelper.GetTextParagraph(
                    new string(ConstantText.EmptyText, 235), false, true, FontFactory.CreateFontText(), 5, TextAlignment.LEFT);
            }
        }

        private static void WriteFooterSecondPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstLine = GePersonalInfoFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 1,
              x: left + 20,
              y: bottom + heightBorder - 375,
              width: widthBorder - 10);

            IEnumerable<Paragraph> GePersonalInfoFirstLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.MainSpecialistText, true, false, FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);
            }

            IEnumerable<Paragraph> secondLine = GePersonalInfoSecondLine();

            ReportHelper.WriteDataTable(_pdfWriter, secondLine,
              columns: 2,
              x: left + 20,
              y: bottom + heightBorder - 390,
              width: widthBorder - 10);

            IEnumerable<Paragraph> GePersonalInfoSecondLine()
            {
                yield return ReportHelper.GetTextParagraph(
                    ConstantText.NameLastNameText, false, false, FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT);

                yield return ReportHelper.GetTextParagraph(
                    ConstantText.FirmText, false, false, FontFactory.CreateFontBoldText(), 10, TextAlignment.CENTER);
            }
        }
    }
}
