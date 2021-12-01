using System;
using System.IO;
using System.Linq;
using iText.IO.Image;
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
    internal class PotentialAssessmentSummary
    {
        private static float heightBorder;
        private static float widthBorder;
        private static float left;
        // private static float right;
        private static float bottom;

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
            _pdfWriter = new PdfWriterManager(stream, PageSize.A3);
            
            List<Certificate> certificatesList = certificates.ToList();
            for(int i = 0; i < certificatesList.Count; i++)
            {
                InitializeStaticField();
                GenerateReportFirstPage(certificatesList[i]);
                UpdateStaticField(-550);
                GenerateReportSecondPage(certificatesList[i]);
                if(i != certificatesList.Count - 1)
                    _pdfWriter.AddNewPage();
            }

            _pdfWriter.Close();
        }

        private static void InitializeStaticField()
        {
            heightBorder = 400;
            widthBorder = 700;
            left = 45;
            // right = 40;
            bottom = 660;
        }

        private static void UpdateStaticField(int displacement)
        {
            bottom += displacement;
        }

        private static void GenerateReportFirstPage(Certificate certificate)
        {
            WriteBorders(
               rectangleLeft: left,
               rectangleTop: bottom,
               rectangleRight: widthBorder,
               rectangleBottom: heightBorder,
               lineWidth: 3);

            WriteImages(left + 5, bottom + 335, 50);

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
              columns: 2,
              x: left + 20,
              y: bottom + heightBorder - 240,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GetEvaluationInfoSecondParagraph()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.EvaluationTypeText, 
                    certificate.EvaluationType, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );

                yield return ReportHelper.GetParagraph(
                    ConstantText.NextEvaluationText, 
                    certificate.NextEvaluation, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }
        }

        private static void WritePersonalInfoFirstPage(Certificate certificate)
        {
            IEnumerable<Paragraph> firstLine = GePersonalInfoFirstLine();

            ReportHelper.WriteDataTable(_pdfWriter, firstLine,
              columns: 3,
              x: left + 20,
              y: bottom + heightBorder - 285,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GePersonalInfoFirstLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.NumberNumberplateText, 
                    certificate.NumberNumberplate, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );

                yield return ReportHelper.GetParagraph(
                    ConstantText.TomeText, 
                    certificate.Tomo, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );

                yield return ReportHelper.GetParagraph(
                    ConstantText.FolioText, 
                    certificate.Folio, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, 
                    TextAlignment.LEFT
                );
            }

            IEnumerable<Paragraph> secondLine = GePersonalInfoSecondLine();

            ReportHelper.WriteDataTable(_pdfWriter, secondLine,
              columns: 2,
              x: left + 20,
              y: bottom + heightBorder - 310,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GePersonalInfoSecondLine()
            {
                string fullName = $"{certificate.Name} {certificate.FirstLastName} {certificate.SecondLastName}";
                
                yield return ReportHelper.GetParagraph(
                    ConstantText.NameLastNameStudentText, fullName, 
                    false, false, FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, TextAlignment.LEFT
                );

                yield return ReportHelper.GetParagraph(
                    ConstantText.AgeText, certificate.Age, 
                    false, false, FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 10, TextAlignment.CENTER
                );
            }

            IEnumerable<Paragraph> thirdLine = GePersonalInfoThirdLine();

            ReportHelper.WriteDataTable(_pdfWriter, thirdLine,
              columns: 2,
              x: left + 20,
              y: bottom + heightBorder - 335,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GePersonalInfoThirdLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.IdentificationNumberText, 
                    certificate.IdentityCard, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 
                    10, TextAlignment.LEFT
                );

                yield return ReportHelper.GetParagraph(
                    ConstantText.DrivingLicenseText, 
                    certificate.CardDriver, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 
                    10, TextAlignment.LEFT
                );
            }

            IEnumerable<Paragraph> fourthLine = GePersonalInfoFourthLine();

            ReportHelper.WriteDataTable(_pdfWriter, fourthLine,
              columns: 2,
              x: left + 20,
              y: bottom + heightBorder - 360,
              width: widthBorder - 40);

            IEnumerable<Paragraph> GePersonalInfoFourthLine()
            {
                yield return ReportHelper.GetParagraph(
                    ConstantText.WorkCenterText, 
                    certificate.WorkCenter, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 
                    10, TextAlignment.LEFT
                );

                yield return ReportHelper.GetParagraph(
                    ConstantText.DrivingVehicleText, 
                    certificate.DrivingVehicle, false, false, 
                    FontFactory.CreateFontBoldText(), 
                    FontFactory.CreateFontBoldText(), 
                    10, TextAlignment.LEFT
                );
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
