using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class HairObservation
    {
        public int Id { get; set; }
        [ForeignKey("HairProfileId")]
        public int HairProfileId { get; set; }
        [ForeignKey("ObservationId")]
        public int? ObservationId { get; set; }

        [ForeignKey("ObsElasticityId")]
        public int? ObsElasticityId { get; set; }
        [ForeignKey("ObsChemicalProductId")]
        public int? ObsChemicalProductId { get; set; }
        [ForeignKey("ObsPhysicalProductId")]
        public int? ObsPhysicalProductId { get; set; }
        [ForeignKey("ObsDamageId")]
        public int? ObsDamageId { get; set; }
        [ForeignKey("ObsBreakageId")]
        public int? ObsBreakageId { get; set; }
        [ForeignKey("ObsSplittingId")]
        public int? ObsSplittingId { get; set; }


        public bool IsTopLeft { get; set; }
        public bool IsTopRight { get; set; }
        public bool IsBottomLeft { get; set; }
        public bool IsBottomRight { get; set; }
        public bool IsCrown { get; set; }

        public virtual HairProfile HairProfile { get; set; }
        public virtual Observation Observation { get; set; }
        public virtual ObsElasticity ObsElasticity { get; set; }
        public virtual ObsChemicalProducts ObsChemicalProducts { get; set; }
        public virtual ObsPhysicalProducts ObsPhysicalProducts { get; set; }
        public virtual ObsDamage ObsDamage { get; set; }
        public virtual ObsBreakage ObsBreakage { get; set; }
        public virtual ObsSplitting ObsSplitting { get; set; }
    }
}
