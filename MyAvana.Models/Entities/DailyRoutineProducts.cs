using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class DailyRoutineProducts
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int RoutineTrackerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
        [ForeignKey("RoutineTrackerId")]
        public virtual DailyRoutineTracker DailyRoutineTracker { get; set; }
    }
}
