using System;

namespace Tramos.Report.Pdf.Utils
{
    public class Certificate
    {
        // corresponde con 'No. Identificacion' en 'Certificado del Curso de Recalificacion'
        public string NoIdentification { get; set; }

        // corresponde con 'No.'
        public string No { get; set; }

        // corresponde con 'Escuela de' en 'Resumen de Evaluacion'
        public string SchoolOf { get; set; }

        // corresponde con 'Tipo de evaluacion' en 'Resumen de Evaluacion'
        public string EvaluationType { get; set; }

        // corresponde con 'Proxima evaluacion' en 'Resumen de Evaluacion'
        public string NextEvaluation { get; set; }

        // corresponde con 'No. Matricula' en 'Resumen de Evaluacion'
        public string NumberNumberplate { get; set; }

        // corresponde con 'Centro de trabajo' en 'Resumen de Evaluacion'
        public string WorkCenter { get; set; }
        
        // corresponde con 'Vehiculo que conduce' en 'Resumen de Evaluacion'
        public string DrivingVehicle { get; set; }

        // corresponde con 'Nombre'
        public string Name { get; set; }

        // corresponde con 'Primer Apellido'
        public string FirstLastName { get; set; }

        // corresponde con 'Segundo Apellido'
        public string SecondLastName { get; set; }

        // corresponde con 'C.I'
        public string IdentityCard { get; set; }

        // corresponde con 'Licencia de conduccion'
        public string CardDriver { get; set; }

        // corresponde con 'Libro' en 'Certificado del Curso Teorico' y 'Certificado del Curso de Recalificacion'
        public string Book { get; set; }

        // corresponde con 'Puntos' en 'Certificado del Curso de Recalificacion'
        public string Points { get; set; }

        // corresponde con 'Aprobado' en 'Certificado del Curso de Recalificacion'
        public string Approve { get; set; }

        // corresponde con 'Examinador' en 'Certificado del Curso de Recalificacion'
        public string Examiner { get; set; }

        // corresponde con 'No. Expediente' en 'Certificado del Curso de Recalificacion'
        public string ExperimentationNumber { get; set; }

        // corresponde con 'Edad'
        public string Age { get; set; }

        // corresponde con 'Numero de matricula' en 'PsychophysiologicalExam'
        public string RegistrationNumber { get; set; }

        // corresponde con 'Fecha/vencimiento' en 'PsychophysiologicalExam'
        public DateTime ExpirationDate { get; set; }

        // corresponde con 'Tomo'
        public string Tomo { get; set; }

        // corresponde con 'Fecha' en 'Certificado del Curso Teorico'
        public DateTime Date { get; set; }

        // corresponde con 'Ptos' en 'Certificado del Curso Teorico'
        public string Ptos { get; set; }

        // corresponde con 'Codigo Carne de recalificacion' en 'Certificado del Curso Teorico'
        public string RecalificationCode { get; set; }

        // corresponde con 'Fecha de expedido'
        public DateTime ExpidetionDate { get; set; }

        // corresponde con 'Fecha valido (un año natural)' en 'Certificado del Curso de Recalificacion'
        public DateTime ValidDate { get; set; }

        // corresponde con 'Valido hasta (dos años naturales)' en 'Certificado del Curso Teorico'
        public DateTime ValidUntil { get; set; }

        // corresponde con 'Grupo' en 'Certificado del Curso Teorico'
        public string Group { get; set; }

        // corresponde con 'Folio'
        public string Folio { get; set; }

        // corresponde con 'E.E.V.C Provincial' en 'Certificado del Curso de Recalificacion'
        public string ProvincialEEVC { get; set; }

        // corresponde con 'Direccion' en 'Carnet'
        public string AddressEEVC { get; set; }

        // corresponde con 'Aula' en 'Carnet'
        public string ClassroomEEVC { get; set; }

        // corresponde con 'Fecha/vencimiento' en 'Carnet'
        public DateTime ExpirationDateEEVC { get; set; }

        // corresponde con 'Aula municipal' en 'Certificado del Curso de Recalificacion'
        public string MunicipalClassroom { get; set; }
    }
}
