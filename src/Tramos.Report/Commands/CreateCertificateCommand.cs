using Tramos.Report.Models;
using Tramos.Shared.Core.Commands;

namespace Tramos.Report.Commands
{
    public class CreateCertificateCommand : Command<CertificationRequest, CertificateResult>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateCertificateCommand()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public CreateCertificateCommand(CertificationRequest request) : base(request)
        {
        }
    }
}
