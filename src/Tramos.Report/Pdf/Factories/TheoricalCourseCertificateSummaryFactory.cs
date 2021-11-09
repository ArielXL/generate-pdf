using System;
using Tramos.Report.Pdf;
using System.Threading.Tasks;
using Tramos.Report.Pdf.Utils;
using System.Collections.Generic;

namespace Tramos.Logic.Commands.Certificates
{
    public class TheoricalCourseCertificateSummaryFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="certificates"></param>
        /// <returns></returns>
        public async Task<byte[]> Create(IEnumerable<Certificate> certificates)
        {
            try
            {
                return await TheoricalCourseCertificateSummary.TryGeneratePdf(certificates);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
