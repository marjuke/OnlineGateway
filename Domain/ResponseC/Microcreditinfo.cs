using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseC
{
    public class Microcreditinfo
    {
        public decimal SlNo { get; set; }
        public string AreaCode { get; set; }
        public string CustIDNO { get; set; }
        public string CustName { get; set; }
        public string SBAccNo { get; set; }
        public string LoanNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Account_Sub_SubCode { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }
        public decimal Balance { get; set; }
        public decimal Installment { get; set; }
        public decimal ServiceChrg { get; set; }
        public decimal Receibale_Income { get; set; }
        public string LaonSchemeCode { get; set; }
        public decimal Status { get; set; }
        public string Tel { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public decimal AmtOfInstment { get; set; }
    }
}
