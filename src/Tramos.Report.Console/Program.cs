using System;
using System.IO;
using Tramos.Report.Pdf.Utils;
using System.Collections.Generic;
using Tramos.Logic.Commands.Certificates;

namespace Tramos.Report.Test
{
    class Program
    {
        private static void GeneratePDFRequalificationCertification(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bytes = new RequalificationCertificationFactory().Create(GetItemDynamics()).Result;

            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            Console.WriteLine("requalification certification pdf done!!!");
        }

        private static void GeneratePDFPotentialAssessment(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bytes = new PotentialAssessmentSummaryFactory().Create(GetItemDynamics()).Result;

            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            Console.WriteLine("potential assessment pdf done!!!");
        }

        private static void GeneratePDFTheoricalCourseCertificate(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bytes = new TheoricalCourseCertificateSummaryFactory().Create(GetItemDynamics()).Result;

            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            Console.WriteLine("theorical course certificate pdf done!!!");
        }

        private static void GeneratePDFRequalificationCourseCertificate(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bytes = new RequalificationCourseCertificateSummaryFactory().Create(GetItemDynamics()).Result;

            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            Console.WriteLine("requalification course certificate pdf done!!!");
        }

        private static void GeneratePDFPsychophysiologicalExam(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bytes = new PsychophysiologicalExamFactory().Create(GetItemDynamics()).Result;

            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            Console.WriteLine("psychophysiological exam carnet pdf done!!!");
        }

        private static List<Certificate> GetItemDynamics()
        {
            return new List<Certificate>
            {
                new Certificate
                {
                    NoIdentification = "123",
                    No = "1", 
                    SchoolOf = "Universidad de La Habana",
                    EvaluationType = "tipo de evaluacion",
                    NextEvaluation = "proxima evaluacion",
                    WorkCenter = "asdasd",
                    DrivingVehicle = "asdasdasdasd",
                    Name = "Alejandro",
                    Date = DateTime.Now,
                    Ptos = "100",
                    Approve = "SI",
                    ValidUntil = DateTime.Now,
                    Group = "123",
                    FirstLastName = "Permuy",
                    Book = "alalalala",
                    SecondLastName = "Chirino",
                    IdentityCard = "123456789",
                    CardDriver = "LCA4312270273",
                    RegistrationNumber = "ED6724",
                    ExpirationDate = DateTime.Now,
                    ExpidetionDate = DateTime.Now,
                    Folio = "56",
                    Tomo = "35",
                    NumberNumberplate = "ABCDEF",
                    Age = "45",
                    ProvincialEEVC = "TEST",
                    ClassroomEEVC = "AULA DIVISION LOGISTICA CT CARIBE",
                    AddressEEVC = "DIVISION LOGISTICA CADENA TIENDAS CARIBE",
                    Points = "70",
                    ExpirationDateEEVC = DateTime.Now,
                    RecalificationCode = "1234567890",
                    Examiner = "123456789",
                    ExperimentationNumber = "45",
                    MunicipalClassroom = "45",
                    ValidDate = DateTime.Now,
                },
                new Certificate
                {
                    NoIdentification = "321",
                    No = "2",
                    ExperimentationNumber = "456",
                    ValidDate = DateTime.Now,
                    SchoolOf = "CUJAE",
                    EvaluationType = "tipo de evaluaciooooooon",
                    Points = "90",
                    Approve = "NO",
                    NextEvaluation = "proxima evaluacion",
                    WorkCenter = "asdasd",
                    DrivingVehicle = "asdasdasdasd",
                    Book = "alalalala",
                    Group = "123",
                    Name = "Alejandro",
                    Date = DateTime.Now,
                    Ptos = "100",
                    ValidUntil = DateTime.Now,
                    FirstLastName = "Permuy",
                    SecondLastName = "Chirino",
                    IdentityCard = "123456789",
                    CardDriver = "LCA4312270273",
                    RegistrationNumber = "ED6724",
                    ExpirationDate = DateTime.Now,
                    ExpidetionDate = DateTime.Now,
                    Folio = "56",
                    Tomo = "35",
                    NumberNumberplate = "ABCDEF",
                    Age = "45",
                    ProvincialEEVC = "TEST",
                    ClassroomEEVC = "G6",
                    AddressEEVC = "TEST",
                    ExpirationDateEEVC = DateTime.Now,
                    MunicipalClassroom = "45",
                    RecalificationCode = "1234567890",
                    Examiner = " "
                },
            };
        }

        static void Main(string[] args)
        {
            GeneratePDFRequalificationCertification("../../pdf/Carnet.pdf");
            GeneratePDFPotentialAssessment("../../pdf/Resumen de Evaluacion.pdf");
            GeneratePDFTheoricalCourseCertificate("../../pdf/Certificado del Curso Teorico.pdf");
            GeneratePDFRequalificationCourseCertificate("../../pdf/Certificado del Curso de Recalificacion.pdf");
            GeneratePDFPsychophysiologicalExam("../../pdf/Carnet de Examen Fisiologico.pdf");
        }
    }
}

