using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAvanaApi.Models.Entities
{
    public class GenericSetting
    {
        [Key, Column(Order = 0)]
        public Guid SettingID { get; set; }
        [Key, Column(Order = 1)]
        public string AdminAccountId { get; set; }
        [Key, Column(Order = 2)]
        public string SettingName { get; set; }
        [Key, Column(Order = 3)]
        public string SubSettingName { get; set; }

        [StringLength(20)]
        public string DefaultTextValue20_1 { get; set; }
        [StringLength(20)]
        public string DefaultTextValue20_2 { get; set; }

        [StringLength(50)]
        public string DefaultTextValue50_1 { get; set; }
        [StringLength(50)]
        public string DefaultTextValue50_2 { get; set; }

        [StringLength(100)]
        public string DefaultTextValue100_1 { get; set; }
        [StringLength(100)]
        public string DefaultTextValue100_2 { get; set; }

        [StringLength(250)]
        public string DefaultTextValue250_1 { get; set; }
        [StringLength(250)]
        public string DefaultTextValue250_2 { get; set; }

        public string DefaultTextMax { get; set; }
        public string DefaultTextMax1 { get; set; }
        public string DefaultTextMax2 { get; set; }
        public string DefaultTextMax3 { get; set; }
        public string DefaultTextMax4 { get; set; }

        public int DefalutInteger1 { get; set; }
        public int DefalutInteger2 { get; set; }
        public int DefalutInteger3 { get; set; }
        public int DefalutInteger4 { get; set; }
        public int DefalutInteger5 { get; set; }


        public decimal DefaultDecimal1 { get; set; }
        public decimal DefaultDecimal2 { get; set; }
        public decimal DefaultDecimal3 { get; set; }
        public decimal DefaultDecimal4 { get; set; }
        public decimal DefaultDecimal5 { get; set; }

        public DateTime? DefaultDateTime1 { get; set; }
        public DateTime? DefaultDateTime2 { get; set; }
        public DateTime? DefaultDateTime3 { get; set; }
        public DateTime? DefaultDateTime4 { get; set; }
        public DateTime? DefaultDateTime5 { get; set; }

        public bool DefaultBool1 { get; set; }
        public bool DefaultBool2 { get; set; }
        public bool DefaultBool3 { get; set; }
        public bool DefaultBool4 { get; set; }
        public bool DefaultBool5 { get; set; }
    }
}
