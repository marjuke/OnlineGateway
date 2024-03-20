using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RequestC
{
    public class ReqPaymentConfirmation
    {
        public string bKashCompletionTime { get; set; }
        public string PartnerID { get; set; }
        public string ParentID { get; set; }
        public string bKashTrxRef { get; set; }
        public string PartnerIntentRef { get; set; }
        public string ActualTRXTime { get; set; }
        public string InitiatorID { get; set; }
    }
}
