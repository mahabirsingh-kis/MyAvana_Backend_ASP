using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }
        public Guid guid { get; set; }
        public string ProductName { get; set; }
        public string ActualName { get; set; }
        public string BrandName { get; set; }
        public string TypeFor { get; set; }
        public string ImageName { get; set; }
        public string Ingredients { get; set; }
        public string ProductDetails { get; set; }
        public string ProductLink { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ProductTypesId { get; set; }

        [ForeignKey("TypeId")]
        public int? TypeId { get; set; }

        public virtual ProductType ProductTypes { get; set; }

        public decimal Price { get; set; }

        public string Product { get; set; }
        public string ProductIndicator { get; set; }
        public string HairChallenges { get; set; }
        public string ProductTags { get; set; }
    }
}
