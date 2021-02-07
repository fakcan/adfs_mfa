using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFA.Models
{
    public class SmsRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientIp { get; set; }
        public string RegisteredNumber { get; set; }
        public string SmsText { get; set; }
    }
}
