using MyAvana.Models.Entities;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class ProductsModel
    {
        //public List<ProductEntity> product { get; set; }
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
        public int? ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public decimal Price { get; set; }
        public string ProductIndicator { get; set; }
        public string HairChallenges { get; set; }
        public string ProductTags { get; set; }
        public string ProductClassification { get; set; }
    }

    public class ProductTypeEntity
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ParentId { get; set; }
        public string ProductParent { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ProductTypesList
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int? ParentId { get; set; }
        public string CategoryName { get; set; }
    }

    public class ProductTypeCategoriesList
    {
        public int ProductTypeId { get; set; }
        public string CategoryName { get; set; }
        public bool? IsHair { get; set; }
        public bool? IsRegimen { get; set; }
    }

    public class ProductEntityModel
    {
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
        public int? TypeId { get; set; }

        public string ProductType { get; set; }
        public decimal Price { get; set; }
        public string ProductClassification { get; set; }
        public string ProductIndicator { get; set; }
        public string HairChallenges { get; set; }
        public string ProductTags { get; set; }
    }

    public class ProductEntityEditModel
    {
        public int Id { get; set; }
        public Guid guid { get; set; }
        public string ProductName { get; set; }
        public string ActualName { get; set; }
        public string BrandName { get; set; }
        public string ImageName { get; set; }
        public string Ingredients { get; set; }
        public string ProductDetails { get; set; }
        public string ProductLink { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ProductTypesId { get; set; }
        public int? TypeId { get; set; }

        public string ProductType { get; set; }
        public decimal Price { get; set; }
        public string ActualPrice { get; set; }
        public string DecimalPrice { get; set; }
        public string ProductClassification { get; set; }
        public string ProductIndicator { get; set; }
        public string HairChallenges { get; set; }
        public string ProductTags { get; set; }

        public List<ProductCommon> productCommons { get; set; }
    }

    public class ProductsListings
    {
        public List<ProductsModelList> ProductsModelLists { get; set; }
        public List<ProductTypeCategory> TypeCategories { get; set; }
        public List<ProductTypesList> ProductTypes { get; set; }
        public List<ProductClassification> ProductClassifications { get; set; }
        public List<HairChallenges> HairChallenges { get; set; }
        public string Message { get; set; }
    }
    public class ProductsModelList
    {
        //public List<ProductEntity> product { get; set; }
        public int Id { get; set; }
        public Guid guid { get; set; }
        public string ProductName { get; set; }
        public string ActualName { get; set; }
        public string BrandName { get; set; }
        public List<HairType> HairType { get; set; }
        public string ImageName { get; set; }
        public string Ingredients { get; set; }
        public string ProductDetails { get; set; }
        public string ProductLink { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public decimal Price { get; set; }
        public List<ProductIndicator> ProductIndicate { get; set; }
        public List<HairChallenges> HairChallenge { get; set; }
        public List<ProductTags> ProductTag { get; set; }
        public List<ProductClassification> ProductClassificatio { get; set; }
    }
}
