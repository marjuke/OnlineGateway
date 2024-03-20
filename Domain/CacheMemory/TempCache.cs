using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CacheMemory
{
    public class TempCache
    {
        public int stanid { get; set; }
        public DateTime serverreqdate { get; set; }
        public string TransactionId { get; set; }
        public string AccountID { get; set; }
        public string ConversationID { get; set; }
    }
}
