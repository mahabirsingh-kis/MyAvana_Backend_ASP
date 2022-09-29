using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class ScheduleTime
    {
        public int ScheduleTimeId { get; set; }
        public string Time { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
   
    

}
