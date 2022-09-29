using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IProductsService
    {
        ProductEntityModel SaveProducts(ProductEntityModel productEntity);

        List<ProductsModelList> GetProducts();
        ProductsListings GetAllProducts();
        List<ProductsModelList> GetBrands();
        ProductEntityEditModel GetProductById(ProductEntityEditModel productEntity);
        bool DeleteProduct(ProductEntity productEntity);
        List<ProductTypeCategoryModelList> GetProductsType();
        bool DeleteProductType(ProductType productType);
        bool DeleteProductCategory(ProductType productType);
        ProductType GetProductTypeById(ProductType productType);
        ProductTypeCategoriesList GetProductCategoryById(ProductTypeCategoriesList productType);
        ProductTypeCategoryModel SaveProductType(ProductTypeCategoryModel productType);
        ProductTypeCategoriesList SaveProductCategory(ProductTypeCategoriesList productType);
        List<ProductTypesList> GetProductTypes();
        List<ProductTypeCategoriesList> GetProductsCategoryList();
        IEnumerable<ProductEntityModel> AddProductList(IEnumerable<ProductEntityModel> productData);
        (JsonResult result, bool success, string error) UploadFile(IFormFile file, UserEntity entity);

        List<HairType> GetHairTypesList();
        List<HairChallenges> GetHairChallengesList();
        List<ProductIndicator> GetProductIndicatorsList();
        List<ProductTags> GetProductTagList();
        List<ProductClassification> GetProductClassificationList();
        List<IngedientsEntity> GetIngredientsList();
        List<ProductTypeCategory> GetProductCategory();
    }
}
