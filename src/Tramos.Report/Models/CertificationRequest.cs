using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using Tramos.Report.Pdf.Utils;

namespace Tramos.Report.Models
{
    public class CertificationRequest
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public DocumentType DocumentType { get; set; }

        public ICollection<Certificate> Certificates { get; set; }

    }
}
