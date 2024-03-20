using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseC
{
    public class IPListInfo
    {
        public int Id { get; set; }
        public required string IP { get; set; }
        public string? Description { get; set; }
        public int? ChannelCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
