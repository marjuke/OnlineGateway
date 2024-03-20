using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseC
{
    public class RoutingServerList
    {
        [Key]
        public int RID { get; set; }
        public required string BranchCode { get; set; }
        public required string Ip { get; set; }
    }
}
