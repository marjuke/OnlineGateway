using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RequestC
{
    public class ReqPassbook
    {
        public int Version { get; set; }

        public string? CommandID { get; set; }
        public string? OriginatorConversationID { get; set; }
        public DateTime Timestamp { get; set; }
        public string? PartnerID { get; set; }
        public string? ParentID { get; set; }
        public string? InitiatorID { get; set; }
        public string? AccountID { get; set; }
        public string? StartMonth { get; set; }
        public string? EndMonth { get; set; }
    }
}
