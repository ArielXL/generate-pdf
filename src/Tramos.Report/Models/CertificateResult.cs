using Tramos.Shared.Core.Models;

namespace Tramos.Report.Models
{
    public class CertificateResult : CommandResult<byte[]>
    {
        /// <summary>
        /// 
        /// </summary>
        public CertificateResult()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public CertificateResult(byte[] item) : base(item)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="item"></param>
        public CertificateResult(string errorMessage, byte[] item)
            : base(errorMessage, item) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="identity"></param>
        /// <param name="item"></param>
        public CertificateResult(bool isSuccess, object identity, byte[] item)
            : base(isSuccess, identity, item) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="identity"></param>
        /// <param name="errorMessage"></param>
        /// <param name="item"></param>
        public CertificateResult(bool isSuccess, object identity, string errorMessage, byte[] item)
            : base(isSuccess, identity, errorMessage, item) { }
    }
}
