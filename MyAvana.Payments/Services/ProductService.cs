using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvana.Payments.Api.Contract;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly AvanaContext _avanaContext;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;






        public ProductService(AvanaContext avanaContext, IHostingEnvironment hostingEnvironment, ILogger logger)
        {
            _avanaContext = avanaContext;
            _env = hostingEnvironment;
            _logger = logger;
        }

        public (JsonResult result, bool success, string error) GetProductDetails(string id)
        {
            try
            {
                var result = _avanaContext.ProductEntities.Where(s => s.guid.ToString() == id).ToList();
                return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        public (JsonResult result, bool success, string error) GetSuggestions(string hairType, string productType, string hairChallenge)
        {
            try
            {
                String type1 = "1";

                List<ProductEntity> result = new List<ProductEntity>();
                List<ProductEntity> resultToReturn = new List<ProductEntity>();
                string[] productTypeArray = null;
                string[] hairTypeArray = null;
                string[] hairChallengeArray = null;
                //string[] hairChallengeArray = hairChallenge.Split(',');
                string hairTypes = "";

                hairType = hairType.ToLower().Replace("type", "").Trim();

                if (hairType == type1)
                {
                    hairTypes = "1a,1b,1c,all types";
                    hairTypeArray = hairTypes.Split(',');
                }
                else
                {
                    hairTypes = hairType + "," + "all types";
                    hairTypeArray = hairTypes.Split(',');
                }

                if (productType == null || productType == "")
                {
                    result = (from products in _avanaContext.ProductEntities
                              join common in _avanaContext.ProductCommons
                              on products.Id equals common.ProductEntityId
                              join pType in _avanaContext.ProductTypes
                              on products.ProductTypesId equals pType.Id
                              join types in _avanaContext.HairTypes
                              on common.HairTypeId equals types.HairTypeId
                              where hairTypeArray.Contains(types.Description) && products.IsActive == true
                              select products).Distinct().ToList();
                    if (!String.IsNullOrEmpty(hairChallenge))
                    {
                        hairChallengeArray = hairChallenge.Split(',');
                        var resultChallenges = (from common in _avanaContext.ProductCommons
                                                where common.HairChallengeId != null
                                                group common by common.ProductEntityId into g
                                                select new { g.Key, Items = string.Join(",", g.Select(kvp => kvp.HairChallengeId)) });

                        resultToReturn = (from r in result
                                          join c in resultChallenges on r.Id equals c.Key
                                          where hairChallengeArray.ToList().Intersect(c.Items.Split(",").ToList()).Count() > 0
                                          select r).ToList();
                    }
                    else
                        resultToReturn = result;
                }
                else
                {
                    if (productType != null)
                    {
                        productTypeArray = productType.Split(',');
                    }
                    result = (from products in _avanaContext.ProductEntities
                              join common in _avanaContext.ProductCommons
                              on products.Id equals common.ProductEntityId
                              join pType in _avanaContext.ProductTypes
                              on products.ProductTypesId equals pType.Id
                              join types in _avanaContext.HairTypes
                              on common.HairTypeId equals types.HairTypeId
                              join pTypeCat in _avanaContext.ProductTypeCategories
                              on pType.ParentId equals pTypeCat.Id
                              where hairTypeArray.Contains(types.Description) && products.IsActive == true && productTypeArray.Contains(pTypeCat.Id.ToString())
                              select products).Distinct().ToList();

                    if (!String.IsNullOrEmpty(hairChallenge))
                    {
                        hairChallengeArray = hairChallenge.Split(',');
                        var resultChallenges = (from common in _avanaContext.ProductCommons
                                                where common.HairChallengeId != null
                                                group common by common.ProductEntityId into g
                                                select new { g.Key, Items = string.Join(",", g.Select(kvp => kvp.HairChallengeId)) });

                        resultToReturn = (from r in result
                                          join c in resultChallenges on r.Id equals c.Key
                                          where hairChallengeArray.ToList().Intersect(c.Items.Split(",").ToList()).Count() > 0
                                          select r).ToList();
                    }
                    else
                        resultToReturn = result;
                }
                return (new JsonResult(resultToReturn), true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);
            }
        }


        public (JsonResult result, bool success, string error) GetAllHairTypes()
        {
            try
            {
                var result = _avanaContext.ProductEntities.Where(i => i.IsActive == true).Select(x => new { x.guid, x.TypeFor }).Distinct().ToList();
                return (new JsonResult(result), true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        public (JsonResult result, bool success, string error) GetAllProductTypes(string hairType)
        {
            try
            {
                String type1 = "1";

                string[] hairTypeArray = null;
                string hairTypes = "";

                hairType = hairType.ToLower().Replace("type", "").Trim();

                if (hairType == type1)
                {
                    hairTypes = "1a,1b,1c,all types";
                    hairTypeArray = hairTypes.Split(',');
                }
                else
                {
                    hairTypes = hairType + "," + "all types";
                    hairTypeArray = hairTypes.Split(',');
                }

                List<int?> parentids = (from products in _avanaContext.ProductEntities
                                        join common in _avanaContext.ProductCommons
                                        on products.Id equals common.ProductEntityId
                                        join pType in _avanaContext.ProductTypes
                                        on products.ProductTypesId equals pType.Id
                                        join types in _avanaContext.HairTypes
                                        on common.HairTypeId equals types.HairTypeId
                                        where hairTypeArray.Contains(types.Description) && products.IsActive == true
                                        select products.ProductTypes.ParentId).Distinct().ToList();

                var result = _avanaContext.ProductTypeCategories.Where(x => parentids.Contains(x.Id)).Select(x => new { x.CategoryName, x.Id, x.CreatedOn }).Distinct().ToList();
                return (new JsonResult(result), true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);
            }
        }


        public (JsonResult result, bool success, string error) GetProductsByTypes(string hairTypes)
        {
            try
            {
                string[] productTypeArray = null;
                if (hairTypes != null)
                {
                    productTypeArray = hairTypes.Split(',');
                }
                var result = _avanaContext.ProductEntities.Include(x => x.ProductTypes).Where(s => productTypeArray.Contains(s.TypeFor)).ToList();
                return (new JsonResult(result), true, "");
            }
            catch (Exception Ex)
            {
                return (new JsonResult(""), false, Ex.Message);

            }
        }
    }
}
