using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseC
{
    public class ChannelCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CodeName { get; set; }
        public int channelId { get; set; }
        public bool IsActive { get; set; }

    }
}
