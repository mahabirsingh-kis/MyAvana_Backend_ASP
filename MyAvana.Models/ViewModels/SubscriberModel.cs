using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class SubscriberModel
    {

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ProviderName { get; set; }
        public bool IsActive { get; set; }
    }
}
