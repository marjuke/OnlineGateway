using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseC
{
    public class OtherInvestmentinfo
    {
        public string SlNo { get; set; }
        public DateTime Last_Paid_Date { get; set; }
        public string AreaCode { get; set; }
        public string CustIDNO { get; set; }
        public string CustName { get; set;}
        public string SBAccNo { get; set;}
        public string LoanNo { get;set;}
        public DateTime IssueDate { get; set;} 
        public DateTime ExpireDate { get; set;}
        public string Account_Sub_SubCode { get; set;}
        public decimal Dr { get; set;}
        public decimal Cr { get; set;}
        public decimal Balance { get; set;}
        public decimal ServiceChrg { get; set;}
        public string Tel { get; set;}
        public string BranchName { get; set;}
        public string BranchCode { get; set;}
        public decimal AmtOfInstment { get; set;}
    }
}
