﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string DisplayName { get; set; }
        public string UserID { get; set; }
    }
}
