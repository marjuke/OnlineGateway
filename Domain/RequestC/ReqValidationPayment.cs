using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RequestC
{
    public class ReqValidationPayment: ReqCheckInformation
    {
        public string LastName { get; set; }
        public string PartnerID { get; set; }
        public string IDNumber { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string PartialAmountFlag { get; set; }
        public string PayAmount { get; set; }
        public string ContactNo { get; set; }
        public string ActualAmount { get; set; }
        public string ConversationID { get; set; }
        public string Timestamp { get; set; }
        public string PaymentID { get; set; }
        public string bKashTrxRef { get; set; }
        public string? GatewayID { get; set; }
    }
}
