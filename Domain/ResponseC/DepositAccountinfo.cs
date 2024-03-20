using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseC
{
    public class DepositAccountinfo
    {
        public string CustIDNO { get; set; }
        public string CustAccNo { get; set; }
        public string CustName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Tel { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdraw { get; set; }
        public decimal Balance { get; set; }
        public string SubDepositName { get; set; }
        public string Duration { get; set; }
        public DateTime ExpierDate { get; set; }
        public DateTime PDate { get; set; }
        public string DepositCodeNo { get; set; }
        public string SubDepositCodeNo { get; set; }
        public string MemStatus { get; set; }
        public decimal DepositInstAmt { get; set; }
        public string AreaCode { get; set; }
    }
}
