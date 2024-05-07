using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class RegisterDTO
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string DiaplayName { get; set; } 
        public string BranchCode { get; set; }  
        public int ChannelID { get; set; }  
    }
}
