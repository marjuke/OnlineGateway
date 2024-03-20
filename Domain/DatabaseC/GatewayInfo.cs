using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseC
{
    public class GatewayInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? GatewayId { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StanId { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime ServerReqDate { get; set; }
        public string? Version { get; set; }
        public string? Command { get; set; }
        public string? LoginID { get; set; }
        public string? Password { get; set; }
        public string? ConversationID { get; set; }
        public DateTime Timestamp { get; set; }
        //Request Response date time
        public DateTime ChannelReqDateTime { get; set; }
        public DateTime SysChannelReqDateTime { get; set; }
        public DateTime SysSpReqDateTime { get; set; }
        public DateTime SpResDateTime { get; set; }
        public DateTime SysSpResDateTime { get; set; }
        public DateTime SysChannelResDateTime { get; set; }

        public string? ResCode { get; set; }
        public string? ResponseDesc { get; set; }
        public required string InitiatorID { get; set; }
        public string? InitiatorKYC { get; set; }
        public string? ChannelCode { get; set; }


        public string? PartnerID { get; set; }
        public int? PayMode { get; set; }
        public int? PayType { get; set; }
        public string? TrxRef { get; set; }
        public required string BranchCode { get; set; }
        public string? RequesterMSISDN { get; set; }
        public int? InquiryMode { get; set; }
        public int? InquiryType { get; set; }
        //MemberID
        public string? AccountID { get; set; }
        public string? SubID { get; set; }
        public string? PaymentID { get; set; }
        public string? PaymentContractNo { get; set; }
        public string? ActualAmount { get; set; }
        public string? PayAmount { get; set; }
        public string? PartialAmountFlag { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMyyyy}")]
        public string? StartMonth { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMyyyy}")]
        public string? EndMonth { get; set; }
        public string? ContactNo { get; set; }
        
        public string? FinAppResCode { get; set; }
        public string? SystemID { get; set; }
        public string? Event { get; set; }
        public string? Note { get; set; }

        public DateTime? PaymentConfirmReqDate { get; set; }
        public DateTime? PaymentConfirmResDate { get; set; }
        public string? SystemTrID { get; set; }
        public string? PaymentConfirmResCode { get; set; }
        public string? PaymentConfirmResDesc { get; set; }
        public string? PaymentConfirmResNote { get; set; }

        public int ChannelListId { get; set; }
        public ChannelList ChannelList { get; set; }
    }
}
