using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseC
{
    public class responsePassbook
    {
        public string? resultCode { get; set; }
        public string? resultDesc { get; set; }
        public string? initiatorID { get; set; }
        public string? accountID { get; set; }
        public string? subID { get; set; }
        public List<Passbook>? passBookData { get; set; }
    }   
    public class Passbook
    {
        private DateTime _dateTimeField;
        public string installmentNumber { get; set; }
        public string transactionPurpose { get; set; }
        //public string paymentTimeAndDate { get; set; }
        public string paymentTimeAndDate
        {
            get => _dateTimeField.ToString("yyyyMMddHHmmss");
            set => _dateTimeField = DateTime.Parse(value);
        }
        public decimal amount { get; set; }
        public string paymentType { get; set; }
        public string senderWallet { get; set; }
        public string transactionStatus { get; set; }
    }
}
