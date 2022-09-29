using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class Fields
    {
        public string ProductName { get; set; }
        public string TypeFor { get; set; }
        public string ImageName { get; set; }
        public string ActualName { get; set; }
        public string BrandName { get; set; }
        public string ProductDetails { get; set; }
        public string Ingredients { get; set; }
        public string ProductLink { get; set; }
    }

    public class Record
    {
        public string id { get; set; }
        public Fields fields { get; set; }
        public DateTime createdTime { get; set; }
    }

    public class AirTableObject
    {
        public List<Record> records { get; set; }
    }
}
