using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class CollaboratedDetailModel
    {
        public List<RecommendedProductsModel> ProductDetailModel { get; set; }
        public List<IngredientDetailModel> IngredientDetailModel { get; set; }
        public List<RegimenDetailModel> RegimenDetailModel { get; set; }
        public List<RecommendedProductsStylingModel> RecommendedProductsStyling { get; set; }

    }

    public class CollaboratedDetailModelLocal
    {
        public List<RecommendedProductsModel> ProductDetailModel { get; set; }
        public List<IngredientDetailModel> IngredientDetailModel { get; set; }
        public List<RegimenDetailModel> RegimenDetailModel { get; set; }
    }

    public class ProductDetailModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ActualName { get; set; }
        public string ImageUrl { get; set; }
        public string PurchaseLink { get; set; }
        public string BrandName { get; set; }
        public string ProductType { get; set; }
        public string ProductTypeName { get; set; }
        public List<ProductsModels> Products { get; set; }
    }
    public class IngredientDetailModel
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
    public class RegimenDetailModel
    {
        public int RegimenId { get; set; }
        public string RegimenName { get; set; }
    }

    public class RecommendedProductModel
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string ActualName { get; set; }
        public string BrandName { get; set; }
        public List<HairTypes> TypeFor { get; set; }
        public string ImageUrl { get; set; }
        public string Ingredients { get; set; }
        public string ProductDetails { get; set; }
        public string PurchaseLink { get; set; }
        public string ProductType { get; set; }
        public List<Ingredients> Ingredient { get; set; }
    }

    public class RecommendedProductModel2
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string ActualName { get; set; }
        public string BrandName { get; set; }
        public string TypeFor { get; set; }
        public string ImageUrl { get; set; }
        public List<Ingredients> Ingredients { get; set; }
        public string ProductDetails { get; set; }
        public string PurchaseLink { get; set; }
        public string ProductType { get; set; }
    }

    public class Ingredients
    {
        public string Name { get; set; }
    }
    public class RecommendedRegimensModel
    {
        public int RegimenId { get; set; }
        public string RegimenName { get; set; }
        public string RegimenTitle { get; set; }
        public string Description { get; set; }
        public List<RegimenStepsModel> RegimenSteps { get; set; }
    }

    public class RegimenStepsModel
    {
        public string RegimenStepPhoto { get; set; }
        public string RegimenStepInstruction { get; set; }
    }

    public class HairTypes
    {
        public string HairType { get; set; }
    }
}
