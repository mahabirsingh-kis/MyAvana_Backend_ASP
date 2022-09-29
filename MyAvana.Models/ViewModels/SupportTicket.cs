using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class SupportTicket
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string contentType { get; set; }
        public string fileName { get; set; }
        public string fileData { get; set; }
        public bool HasAttachment { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
    }
}
