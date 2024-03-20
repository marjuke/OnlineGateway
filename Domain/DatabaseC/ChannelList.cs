using Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseC
{
    public class ChannelList
    {
        [Key]
        public int Id { get; set; }
        public required string ChannelName { get; set; }

        public List<AppUser> AppUser { get; set; }
        public List<GatewayInfo> GatewayInfo { get; set; }
    }
}
