using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class EmailTemplate
    {
        [Key]
        public string TemplateCode { get; set; }
        public string TemplateType { get; set; }
        public string TemplateName { get; set; }
        public string SMTPUsername { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SMTPPassword { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string HostName { get; set; }
        public int HostPort { get; set; }
        public bool EnableSSL { get; set; }
        public int TimeOut { get; set; }
    }
}
