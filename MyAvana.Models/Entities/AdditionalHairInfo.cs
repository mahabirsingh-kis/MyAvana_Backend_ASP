using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class AdditionalHairInfo
    {
        public int Id { get; set; }
        public int HairId { get; set; }
        public string TypeId { get; set; }
        public string TypeDescription { get; set; }
        public string TextureId { get; set; }
        public string TextureDescription { get; set; }
        public string HealthId { get; set; }
        public string HealthDescription { get; set; }
        public string PorosityId { get; set; }
        public string PorosityDescription { get; set; }
        public string ElasticityId { get; set; }
        public string ElasticityDescription { get; set; }
        public string DensityId { get; set; }
        public string DensityDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
