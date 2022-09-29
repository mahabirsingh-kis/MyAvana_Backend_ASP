using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.ViewModels
{
    public class Identity
    {
        public string value { get; set; }
        public string type { get; set; }
        public object timestamp { get; set; }
        public bool isPrimary { get; set; }
    }

    public class IdentityProfile
    {
        public int vid { get; set; }
        public List<Identity> identity { get; set; }
        public List<object> linkedVid { get; set; }
        public bool isContact { get; set; }
        public long savedAtTimestamp { get; set; }
    }

    public class HubSpotException
    {
        public string message { get; set; }
        public IdentityProfile identityProfile { get; set; }
        public string status { get; set; }
        public string correlationId { get; set; }
        public string error { get; set; }
        public string requestId { get; set; }
    }
}
