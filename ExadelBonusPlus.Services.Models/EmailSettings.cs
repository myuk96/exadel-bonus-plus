using System;
using System.Collections.Generic;
using System.Text;

namespace ExadelBonusPlus.Services.Models
{
    public class EmailSettings
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPServerPort { get; set; }
    }
}
