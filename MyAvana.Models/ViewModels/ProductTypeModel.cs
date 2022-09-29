using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class ProductTypeModel
    {
        public List<ProductType> productType { get; set; }
    }


    public class ProductTypeCategoryModel
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string ProductName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsHair { get; set; }
        public bool? IsRegimens { get; set; }
    }
    public class ProductTypeCategoryModelList
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
