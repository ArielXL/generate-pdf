# Generador de PDF

### Historial de Cambios:

1. En una misma fila se alinean columnas (esto logré hacerlo que lo tenía pendiente).
2. Bajé un poco la foto tramos como me pediste.
3. La clase `FirstCertificateSummary` pasó a llamarse `TheoricalCourseCertificateSummary`.
4. La clase `FirstCertificateSummaryFactory` pasó a llamarse `TheoricalCourseCertificateSummaryFactory`.
5. La clase `SecondCertificateSummary` pasó a llamarse `RequalificationCourseCertificateSummary`.
6. La clase `RequalificationCourseCertificateSummaryFactory` pasó a llamarse `RequalificationCourseCertificateSummaryFactory`.
7. Los archivos pdf están en la carpeta `pdf/`.
8. Agrego una documentación de la clase `Certificate` con sus referencias tanto en el código como en el README.md.

### Campos de la clase `Certificate` y sus referencias:

* *string No* : corresponde con *No.* en varios pdf.
* *string SchoolOf* : corresponde con *Escuela de* en *Resumen de Evaluación*.
* *string EvaluationType* : corresponde con *Tipo de evaluación* en *Resumen de Evaluación*.
* *string NextEvaluation* : corresponde con *Próxima evaluación* en *Resumen de Evaluación*.
* *string NumberNumberplate* : corresponde con *No. Matricula* en *Resumen de Evaluación*.
* *string WorkCenter* : corresponde con *Centro de trabajo* en *Resumen de Evaluación*.
* *string DrivingVehicle* : corresponde con *Vehículo que conduce* en *Resumen de Evaluación*.
* *string Name* : corresponde con *Nombre* en *Carnet*, pero se usa en otros pdf para completar el nombre completo.
* *string FirstLastName* : corresponde con *Primer Apellido* en *Carnet*, pero se usa en otros pdf para completar el nombre completo.
* *string SecondLastName* : corresponde con *Segundo Apellido* en *Carnet*, pero se usa en otros pdf para completar el nombre completo.
* *string IdentityCard* : corresponde con *C.I*.
* *string CardDriver* : corresponde con *Licencia de conducción* en *Carnet* y *Certificado del Curso Teórico*.
* *string Book* : corresponde con *Libro* en *Certificado del Curso Teórico* y *Certificado del Curso de Recalificación*.
* *string Points* : corresponde con *Puntos* en *Certificado del Curso de Recalificación*.
* *string Approve* : corresponde con *Aprobado* en *Certificado del Curso de Recalificación*.
* *string Examiner* : corresponde con *Examinador* en *Certificado del Curso de Recalificación*.
* *string ExperimentationNumber* : corresponde con *No. Expediente* en *Certificado del Curso de Recalificación*.
* *string Age* : corresponde con 'Edad' en varios pdf.
* *string RegistrationNumber* : corresponde con *Número de matrícula* en *PsychophysiologicalExam*.
* *DateTime ExpirationDate* : corresponde con *Fecha/vencimiento* en *PsychophysiologicalExam*, este campo yo no lo uso en lo que hice.
* *string Tomo* : corresponde con *Tomo* en varios pdf.
* *DateTime Date* : corresponde con *Fecha* en *Certificado del Curso Teórico*.
* *string Ptos* : corresponde con *Ptos* en *Certificado del Curso Teórico*.
* *string RecalificationCode* : corresponde con *Código Carné de Recalificación* en *Certificado del Curso Teórico*.
* *DateTime ExpidetionDate* : corresponde con *Fecha de expedido*.
* *DateTime ValidDate* : corresponde con *Fecha válido (un año natural)* en *Certificado del Curso de Recalificación*.
* *DateTime ValidUntil* : corresponde con *Válido hasta (dos años naturales)* en *Certificado del Curso Teórico*.
* *string Group* : corresponde con *Grupo* en *Certificado del Curso Teórico*.
* *string Folio* : corresponde con *Folio* en varios pdf.
* *string ProvincialEEVC* : corresponde con *E.E.V.C Provincial* en *Certificado del Curso de Recalificación*.
* *string AddressEEVC* : corresponde con *Dirección* en *Carnet*.
* *string ClassroomEEVC* : corresponde con *Aula* en *Carnet*.
* *DateTime ExpirationDateEEVC* : corresponde con *Fecha/vencimiento* en *Carnet*, yo no lo uso.
* *string MunicipalClassroom* : corresponde con *Aula municipal* en *Certificado del Curso de Recalificación*.
