using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGeApp.Core.Config
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; }

        public string SenderEmail { get; set; }

        public string SenderName { get; set; }
    }
}
