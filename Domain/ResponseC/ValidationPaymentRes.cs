using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseC
{
    public class ValidationPaymentRes
    {
        public string id { get; set; }
        public DateTime trDate { get; set; }
        public string referenceNo { get; set; }
        public string trIDNo { get; set; }
        public string status { get; set; }
        public string BranchName { get; set; }
        public string WalletNumber { get; set; }
    }
}
