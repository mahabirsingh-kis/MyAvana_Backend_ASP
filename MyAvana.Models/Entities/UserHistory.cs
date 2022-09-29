using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class UserHistory
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? AccessTime { get; set; }
        public string UsedCode { get; set; }
    }
}
