using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCodeGenLib.DTO
{
    public class AdUserDto
    {
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public bool IsAdmin { get; set; }
    }
}
