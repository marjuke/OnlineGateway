using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseC
{
    public class DeviceInfo
    {
        public int id { get; set; }
        [Required]
        public string DeviceID { get; set; }
        [Required]
        public string UserID { get; set; }
    }
}
