using Domain.DatabaseC;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class AppUser:IdentityUser
    {
        public required string UserID { get; set; }
        public string? BranchCode { get; set; }
        public int ChannelListID { get; set; }
        [JsonIgnore]
        public ChannelList ChannelList { get; set; }
        
    }
}
