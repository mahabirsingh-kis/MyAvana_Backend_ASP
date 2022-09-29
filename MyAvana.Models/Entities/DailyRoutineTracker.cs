using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class DailyRoutineTracker
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string HairStyle { get; set; }
        public string ProfileImage { get; set; }
        public string Notes { get; set; }
        public DateTime TrackTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
    }
}
