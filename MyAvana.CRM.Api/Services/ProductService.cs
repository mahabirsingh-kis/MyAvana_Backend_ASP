using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class ProductService : IProductsService
    {
        private readonly AvanaContext _context;
        public ProductService(AvanaContext avanaContext)
        {
            _context = avanaContext;
        }

        public bool DeleteProduct(ProductEntity productEntity)
        {
            try
            {
                var objProduct = _context.ProductEntities.FirstOrDefault(x => x.guid == productEntity.guid);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }

        public ProductEntityEditModel GetProductById(ProductEntityEditModel productEntity)
        {
            try
            {
                ProductEntityEditModel product = (from prd in _context.ProductEntities
                                                  where prd.guid == productEntity.guid
                                                  && prd.IsActive == true
                                                  select new ProductEntityEditModel()
                                                  {
                                                      guid = prd.guid,
                                                      ProductTypesId = prd.ProductTypesId,
                                                      ProductName = prd.ProductName,
                                                      ActualName = prd.ActualName,
                                                      BrandName = prd.BrandName,
                                                      ImageName = prd.ImageName,
                                                      Ingredients = prd.Ingredients,
                                                      ProductDetails = prd.ProductDetails,
                                                      ProductLink = prd.ProductLink,
                                                      ActualPrice = prd.Price.ToString().Substring(0, prd.Price.ToString().IndexOf('.')),
                                                      DecimalPrice = prd.Price.ToString().Substring(prd.Price.ToString().IndexOf('.') + 1),
                                                      IsActive = prd.IsActive,
                                                      CreatedOn = prd.CreatedOn,

                                                      productCommons = _context.ProductCommons.Where(x => x.ProductEntityId == prd.Id).ToList()

                                                  }).FirstOrDefault();

                return product;
                //ProductEntity productEntityModel = _context.ProductEntities.Where(x => x.guid == productEntity.guid).FirstOrDefault();
                //return productEntityModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>


        //public List<ProductsModelList> GetProducts()
        //{
        //    try
        //    {
        //        List<ProductsModelList> lstProductModel = _context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
        //            Select(x => new ProductsModelList
        //            {
        //                Id = x.Id,
        //                guid = x.guid,
        //                ProductName = x.ProductName,
        //                ActualName = x.ActualName,
        //                BrandName = x.BrandName,
        //                HairType = _context.ProductCommons.Include(h => h.HairType).Where(p => p.ProductEntityId == x.Id && p.HairTypeId != null).Select(d => new HairType
        //                {
        //                    Description = d.HairType.Description
        //                }).ToList(),
        //                ImageName = x.ImageName,
        //                Ingredients = x.Ingredients,
        //                ProductDetails = x.ProductDetails,
        //                ProductLink = x.ProductLink,
        //                IsActive = x.IsActive,
        //                CreatedOn = x.CreatedOn,
        //                ProductTypeId = x.ProductTypesId,
        //                ProductType = x.ProductTypes.ProductName,
        //                ParentId = x.ProductTypes.ParentId,
        //                ParentName = _context.ProductTypes.Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
        //                Price = x.Price,
        //                HairChallenge = _context.ProductCommons.Include(h => h.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(d => new HairChallenges
        //                {
        //                    Description = d.HairChallenges.Description
        //                }).ToList(),
        //                ProductIndicate = _context.ProductCommons.Include(h => h.ProductIndicator).Where(p => p.ProductEntityId == x.Id && p.ProductIndicatorId != null).Select(d => new ProductIndicator
        //                {
        //                    Description = d.ProductIndicator.Description
        //                }).ToList(),
        //                ProductTag = _context.ProductCommons.Include(h => h.ProductTags).Where(p => p.ProductEntityId == x.Id && p.ProductTagsId != null).Select(d => new ProductTags
        //                {
        //                    Description = d.ProductTags.Description
        //                }).ToList(),
        //                ProductClassificatio = _context.ProductCommons.Include(h => h.ProductClassification).Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(d => new ProductClassification
        //                {
        //                    Description = d.ProductClassification.Description
        //                }).ToList()
        //            }).ToList();

        //        return lstProductModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        //public List<ProductsModelList> GetProducts()
        //{
        //    try
        //    {
        //        var productCommons = _context.ProductCommons.Include(x => x.HairChallenges).Include(y => y.ProductIndicator).Include(z => z.HairType).
        //            Include(h => h.ProductTags).Include(h => h.ProductClassification).ToList();
        //        List<ProductsModelList> lstProductModel = _context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
        //            Select(x => new ProductsModelList
        //            {
        //                Id = x.Id,
        //                guid = x.guid,
        //                ProductName = x.ProductName,
        //                ActualName = x.ActualName,
        //                BrandName = x.BrandName,
        //                HairType = productCommons.Where(p => p.ProductEntityId == x.Id && p.HairTypeId != null).Select(d => new HairType
        //                {
        //                    Description = d.HairType.Description
        //                }).ToList(),
        //                ImageName = x.ImageName,
        //                Ingredients = x.Ingredients,
        //                ProductDetails = x.ProductDetails,
        //                ProductLink = x.ProductLink,
        //                IsActive = x.IsActive,
        //                CreatedOn = x.CreatedOn,
        //                ProductTypeId = x.ProductTypesId,
        //                ProductType = x.ProductTypes.ProductName,
        //                ParentId = x.ProductTypes.ParentId,
        //                ParentName = _context.ProductTypes.Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
        //                Price = x.Price,
        //                HairChallenge = productCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(d => new HairChallenges
        //                {
        //                    Description = d.HairChallenges.Description
        //                }).ToList(),
        //                ProductIndicate = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductIndicatorId != null).Select(d => new ProductIndicator
        //                {
        //                    Description = d.ProductIndicator.Description
        //                }).ToList(),
        //                ProductTag = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductTagsId != null).Select(d => new ProductTags
        //                {
        //                    Description = d.ProductTags.Description
        //                }).ToList(),
        //                ProductClassificatio = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(d => new ProductClassification
        //                {
        //                    Description = d.ProductClassification.Description
        //                }).ToList()
        //            }).ToList();

        //        return lstProductModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        public List<ProductsModelList> GetProducts()
        {
            try
            {
                this._context.Database.SetCommandTimeout(280);
                var productCommons = _context.ProductCommons.Include(x => x.HairChallenges).Include(y => y.ProductIndicator).Include(z => z.HairType).
                    Include(h => h.ProductTags).Include(h => h.ProductClassification).ToList();
                var result = _context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                List<ProductsModelList> lstProductModel = result.Select(x => new ProductsModelList
                {
                    Id = x.Id,
                    guid = x.guid,
                    ProductName = x.ProductName,
                    ActualName = x.ActualName,
                    BrandName = x.BrandName,
                    HairType = productCommons.Where(p => p.ProductEntityId == x.Id && p.HairTypeId != null).Select(d => new HairType
                    {
                        Description = d.HairType.Description
                    }).ToList(),
                    ImageName = x.ImageName,
                    Ingredients = x.Ingredients,
                    ProductDetails = x.ProductDetails,
                    ProductLink = x.ProductLink,
                    IsActive = x.IsActive,
                    CreatedOn = x.CreatedOn,
                    ProductTypeId = x.ProductTypesId,
                    ProductType = x.ProductTypes.ProductName,
                    ParentId = x.ProductTypes.ParentId,
                    ParentName = _context.ProductTypes.Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
                    Price = x.Price,
                    HairChallenge = productCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(d => new HairChallenges
                    {
                        Description = d.HairChallenges.Description
                    }).ToList(),
                    ProductIndicate = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductIndicatorId != null).Select(d => new ProductIndicator
                    {
                        Description = d.ProductIndicator.Description
                    }).ToList(),
                    ProductTag = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductTagsId != null).Select(d => new ProductTags
                    {
                        Description = d.ProductTags.Description
                    }).ToList(),
                    ProductClassificatio = productCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(d => new ProductClassification
                    {
                        Description = d.ProductClassification.Description
                    }).ToList()
                }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProductsModelList> GetBrands()
        {
            try
            {
                List<ProductsModelList> lstProductModel = _context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ProductsModelList
                    {
                        Id = x.Id,
                        guid = x.guid,
                        ProductName = x.ProductName,
                        ActualName = x.ActualName,
                        BrandName = x.BrandName,
                        ImageName = x.ImageName,
                        Ingredients = x.Ingredients,
                        ProductDetails = x.ProductDetails,
                        ProductLink = x.ProductLink,
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn,
                        ProductTypeId = x.ProductTypesId,
                        ProductType = x.ProductTypes.ProductName,
                        ParentId = x.ProductTypes.ParentId,
                        ParentName = _context.ProductTypes.Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
                        Price = x.Price,
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ProductEntityModel SaveProducts(ProductEntityModel productEntity)
        {
            try
            {
                List<HairType> listHairType = JsonConvert.DeserializeObject<List<HairType>>(productEntity.TypeFor);
                List<HairChallenges> listHairChallenges = JsonConvert.DeserializeObject<List<HairChallenges>>(productEntity.HairChallenges);
                List<ProductIndicator> listProductIndicators = JsonConvert.DeserializeObject<List<ProductIndicator>>(productEntity.ProductIndicator);
                List<ProductTags> listProductTags = JsonConvert.DeserializeObject<List<ProductTags>>(productEntity.ProductTags);
                List<ProductClassification> listProductClassification = JsonConvert.DeserializeObject<List<ProductClassification>>(productEntity.ProductClassification);


                //var objProduct = _context.ProductEntities.Where(x => x.guid == productEntity.guid).FirstOrDefault();
                //if (objProduct != null)
                //{
                //    objProduct.IsActive = false;
                //    _context.SaveChanges();
                //}

                ProductEntity objProductList = new ProductEntity();
                objProductList.guid = Guid.NewGuid();
                objProductList.ProductTypesId = productEntity.ProductTypesId;
                objProductList.ProductName = productEntity.ProductName;
                objProductList.ActualName = productEntity.ActualName;
                objProductList.BrandName = productEntity.BrandName;
                objProductList.ImageName = productEntity.ImageName;
                objProductList.Ingredients = productEntity.Ingredients;
                objProductList.ProductDetails = productEntity.ProductDetails;
                objProductList.ProductLink = productEntity.ProductLink;
                objProductList.HairChallenges = productEntity.HairChallenges;
                objProductList.ProductIndicator = productEntity.ProductIndicator;
                objProductList.Product = productEntity.ProductClassification;
                objProductList.IsActive = true;
                objProductList.CreatedOn = DateTime.UtcNow;
                objProductList.Price = productEntity.Price;
                _context.Add(objProductList);
                _context.SaveChanges();

                var objProduct = _context.ProductEntities.Where(x => x.guid == productEntity.guid).FirstOrDefault();
                if (objProduct != null)
                {
                    objProduct.IsActive = false;
                    var recProdStyleReg = _context.RecommendedProductsStyleRegimens.Where(x => x.ProductId == objProduct.Id);
                    foreach (var item in recProdStyleReg)
                    {
                        item.ProductId = objProductList.Id;
                    }
                    var recProd = _context.RecommendedProducts.Where(x => x.ProductId == objProduct.Id);
                    foreach (var item in recProd)
                    {
                        item.ProductId = objProductList.Id;
                    }
                    _context.SaveChanges();
                }

                foreach (var spec in listHairType)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.HairTypeId = spec.HairTypeId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listHairChallenges)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.HairChallengeId = spec.HairChallengeId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listProductIndicators)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.ProductIndicatorId = spec.ProductIndicatorId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }

                foreach (var spec in listProductTags)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.ProductTagsId = spec.ProductTagsId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }


                foreach (var spec in listProductClassification)
                {
                    ProductCommon objcommon = new ProductCommon();
                    objcommon.ProductClassificationId = spec.ProductClassificationId;
                    objcommon.IsActive = true;
                    objcommon.CreatedOn = DateTime.Now;
                    objcommon.ProductEntityId = objProductList.Id;

                    _context.Add(objcommon);
                    _context.SaveChanges();
                }
                return productEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteProductCategory(ProductType productType)
        {
            try
            {
                var objProduct = _context.ProductTypeCategories.FirstOrDefault(x => x.Id == productType.Id);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }
        public bool DeleteProductType(ProductType productType)
        {
            try
            {
                var objProduct = _context.ProductTypes.FirstOrDefault(x => x.Id == productType.Id);
                {
                    if (objProduct != null)
                    {
                        objProduct.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }

        public ProductType GetProductTypeById(ProductType productType)
        {
            try
            {
                ProductType objType = _context.ProductTypes.Where(x => x.Id == productType.Id).FirstOrDefault();
                return objType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ProductTypeCategoriesList GetProductCategoryById(ProductTypeCategoriesList productType)
        {
            try
            {
                ProductTypeCategoriesList objType = _context.ProductTypeCategories.Where(x => x.Id == productType.ProductTypeId).
                    Select(x => new ProductTypeCategoriesList
                    {
                        ProductTypeId = x.Id,
                        CategoryName = x.CategoryName,
                        IsHair = x.IsHair,
                        IsRegimen = x.IsRegimens
                    }).FirstOrDefault();
                return objType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ProductTypeCategoryModelList> GetProductsType()
        {
            try
            {
                List<ProductTypeCategoryModelList> productTypes = _context.ProductTypes.Include(x => x.ProductTypeCategory).Where(x => x.IsActive == true && x.ParentId != null).OrderByDescending(x => x.CreatedOn)
                   .Select(x => new ProductTypeCategoryModelList
                   {
                       Id = x.Id,
                       ProductName = x.ProductName,
                       CreatedOn = x.CreatedOn,
                       IsActive = x.IsActive,
                       CategoryName = _context.ProductTypeCategories.FirstOrDefault(c => c.Id == x.ParentId).CategoryName
                   }).ToList();

                //ProductTypeModel productModel = new ProductTypeModel();
                //productModel.productType = productTypes;
                return productTypes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ProductTypeCategoryModel SaveProductType(ProductTypeCategoryModel productType)
        {
            try
            {
                var productTypeModel = _context.ProductTypes.FirstOrDefault(x => x.Id == productType.Id);
                if (productTypeModel != null)
                {
                    productTypeModel.ProductName = productType.ProductName;
                    productTypeModel.ParentId = productType.CategoryId;
                }
                else
                {
                    ProductType objType = new ProductType();
                    objType.ProductName = productType.ProductName;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsActive = true;
                    objType.ParentId = productType.CategoryId;

                    _context.Add(objType);
                }
                _context.SaveChanges();
                return productType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ProductTypeCategoriesList SaveProductCategory(ProductTypeCategoriesList productType)
        {
            try
            {
                var productTypeModel = _context.ProductTypeCategories.FirstOrDefault(x => x.Id == productType.ProductTypeId);
                if (productTypeModel != null)
                {
                    productTypeModel.CategoryName = productType.CategoryName;
                    productTypeModel.IsHair = productType.IsHair;
                    productTypeModel.IsRegimens = productType.IsRegimen;
                }
                else

                {
                    ProductTypeCategory objType = new ProductTypeCategory();
                    objType.CategoryName = productType.CategoryName;
                    objType.CreatedOn = DateTime.Now;
                    objType.IsHair = productType.IsHair;
                    objType.IsRegimens = productType.IsRegimen;
                    objType.IsActive = true;
                    _context.Add(objType);
                }
                _context.SaveChanges();
                return productType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ProductTypesList> GetProductTypes()
        {
            try
            {
                List<ProductTypesList> lstProductModel = _context.ProductTypes.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ProductTypesList
                    {
                        ProductTypeId = x.Id,
                        ProductTypeName = x.ProductName,
                        ParentId = x.ParentId
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProductTypeCategoriesList> GetProductsCategoryList()
        {
            try
            {
                List<ProductTypeCategoriesList> lstProductModel = _context.ProductTypeCategories.Where(y => y.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ProductTypeCategoriesList
                    {
                        ProductTypeId = x.Id,
                        CategoryName = x.CategoryName,
                        IsHair = x.IsHair,
                        IsRegimen = x.IsRegimens
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<HairType> GetHairTypesList()
        {
            try
            {
                List<HairType> lstHairType = _context.HairTypes.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<HairChallenges> GetHairChallengesList()
        {
            try
            {
                List<HairChallenges> lstHairType = _context.HairChallenges.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProductIndicator> GetProductIndicatorsList()
        {
            try
            {
                List<ProductIndicator> lstHairType = _context.ProductIndicator.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<IngedientsEntity> GetIngredientsList()
        {
            try
            {
                List<IngedientsEntity> lstHairType = _context.IngedientsEntities.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProductTags> GetProductTagList()
        {
            try
            {
                List<ProductTags> lstHairType = _context.ProductTags.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProductClassification> GetProductClassificationList()
        {
            try
            {
                List<ProductClassification> lstHairType = _context.ProductClassification.Where(x => x.IsActive == true).ToList();
                return lstHairType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ProductEntityModel> AddProductList(IEnumerable<ProductEntityModel> productData)
        {
            try
            {
                if (productData != null)
                {
                    foreach (var product in productData)
                    {
                        var productType = _context.ProductTypes.Where(x => x.ProductName == product.ProductType && x.ParentId != null).Select(x => x.Id).FirstOrDefault();
                        if (productType == 0)
                        {
                            ProductType objType = new ProductType();
                            objType.ProductName = product.ProductType;
                            objType.ParentId = null;
                            objType.IsActive = true;
                            objType.CreatedOn = DateTime.Now;
                            _context.Add(objType);
                            _context.SaveChanges();

                            ProductType objType2 = new ProductType();
                            objType2.ProductName = product.ProductType;
                            objType2.ParentId = objType.Id;
                            objType2.IsActive = true;
                            objType2.CreatedOn = DateTime.Now;
                            _context.Add(objType2);
                            _context.SaveChanges();
                        }

                        ProductEntity objProduct = new ProductEntity();
                        objProduct.guid = Guid.NewGuid();
                        objProduct.ProductName = product.ProductName;
                        objProduct.ActualName = product.ActualName;
                        objProduct.BrandName = product.BrandName;
                        objProduct.TypeFor = product.TypeFor;
                        objProduct.ImageName = product.ImageName;
                        objProduct.Ingredients = product.Ingredients;
                        objProduct.ProductDetails = product.ProductDetails;
                        objProduct.ProductLink = product.ProductLink;
                        objProduct.Price = product.Price;
                        objProduct.Product = product.ProductClassification;
                        objProduct.ProductTypesId = _context.ProductTypes.Where(x => x.ProductName == product.ProductType && x.ParentId != null).Select(x => x.Id).FirstOrDefault(); ;
                        objProduct.HairChallenges = product.HairChallenges;
                        objProduct.ProductIndicator = product.ProductIndicator;
                        objProduct.ProductTags = product.ProductTags;
                        objProduct.IsActive = true;
                        objProduct.CreatedOn = DateTime.Now;

                        _context.Add(objProduct);
                        _context.SaveChanges();
                    }
                }
                return productData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public (JsonResult result, bool success, string error) UploadFile(IFormFile file, UserEntity entity)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var extension = Path.GetExtension(fileName);
                    if (extension != ".xlsx")
                    {
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var imageURL = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        if (entity != null)
                        {
                            UserEntity us = _context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                            us.ImageURL = "http://localhost:5004/" + "Resources/" + "Images/" + fileName;
                            _context.SaveChanges();
                        }
                        return (new JsonResult(""), true, "http://localhost:5004/" + "Resources/" + "Images/" + fileName);
                    }
                    return (new JsonResult(""), false, "Wrong Image File");
                }
                else
                {
                    return (new JsonResult(""), false, "");
                }
            }
            catch (Exception ex)
            {
                return (new JsonResult(""), false, "");
            }
        }


        public List<ProductTypeCategory> GetProductCategory()
        {
            try
            {
                List<ProductTypeCategory> lstProductModel = _context.ProductTypeCategories.Where(x => x.IsActive == true).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ProductsListings GetAllProducts()
        {
            try
            {

                this._context.Database.SetCommandTimeout(280);
                ProductsListings productsListings = new ProductsListings();
                productsListings.TypeCategories = _context.ProductTypeCategories.AsNoTracking().Where(x => x.IsActive == true).ToList();
                productsListings.ProductClassifications = _context.ProductClassification.Where(x => x.IsActive == true).ToList();
                productsListings.HairChallenges = _context.HairChallenges.Where(x => x.IsActive == true).ToList();
                productsListings.ProductTypes = _context.ProductTypes.AsNoTracking().Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                        Select(x => new ProductTypesList
                        {
                            ProductTypeId = x.Id,
                            ProductTypeName = x.ProductName,
                            ParentId = x.ParentId
                        }).ToList();

                productsListings.ProductsModelLists = _context.ProductEntities.AsNoTracking().Include(x => x.ProductTypes).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ProductsModelList
                    {
                        Id = x.Id,
                        ProductName = x.ProductName,
                        BrandName = x.BrandName,
                        ProductTypeId = x.ProductTypesId,
                        ProductType = x.ProductTypes.ProductName,
                        ParentId = x.ProductTypes.ParentId,
                        ParentName = _context.ProductTypes.AsNoTracking().Where(y => y.Id == x.ProductTypes.ParentId).Select(y => y.ProductName).FirstOrDefault(),
                        ProductClassificatio = _context.ProductCommons.Include(p => p.ProductClassification).Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(p => new ProductClassification {
                                Description = p.ProductClassification.Description,
                                ProductClassificationId = p.ProductClassification.ProductClassificationId
                            }).ToList(),
                        HairChallenge = _context.ProductCommons.Include(p => p.HairChallenges).Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(p => new HairChallenges
                        {
                            Description = p.HairChallenges.Description,
                            HairChallengeId = p.HairChallenges.HairChallengeId
                        }).ToList()
                    }).ToList();
                return productsListings;
            }
            catch (Exception ex)
            {
                ProductsListings productsListings = new ProductsListings();
                productsListings.Message = ex.Message;
                return productsListings;
            }
        }
    }
}
