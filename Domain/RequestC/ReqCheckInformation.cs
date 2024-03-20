using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RequestC
{
    public class ReqCheckInformation
    {
        public string InitiatorID { get; set; }
        //Mandatory
        public string channelCode { get; set; }
        //Mandatory
        public string PartnerID { get; set; }
        public string ParentID { get; set; }
        public string RequesterMSISDN { get; set; }
        public string InquiryMode { get; set; }
        //Mandatory
        public string InquiryType { get; set; }
        //Mandatory
        public string AccountID { get; set; }
    }
}
