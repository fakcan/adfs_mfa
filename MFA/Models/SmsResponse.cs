using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFA.Models
{
    public class SmsResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
    }
}
