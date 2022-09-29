using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class HairProfileService : IHairProfileService
    {
        private AvanaContext context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IEmailService _emailService;
        public HairProfileService(AvanaContext _context, UserManager<UserEntity> userManager, IEmailService emailService)
        {
            context = _context;
            _userManager = userManager;
            _emailService = emailService;

        }

        public HairProfileModel GetHairProfile(string userId)
        {
            int id = context.HairProfiles.Where(x => x.UserId == userId && x.IsActive == true).Select(x => x.Id).FirstOrDefault();

            HairProfileModel profile = (from hr in context.HairProfiles
                                        join st in context.HairStrands
                                        on hr.Id equals st.HairProfileId
                                        where hr.UserId == userId
                                        && hr.IsActive == true && hr.IsDrafted == false
                                        select new HairProfileModel()
                                        {
                                            UserId = hr.UserId,
                                            ProfileId = hr.Id,
                                            HairId = hr.HairId,
                                            HealthSummary = hr.HealthSummary,
                                            TopLeft = new TopLeftMobile()
                                            {
                                                TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.TopLeftImage).ToList(),
                                                TopLeftHealthText = st.TopLeftHealthText,
                                                TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                                Health = (from hb in context.HairHealths
                                                          join ob in context.Healths
                                                          on hb.HealthId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                          select new HealthModel()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),

                                            },
                                            TopRight = new TopRightMobile()
                                            {
                                                TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.TopRightImage).ToList(),
                                                TopRightHealthText = st.TopRightHealthText,
                                                TopRightStrandDiameter = st.TopRightStrandDiameter,
                                                Health = (from hb in context.HairHealths
                                                          join ob in context.Healths
                                                          on hb.HealthId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                          select new HealthModel()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                            },
                                            BottomLeft = new BottomLeftMobile()
                                            {
                                                BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.BottomLeftImage).ToList(),
                                                BottomLeftHealthText = st.BottomLeftHealthText,
                                                BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                                Health = (from hb in context.HairHealths
                                                          join ob in context.Healths
                                                          on hb.HealthId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                          select new HealthModel()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),

                                            },
                                            BottomRight = new BottomRightMobile()
                                            {
                                                BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.BottomRightImage).ToList(),
                                                BottomRightHealthText = st.BottomRightHealthText,
                                                BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                                Health = (from hb in context.HairHealths
                                                          join ob in context.Healths
                                                          on hb.HealthId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                          select new HealthModel()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                            },
                                            CrownStrand = new CrownStrandMobile()
                                            {
                                                CrownHealthText = st.CrownHealthText,
                                                CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.CrownImage).ToList(),
                                                CrownStrandDiameter = st.CrownStrandDiameter,
                                                Health = (from hb in context.HairHealths
                                                          join ob in context.Healths
                                                          on hb.HealthId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                          select new HealthModel()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                            },
                                            RecommendedVideos = (from rv in context.RecommendedVideos
                                                                 join ml in context.MediaLinkEntities
                                                                 on rv.MediaLinkEntityId equals ml.MediaLinkEntityId
                                                                 where rv.HairProfileId == hr.Id
                                                                 select new RecommendedVideos()
                                                                 {
                                                                     MediaLinkEntityId = ml.MediaLinkEntityId,
                                                                     Name = ml.VideoId
                                                                 }).ToList()

                                            //RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hr.Id).ToList()
                                        }).FirstOrDefault();
            if (id != 0)
            {
                profile.TopLeft.ObservationValues = GetData(id, "topLeft");
                profile.TopRight.ObservationValues = GetData(id, "topRight");
                profile.BottomLeft.ObservationValues = GetData(id, "bottomLeft");
                profile.BottomRight.ObservationValues = GetData(id, "bottomRight");
                profile.CrownStrand.ObservationValues = GetData(id, "crown");

                foreach (var abc in profile.RecommendedVideos)
                {
                    if (abc.Name.Contains("instagram"))
                    {
                        abc.ThumbNail = "http://admin.myavana.com/images/instagram.jpg";
                    }
                }
            }
            return profile;
        }

        public HairProfileModel2 GetHairProfile2(string userId)
        {
            int id = context.HairProfiles.Where(x => x.UserId == userId && x.IsActive == true).Select(x => x.Id).FirstOrDefault();

            HairProfileModel2 profile = (from hr in context.HairProfiles
                                         join st in context.HairStrands
                                         on hr.Id equals st.HairProfileId
                                         where hr.UserId == userId
                                         && hr.IsActive == true && hr.IsDrafted == false
                                         select new HairProfileModel2()
                                         {
                                             UserId = hr.UserId,
                                             ProfileId = hr.Id,
                                             HairId = hr.HairId,
                                             HealthSummary = hr.HealthSummary,
                                             ConsultantNotes = hr.ConsultantNotes,
                                             TopLeft = new TopLeftMobile()
                                             {
                                                 TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.TopLeftImage).ToList(),
                                                 TopLeftHealthText = st.TopLeftHealthText,
                                                 TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                                 Health = (from hb in context.HairHealths
                                                           join ob in context.Healths
                                                           on hb.HealthId equals ob.Id
                                                           where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                           select new HealthModel()
                                                           {
                                                               Id = ob.Id,
                                                               Description = ob.Description
                                                           }).ToList(),

                                             },
                                             TopRight = new TopRightMobile()
                                             {
                                                 TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.TopRightImage).ToList(),
                                                 TopRightHealthText = st.TopRightHealthText,
                                                 TopRightStrandDiameter = st.TopRightStrandDiameter,
                                                 Health = (from hb in context.HairHealths
                                                           join ob in context.Healths
                                                           on hb.HealthId equals ob.Id
                                                           where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                           select new HealthModel()
                                                           {
                                                               Id = ob.Id,
                                                               Description = ob.Description
                                                           }).ToList(),
                                             },
                                             BottomLeft = new BottomLeftMobile()
                                             {
                                                 BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.BottomLeftImage).ToList(),
                                                 BottomLeftHealthText = st.BottomLeftHealthText,
                                                 BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                                 Health = (from hb in context.HairHealths
                                                           join ob in context.Healths
                                                           on hb.HealthId equals ob.Id
                                                           where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                           select new HealthModel()
                                                           {
                                                               Id = ob.Id,
                                                               Description = ob.Description
                                                           }).ToList(),

                                             },
                                             BottomRight = new BottomRightMobile()
                                             {
                                                 BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.BottomRightImage).ToList(),
                                                 BottomRightHealthText = st.BottomRightHealthText,
                                                 BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                                 Health = (from hb in context.HairHealths
                                                           join ob in context.Healths
                                                           on hb.HealthId equals ob.Id
                                                           where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                           select new HealthModel()
                                                           {
                                                               Id = ob.Id,
                                                               Description = ob.Description
                                                           }).ToList(),
                                             },
                                             CrownStrand = new CrownStrandMobile()
                                             {
                                                 CrownHealthText = st.CrownHealthText,
                                                 CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "").Select(x => "http://admin.myavana.com/HairProfile/" + x.CrownImage).ToList(),
                                                 CrownStrandDiameter = st.CrownStrandDiameter,
                                                 Health = (from hb in context.HairHealths
                                                           join ob in context.Healths
                                                           on hb.HealthId equals ob.Id
                                                           where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                           select new HealthModel()
                                                           {
                                                               Id = ob.Id,
                                                               Description = ob.Description
                                                           }).ToList(),
                                             },
                                             RecommendedVideos = (from rv in context.RecommendedVideos
                                                                  join ml in context.MediaLinkEntities
                                                                  on rv.MediaLinkEntityId equals ml.MediaLinkEntityId
                                                                  where rv.HairProfileId == hr.Id
                                                                  select new RecommendedVideos()
                                                                  {
                                                                      MediaLinkEntityId = ml.MediaLinkEntityId,
                                                                      Name = ml.VideoId
                                                                  }).ToList(),
                                             RecommendedStylist = (from rs in context.RecommendedStylists
                                                                   join ml in context.Stylists
                                                                   on rs.StylistId equals ml.StylistId
                                                                   where rs.HairProfileId == hr.Id
                                                                   select new StylistCustomerModel()
                                                                   {
                                                                       StylistName = ml.StylistName,
                                                                       Salon = ml.SalonName,
                                                                       Email = ml.Email,
                                                                       Phone = ml.PhoneNumber,
                                                                       Website = ml.Website,
                                                                       Instagram = ml.Instagram
                                                                   }).ToList()
                                             //RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hr.Id).ToList()
                                         }).FirstOrDefault();
            if (id != 0)
            {
                profile.TopLeft.ObservationValues = GetData2(id, "topLeft");
                profile.TopRight.ObservationValues = GetData2(id, "topRight");
                profile.BottomLeft.ObservationValues = GetData2(id, "bottomLeft");
                profile.BottomRight.ObservationValues = GetData2(id, "bottomRight");
                profile.CrownStrand.ObservationValues = GetData2(id, "crown");

                foreach (var abc in profile.RecommendedVideos)
                {
                    if (abc.Name.Contains("instagram"))
                    {
                        abc.ThumbNail = "http://admin.myavana.com/images/instagram.jpg";
                    }
                }
            }
            return profile;
        }

        public CollaboratedDetailModel CollaboratedDetails(string hairProfileId)
        {
            int profileId = Convert.ToInt32(hairProfileId);
            CollaboratedDetailModel collaboratedDetailModel = new CollaboratedDetailModel();

            List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == profileId).Select(x => x.ProductId).ToList();
            List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
            List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
            var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
            List<RecommendedProductsModel> productsTypesList = new List<RecommendedProductsModel>();
            foreach (var parentProduct in parents)
            {
                RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                productsTypes.ProductParentName = parentProduct.CategoryName;
                productsTypes.ProductId = parentProduct.Id;
                List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                    .Select(x => x.ProductTypesId).Distinct().ToList();

                foreach (var type in productByTypes)
                {
                    if (type != null)
                    {
                        ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                        var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id)).ToList();
                        if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                        {
                            productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                            productsTypesModel.ProductId = type;
                        }

                        productsTypesModel.Products = products.Select(x => new ProductsModels
                        {
                            Id = x.Id,
                            BrandName = x.BrandName,
                            ImageName = x.ImageName,
                            ProductLink = x.ProductLink,
                            ProductDetails = x.ProductDetails,
                            ProductName = x.ProductName,
                            ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                        }).ToList();
                        productsTypesModels.Add(productsTypesModel);
                    }
                }
                productsTypes.ProductsTypes = productsTypesModels;
                productsTypesList.Add(productsTypes);
            }
            collaboratedDetailModel.ProductDetailModel = productsTypesList;

            //Styling Regimens Code
            List<int> rProductIds = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == profileId).Select(x => x.ProductId).ToList();
            List<int?> pTypes = context.ProductEntities.Where(x => rProductIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
            List<int?> pParentIds = context.ProductTypes.Where(x => pTypes.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
            var pParents = context.ProductTypeCategories.Where(x => pParentIds.Contains(x.Id)).ToList();
            List<RecommendedProductsStylingModel> productsTypesStylingList = new List<RecommendedProductsStylingModel>();
            foreach (var parentProduct in pParents)
            {
                RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                productsTypes.ProductParentName = parentProduct.CategoryName;
                productsTypes.ProductId = parentProduct.Id;
                List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id))
                    .Select(x => x.ProductTypesId).Distinct().ToList();

                foreach (var type in productByTypes)
                {
                    if (type != null)
                    {
                        ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                        var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id)).ToList();
                        if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                        {
                            productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                            productsTypesModel.ProductId = type;
                        }

                        productsTypesModel.Products = products.Select(x => new ProductsStylingModels
                        {
                            Id = x.Id,
                            BrandName = x.BrandName,
                            ImageName = x.ImageName,
                            ProductLink = x.ProductLink,
                            ProductDetails = x.ProductDetails,
                            ProductName = x.ProductName,
                            ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                        }).ToList();
                        productsTypesModels.Add(productsTypesModel);
                    }
                }
                productsTypes.ProductsTypes = productsTypesModels;
                productsTypesStylingList.Add(productsTypes);
            }
            collaboratedDetailModel.RecommendedProductsStyling = productsTypesStylingList;

            collaboratedDetailModel.IngredientDetailModel = (from rprod in context.RecommendedIngredients
                                                             join ing in context.IngedientsEntities
                                                             on rprod.IngredientId equals ing.IngedientsEntityId
                                                             where rprod.HairProfileId == profileId
                                                             select new IngredientDetailModel()
                                                             {
                                                                 IngredientId = ing.IngedientsEntityId,
                                                                 Name = ing.Name,
                                                                 ImageUrl = "http://admin.myavana.com/Ingredients/" + ing.Image
                                                             }).ToList();
            collaboratedDetailModel.RegimenDetailModel = (from rprod in context.RecommendedRegimens
                                                          join reg in context.Regimens
                                                          on rprod.RegimenId equals reg.RegimensId
                                                          where rprod.HairProfileId == profileId
                                                          select new RegimenDetailModel()
                                                          {
                                                              RegimenId = reg.RegimensId,
                                                              RegimenName = reg.Name
                                                          }).ToList();
            return collaboratedDetailModel;
        }

        public RecommendedRegimensModel RecommendedRegimens(int regimenId)
        {
            Regimens regimens = context.Regimens.Where(x => x.RegimensId == regimenId).FirstOrDefault();
            RegimenSteps regimenList = context.RegimenSteps.Where(z => z.RegimenStepsId == regimens.RegimenStepsId).FirstOrDefault();
            RegimenSteps regimenSteps = regimenList;

            List<RegimenStepsModel> regimenStepsModels = GetRegimenSteps(regimenSteps);

            RecommendedRegimensModel recommendedRegimensModel = new RecommendedRegimensModel();
            recommendedRegimensModel.RegimenId = regimens.RegimensId;
            recommendedRegimensModel.RegimenName = regimens.Name;
            recommendedRegimensModel.RegimenSteps = regimenStepsModels;
            recommendedRegimensModel.RegimenTitle = regimens.Title;
            recommendedRegimensModel.Description = regimens.Description;

            return recommendedRegimensModel;
        }

        public RecommendedProductModel RecommendedProducts(int productId)
        {
            String[] Ingredient = { "Almond Oil", "Aloe Vera", "AMMONIUM LAURETH SULFATE", "Apple Cider vinegar", "Argan Oil", "Avocado Oil", "Beeswax", "BEHENTRIMONIUM CHLORIDE", "BEHENTRIMONIUM METHOSULFATE", "Biotin", "Boabab Oil", "Castor Oil" };

            var productList = (from prod in context.ProductEntities
                               where prod.Id == productId
                               select new RecommendedProductModel()
                               {
                                   ProductId = prod.Id,
                                   ProductName = prod.ProductName,
                                   ActualName = prod.ActualName,
                                   ImageUrl = prod.ImageName,
                                   PurchaseLink = prod.ProductLink,
                                   ProductDetails = prod.ProductDetails,
                                   BrandName = prod.BrandName,
                                   //TypeFor = prod.TypeFor,
                                   TypeFor = (from prdc in context.ProductCommons
                                              where prdc.ProductEntityId == productId && prdc.HairTypeId != null
                                              select new HairTypes()
                                              {
                                                  HairType = prdc.HairType.Description
                                              }).ToList(),
                                   Ingredient = (from ing in Ingredient
                                                 select new Ingredients()
                                                 {
                                                     Name = ing,
                                                 }).ToList()
                               }).FirstOrDefault();
            return productList;
        }

        public RecommendedProductModel2 RecommendedProducts2(int productId)
        {
            String[] Ingredient = { "Almond Oil", "Aloe Vera", "AMMONIUM LAURETH SULFATE", "Apple Cider vinegar", "Argan Oil", "Avocado Oil", "Beeswax", "BEHENTRIMONIUM CHLORIDE", "BEHENTRIMONIUM METHOSULFATE", "Biotin", "Boabab Oil", "Castor Oil" };
            var productList = (from prod in context.ProductEntities
                               where prod.Id == productId
                               select new RecommendedProductModel2()
                               {
                                   ProductId = prod.Id,
                                   ProductName = prod.ProductName,
                                   ActualName = prod.ActualName,
                                   ImageUrl = prod.ImageName,
                                   PurchaseLink = prod.ProductLink,
                                   ProductDetails = prod.ProductDetails,
                                   BrandName = prod.BrandName,
                                   TypeFor = prod.TypeFor,
                                   Ingredients = (from ing in Ingredient
                                                  select new Ingredients()
                                                  {
                                                      Name = ing,
                                                  }).ToList()
                               }).FirstOrDefault();
            return productList;
        }

        private List<RegimenStepsModel> GetRegimenSteps(RegimenSteps regimenSteps)
        {
            List<RegimenStepsModel> regimenStepsModels = new List<RegimenStepsModel>();


            for (int i = 1; i <= 20; i++)
            {
                if (i == 1 && regimenSteps.Step1Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step1Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step1Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 2 && regimenSteps.Step2Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step2Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step2Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 3 && regimenSteps.Step3Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step3Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step3Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 4 && regimenSteps.Step4Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step4Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step4Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 5 && regimenSteps.Step5Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step5Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step5Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 6 && regimenSteps.Step6Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step6Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step6Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 7 && regimenSteps.Step7Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step7Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step7Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 8 && regimenSteps.Step8Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step8Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step8Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 9 && regimenSteps.Step9Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step9Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step9Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 10 && regimenSteps.Step10Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step10Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step10Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 11 && regimenSteps.Step11Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step11Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step11Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 12 && regimenSteps.Step12Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step12Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step12Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 13 && regimenSteps.Step13Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step13Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step13Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 14 && regimenSteps.Step14Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step14Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step14Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 15 && regimenSteps.Step15Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step15Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step15Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 16 && regimenSteps.Step16Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();

                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step16Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step16Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 17 && regimenSteps.Step17Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step17Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step17Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 18 && regimenSteps.Step18Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step18Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step18Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 19 && regimenSteps.Step19Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step19Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step19Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
                if (i == 20 && regimenSteps.Step20Photo != null)
                {
                    RegimenStepsModel regimenStepsModel = new RegimenStepsModel();
                    regimenStepsModel.RegimenStepPhoto = "http://admin.myavana.com/Regimens/" + regimenSteps.Step20Photo;
                    regimenStepsModel.RegimenStepInstruction = regimenSteps.Step20Instruction;
                    regimenStepsModels.Add(regimenStepsModel);
                }
            }

            return regimenStepsModels;
        }

        public Models.ViewModels.HairProfile SaveProfile(Models.ViewModels.HairProfile hairProfile)
        {
            string TabNo = hairProfile.TabNo;


            List<HairStrandsImages> hairStrandsImages = new List<HairStrandsImages>();
            Models.Entities.HairProfile hair = new Models.Entities.HairProfile();

            hairProfile.Health = JsonConvert.DeserializeObject<List<HealthModel>>(hairProfile.TempHealth);

            hairProfile.Observation = JsonConvert.DeserializeObject<List<ObservationModel>>(hairProfile.TempObservation);
            hairProfile.observationElasticityModels = JsonConvert.DeserializeObject<List<ObservationElasticityModel>>(hairProfile.TempObservationElasticity);
            hairProfile.observationChemicalModels = JsonConvert.DeserializeObject<List<ObservationChemicalModel>>(hairProfile.TempObservationChemicalProduct);
            hairProfile.observationPhysicalModels = JsonConvert.DeserializeObject<List<ObservationPhysicalModel>>(hairProfile.TempObservationPhysicalProduct);
            //hairProfile.observationDamageModels = JsonConvert.DeserializeObject<List<ObservationDamageModel>>(hairProfile.TempObservationDamage);
            hairProfile.observationBreakageModels = JsonConvert.DeserializeObject<List<ObservationBreakageModel>>(hairProfile.TempObservationBreakage);
            hairProfile.observationSplittingModels = JsonConvert.DeserializeObject<List<ObservationSplittingModel>>(hairProfile.TempObservationSplitting);
            hairProfile.SelectedAnswer = JsonConvert.DeserializeObject<QuestionaireSelectedAnswer>(hairProfile.TempSelectedAnswer);

            hairProfile.Pororsity = JsonConvert.DeserializeObject<List<PororsityModel>>(hairProfile.TempPororsity);
            hairProfile.Elasticity = JsonConvert.DeserializeObject<List<ElasticityModel>>(hairProfile.TempElasticity);

            hairProfile.RecommendedIngredients = JsonConvert.DeserializeObject<List<RecommendedIngredients>>(hairProfile.TempRecommendedIngredients);
            hairProfile.RecommendedVideos = JsonConvert.DeserializeObject<List<RecommendedVideos>>(hairProfile.TempRecommendedVideos);

            hairProfile.RecommendedTools = JsonConvert.DeserializeObject<List<RecommendedTools>>(hairProfile.TempRecommendedTools);
            hairProfile.RecommendedCategories = JsonConvert.DeserializeObject<List<RecommendedCategory>>(hairProfile.TempRecommendedCategories);
            hairProfile.RecommendedProductTypes = JsonConvert.DeserializeObject<List<RecommendedProductTypes>>(hairProfile.TempRecommendedProductTypes);

            hairProfile.RecommendedProducts = JsonConvert.DeserializeObject<List<RecommendedProducts>>(hairProfile.TempRecommendedProducts);
            hairProfile.AllRecommendedProductsEssential = JsonConvert.DeserializeObject<List<RecommendedProducts>>(hairProfile.TempAllRecommendedProductsEssential);
            hairProfile.AllRecommendedProductsStyling = JsonConvert.DeserializeObject<List<RecommendedProductsStylingRegimen>>(hairProfile.TempAllRecommendedProductsStyling);
            hairProfile.RecommendedProductsStylings = JsonConvert.DeserializeObject<List<RecommendedProductsStylingRegimen>>(hairProfile.TempRecommendedProductsStylings);
            hairProfile.RecommendedRegimens = JsonConvert.DeserializeObject<List<RecommendedRegimens>>(hairProfile.TempRecommendedRegimens);
            hairProfile.RecommendedStylists = JsonConvert.DeserializeObject<List<RecommendedStylist>>(hairProfile.TempRecommendedStylist);

            string[] TopLeftImage = null;
            if (hairProfile.TopLeftPhoto != null)
            {
                TopLeftImage = hairProfile.TopLeftPhoto.Split(',');
            }
            string[] TopRightImage = null;
            if (hairProfile.TopRightPhoto != null)
            {
                TopRightImage = hairProfile.TopRightPhoto.Split(',');
            }
            string[] BottomLeftImage = null;
            if (hairProfile.BottomLeftPhoto != null)
            {
                BottomLeftImage = hairProfile.BottomLeftPhoto.Split(',');
            }
            string[] BottomRightImage = null;
            if (hairProfile.BottomRightPhoto != null)
            {
                BottomRightImage = hairProfile.BottomRightPhoto.Split(',');
            }
            string[] CrownImage = null;
            if (hairProfile.CrownPhoto != null)
            {
                CrownImage = hairProfile.CrownPhoto.Split(',');
            }

            if (hairProfile.UserId != null)
            {


                hair = context.HairProfiles.Where(x => x.UserId == hairProfile.UserId && x.IsActive == true).FirstOrDefault();
                var user = context.Users.Where(x => x.Email.ToLower() == hairProfile.UserId.ToLower()).FirstOrDefault();
                if (hair != null)
                {

                    if (TabNo.Equals("Tab1"))
                    {
                        if (hairProfile.SaveType == "draft")
                        {

                            hair.UserId = hairProfile.UserId;
                            hair.HairId = hairProfile.HairId;
                            hair.HealthSummary = hairProfile.HealthSummary;
                            hair.ConsultantNotes = hairProfile.ConsultantNotes;
                            hair.IsActive = true;
                            hair.IsDrafted = true;
                            hair.CreatedOn = DateTime.Now;

                            // context.Add(hair);
                            context.SaveChanges();
                        }
                        else
                        {
                            hair.UserId = hairProfile.UserId;
                            hair.HairId = hairProfile.HairId;
                            hair.HealthSummary = hairProfile.HealthSummary;
                            hair.ConsultantNotes = hairProfile.ConsultantNotes;
                            hair.IsActive = true;
                            hair.CreatedOn = DateTime.Now;
                            hair.IsDrafted = false;
                            // context.Add(hair);
                            context.SaveChanges();
                        }
                    }
                }
                else
                {
                    //if (TabNo.Equals("Tab1"))
                    //{
                    if (hairProfile.SaveType == "draft")
                    {
                        hair = new Models.Entities.HairProfile();

                        hair.UserId = hairProfile.UserId;
                        hair.HairId = hairProfile.HairId;
                        hair.HealthSummary = hairProfile.HealthSummary;
                        hair.ConsultantNotes = hairProfile.ConsultantNotes;
                        hair.IsActive = true;
                        hair.IsDrafted = true;
                        hair.CreatedOn = DateTime.Now;

                        context.Add(hair);
                        context.SaveChanges();
                    }
                    else
                    {
                        hair = new Models.Entities.HairProfile();

                        hair.UserId = hairProfile.UserId;
                        hair.HairId = hairProfile.HairId;
                        hair.HealthSummary = hairProfile.HealthSummary;
                        hair.ConsultantNotes = hairProfile.ConsultantNotes;
                        hair.IsActive = true;
                        hair.CreatedOn = DateTime.Now;
                        hair.IsDrafted = false;
                        context.Add(hair);
                        context.SaveChanges();
                    }
                    //}


                }
                if (TabNo.Equals("Tab2"))
                {
                    HairStrands strands = new HairStrands();
                    strands = context.HairStrands.Where(x => x.HairProfileId == hair.Id).FirstOrDefault();
                    if (strands != null)
                    {
                        //HairStrands strands = new HairStrands();
                        strands.TopLeftPhoto = hairProfile.TopLeftPhoto;
                        strands.TopLeftStrandDiameter = hairProfile.TopLeftStrandDiameter;
                        strands.TopLeftHealthText = hairProfile.TopLeftHealthText;

                        strands.TopRightPhoto = hairProfile.TopRightPhoto;
                        strands.TopRightStrandDiameter = hairProfile.TopRightStrandDiameter;
                        strands.TopRightHealthText = hairProfile.TopRightHealthText;

                        strands.BottomLeftPhoto = hairProfile.BottomLeftPhoto;
                        strands.BottomLeftStrandDiameter = hairProfile.BottomLeftStrandDiameter;
                        strands.BottomLeftHealthText = hairProfile.BottomLeftHealthText;

                        strands.BottomRightPhoto = hairProfile.BottomRightPhoto;
                        strands.BottomRightHealthText = hairProfile.BottomRightHealthText;
                        strands.BottomRightStrandDiameter = hairProfile.BottoRightStrandDiameter;

                        strands.CrownPhoto = hairProfile.CrownPhoto;
                        strands.CrownHealthText = hairProfile.CrownHealthText;
                        strands.CrownStrandDiameter = hairProfile.CrownStrandDiameter;

                        strands.HairProfileId = hair.Id;

                        //context.Add(strands);
                        context.SaveChanges();
                    }
                    else
                    {
                        strands = new HairStrands();
                        strands.TopLeftPhoto = hairProfile.TopLeftPhoto;
                        strands.TopLeftStrandDiameter = hairProfile.TopLeftStrandDiameter;
                        strands.TopLeftHealthText = hairProfile.TopLeftHealthText;

                        strands.TopRightPhoto = hairProfile.TopRightPhoto;
                        strands.TopRightStrandDiameter = hairProfile.TopRightStrandDiameter;
                        strands.TopRightHealthText = hairProfile.TopRightHealthText;

                        strands.BottomLeftPhoto = hairProfile.BottomLeftPhoto;
                        strands.BottomLeftStrandDiameter = hairProfile.BottomLeftStrandDiameter;
                        strands.BottomLeftHealthText = hairProfile.BottomLeftHealthText;

                        strands.BottomRightPhoto = hairProfile.BottomRightPhoto;
                        strands.BottomRightHealthText = hairProfile.BottomRightHealthText;
                        strands.BottomRightStrandDiameter = hairProfile.BottoRightStrandDiameter;

                        strands.CrownPhoto = hairProfile.CrownPhoto;
                        strands.CrownHealthText = hairProfile.CrownHealthText;
                        strands.CrownStrandDiameter = hairProfile.CrownStrandDiameter;

                        strands.HairProfileId = hair.Id;

                        context.Add(strands);
                        context.SaveChanges();

                    }


                    //-------------
                    if (TopLeftImage == null)
                    {

                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopLeftImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();
                    }

                    if (TopLeftImage != null)
                    {
                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopLeftImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();

                        List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                        foreach (var tLeft in TopLeftImage)
                        {
                            if (tLeft != "")
                            {
                                HairStrandsImages hairStrandsImag = new HairStrandsImages();
                                hairStrandsImag.TopLeftImage = tLeft;
                                hairStrandsImag.IsActive = true;
                                hairStrandsImag.CreatedOn = DateTime.Now;
                                hairStrandsImag.Id = strands.Id;
                                lsthairStrands.Add(hairStrandsImag);

                            }
                        }
                        context.AddRange(lsthairStrands);
                        context.SaveChanges();
                    }




                    if (TopRightImage == null)
                    {

                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopRightImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();
                    }
                    if (TopRightImage != null)
                    {
                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.TopRightImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();

                        List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                        foreach (var tRight in TopRightImage)
                        {

                            if (tRight != "")
                            {
                                HairStrandsImages hairStrands = new HairStrandsImages();

                                hairStrands.TopRightImage = tRight;
                                hairStrands.IsActive = true;
                                hairStrands.CreatedOn = DateTime.Now;
                                hairStrands.Id = strands.Id;
                                lsthairStrands.Add(hairStrands);

                            }
                        }
                        context.AddRange(lsthairStrands);
                        context.SaveChanges();
                    }
                    if (BottomLeftImage == null)
                    {

                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.BottomLeftImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();
                    }
                    if (BottomLeftImage != null)
                    {
                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.BottomLeftImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();

                        List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                        foreach (var bLeft in BottomLeftImage)
                        {
                            if (bLeft != "")
                            {
                                HairStrandsImages hairStrands = new HairStrandsImages();

                                hairStrands.BottomLeftImage = bLeft;
                                hairStrands.IsActive = true;
                                hairStrands.CreatedOn = DateTime.Now;
                                hairStrands.Id = strands.Id;
                                lsthairStrands.Add(hairStrands);

                            }
                        }
                        context.AddRange(lsthairStrands);
                        context.SaveChanges();
                    }
                    if (BottomRightImage == null)
                    {

                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.BottomRightImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();
                    }
                    if (BottomRightImage != null)
                    {
                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.BottomRightImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();

                        List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                        foreach (var bRight in BottomRightImage)
                        {
                            if (bRight != "")
                            {
                                HairStrandsImages hairStrands = new HairStrandsImages();

                                hairStrands.BottomRightImage = bRight;
                                hairStrands.IsActive = true;
                                hairStrands.CreatedOn = DateTime.Now;
                                hairStrands.Id = strands.Id;
                                lsthairStrands.Add(hairStrands);

                            }
                        }
                        context.AddRange(lsthairStrands);
                        context.SaveChanges();
                    }
                    if (CrownImage == null)
                    {

                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.CrownImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();
                    }

                    if (CrownImage != null)
                    {
                        List<HairStrandsImages> lstHairStrandsImages = context.HairStrandsImages.Where(x => x.Id == strands.Id && x.CrownImage != null).ToList();
                        context.RemoveRange(lstHairStrandsImages);
                        context.SaveChanges();

                        List<HairStrandsImages> lsthairStrands = new List<HairStrandsImages>();
                        foreach (var crown in CrownImage)
                        {
                            if (crown != "")
                            {
                                HairStrandsImages hairStrands = new HairStrandsImages();

                                hairStrands.CrownImage = crown;
                                hairStrands.IsActive = true;
                                hairStrands.CreatedOn = DateTime.Now;
                                hairStrands.Id = strands.Id;
                                lsthairStrands.Add(hairStrands);
                            }
                        }
                        context.AddRange(lsthairStrands);
                        context.SaveChanges();
                    }

                    if (hairProfile.Health.Count() > 0 || hairProfile.Health.Count() == 0)
                    {
                        List<HairHealth> lst = context.HairHealths.Where(x => x.HairProfileId == hair.Id).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();
                        if (hairProfile.Health.Count() > 0)
                        {
                            List<HairHealth> lstHairHealth = new List<HairHealth>();
                            foreach (var health in hairProfile.Health)
                            {
                                HairHealth hairHealth = new HairHealth();
                                hairHealth.HairProfileId = hair.Id;
                                hairHealth.HealthId = health.Id;
                                hairHealth.IsTopLeft = health.IsTopLeft;
                                hairHealth.IsTopRight = health.IsTopRight;
                                hairHealth.IsBottomLeft = health.IsBottomLeft;
                                hairHealth.IsBottomRight = health.IsBottomRight;
                                hairHealth.IsCrown = health.IsCrown;
                                lstHairHealth.Add(hairHealth);

                            }
                            context.AddRange(lstHairHealth);
                            context.SaveChanges();
                        }
                    }

                    if (hairProfile.Observation.Count() > 0 || hairProfile.Observation.Count() == 0)
                    {
                        List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObservationId != null).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();
                        if (hairProfile.Observation.Count() > 0)
                        {
                            List<HairObservation> lstHairObservation = new List<HairObservation>();
                            foreach (var obser in hairProfile.Observation)
                            {
                                HairObservation hairObservation = new HairObservation();
                                hairObservation.HairProfileId = hair.Id;
                                hairObservation.ObservationId = obser.Id;
                                hairObservation.IsTopLeft = obser.IsTopLeft;
                                hairObservation.IsTopRight = obser.IsTopRight;
                                hairObservation.IsBottomLeft = obser.IsBottomLeft;
                                hairObservation.IsBottomRight = obser.IsBottomRight;
                                hairObservation.IsCrown = obser.IsCrown;
                                lstHairObservation.Add(hairObservation);

                            }
                            context.AddRange(lstHairObservation);
                            context.SaveChanges();
                        }
                    }


                    if (hairProfile.observationElasticityModels.Count() > 0)
                    {
                        List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsElasticityId != null).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();

                        List<HairObservation> lstHairObservation = new List<HairObservation>();
                        foreach (var obser in hairProfile.observationElasticityModels)
                        {
                            HairObservation hairObservation = new HairObservation();
                            hairObservation.HairProfileId = hair.Id;
                            hairObservation.ObsElasticityId = obser.Id;
                            hairObservation.IsTopLeft = obser.IsTopLeft;
                            hairObservation.IsTopRight = obser.IsTopRight;
                            hairObservation.IsBottomLeft = obser.IsBottomLeft;
                            hairObservation.IsBottomRight = obser.IsBottomRight;
                            hairObservation.IsCrown = obser.IsCrown;

                            lstHairObservation.Add(hairObservation);

                        }
                        context.AddRange(lstHairObservation);
                        context.SaveChanges();
                    }

                    if (hairProfile.observationChemicalModels.Count() > 0)
                    {
                        List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsChemicalProductId != null).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();

                        List<HairObservation> lstHairObservation = new List<HairObservation>();
                        foreach (var obser in hairProfile.observationChemicalModels)
                        {
                            HairObservation hairObservation = new HairObservation();
                            hairObservation.HairProfileId = hair.Id;
                            hairObservation.ObsChemicalProductId = obser.Id;
                            hairObservation.IsTopLeft = obser.IsTopLeft;
                            hairObservation.IsTopRight = obser.IsTopRight;
                            hairObservation.IsBottomLeft = obser.IsBottomLeft;
                            hairObservation.IsBottomRight = obser.IsBottomRight;
                            hairObservation.IsCrown = obser.IsCrown;

                            lstHairObservation.Add(hairObservation);
                        }
                        context.AddRange(lstHairObservation);
                        context.SaveChanges();
                    }

                    if (hairProfile.observationPhysicalModels.Count() > 0)
                    {
                        List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsPhysicalProductId != null).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();

                        List<HairObservation> lstHairObservation = new List<HairObservation>();
                        foreach (var obser in hairProfile.observationPhysicalModels)
                        {
                            HairObservation hairObservation = new HairObservation();
                            hairObservation.HairProfileId = hair.Id;
                            hairObservation.ObsPhysicalProductId = obser.Id;
                            hairObservation.IsTopLeft = obser.IsTopLeft;
                            hairObservation.IsTopRight = obser.IsTopRight;
                            hairObservation.IsBottomLeft = obser.IsBottomLeft;
                            hairObservation.IsBottomRight = obser.IsBottomRight;
                            hairObservation.IsCrown = obser.IsCrown;

                            lstHairObservation.Add(hairObservation);
                        }
                        context.AddRange(lstHairObservation);
                        context.SaveChanges();
                    }


                    if (hairProfile.observationBreakageModels.Count() > 0)
                    {
                        List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsBreakageId != null).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();

                        List<HairObservation> lstHairObservation = new List<HairObservation>();
                        foreach (var obser in hairProfile.observationBreakageModels)
                        {
                            HairObservation hairObservation = new HairObservation();
                            hairObservation.HairProfileId = hair.Id;
                            hairObservation.ObsBreakageId = obser.Id;
                            hairObservation.IsTopLeft = obser.IsTopLeft;
                            hairObservation.IsTopRight = obser.IsTopRight;
                            hairObservation.IsBottomLeft = obser.IsBottomLeft;
                            hairObservation.IsBottomRight = obser.IsBottomRight;
                            hairObservation.IsCrown = obser.IsCrown;
                            lstHairObservation.Add(hairObservation);

                        }
                        context.AddRange(lstHairObservation);
                        context.SaveChanges();
                    }

                    if (hairProfile.observationSplittingModels.Count() > 0)
                    {
                        List<HairObservation> lst = context.HairObservations.Where(x => x.HairProfileId == hair.Id && x.ObsSplittingId != null).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();

                        List<HairObservation> lstHairObservation = new List<HairObservation>();
                        foreach (var obser in hairProfile.observationSplittingModels)
                        {
                            HairObservation hairObservation = new HairObservation();
                            hairObservation.HairProfileId = hair.Id;
                            hairObservation.ObsSplittingId = obser.Id;
                            hairObservation.IsTopLeft = obser.IsTopLeft;
                            hairObservation.IsTopRight = obser.IsTopRight;
                            hairObservation.IsBottomLeft = obser.IsBottomLeft;
                            hairObservation.IsBottomRight = obser.IsBottomRight;
                            hairObservation.IsCrown = obser.IsCrown;
                            lstHairObservation.Add(hairObservation);

                        }
                        context.AddRange(lstHairObservation);
                        context.SaveChanges();
                    }

                    if (hairProfile.Pororsity.Count() > 0)
                    {
                        List<HairPorosity> lst = context.HairPorosities.Where(x => x.HairProfileId == hair.Id).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();

                        List<HairPorosity> lstHairPorosity = new List<HairPorosity>();
                        foreach (var poro in hairProfile.Pororsity)
                        {
                            HairPorosity hairPorosity = new HairPorosity();
                            hairPorosity.HairProfileId = hair.Id;
                            hairPorosity.PorosityId = poro.Id;
                            hairPorosity.IsTopLeft = poro.IsTopLeft;
                            hairPorosity.IsTopRight = poro.IsTopRight;
                            hairPorosity.IsBottomLeft = poro.IsBottomLeft;
                            hairPorosity.IsBottomRight = poro.IsBottomRight;
                            hairPorosity.IsCrown = poro.IsCrown;
                            lstHairPorosity.Add(hairPorosity);

                        }
                        context.AddRange(lstHairPorosity);
                        context.SaveChanges();
                    }

                    if (hairProfile.Elasticity.Count() > 0)
                    {
                        List<HairElasticity> lst = context.HairElasticities.Where(x => x.HairProfileId == hair.Id).ToList();
                        context.RemoveRange(lst);
                        context.SaveChanges();

                        List<HairElasticity> lstHairElasticity = new List<HairElasticity>();
                        foreach (var elas in hairProfile.Elasticity)
                        {
                            HairElasticity hairElasticity = new HairElasticity();
                            hairElasticity.HairProfileId = hair.Id;
                            hairElasticity.ElasticityId = elas.Id;
                            hairElasticity.IsTopLeft = elas.IsTopLeft;
                            hairElasticity.IsTopRight = elas.IsTopRight;
                            hairElasticity.IsBottomLeft = elas.IsBottomLeft;
                            hairElasticity.IsBottomRight = elas.IsBottomRight;
                            hairElasticity.IsCrown = elas.IsCrown;
                            lstHairElasticity.Add(hairElasticity);
                        }
                        context.AddRange(lstHairElasticity);
                        context.SaveChanges();
                    }

                }
                if (TabNo.Equals("Tab1"))
                {
                    if (hairProfile.SelectedAnswer != null)
                    {
                        var ansResponse = context.AdditionalHairInfo.Where(x => x.HairId == hair.Id).FirstOrDefault();
                        int AdditonalHairInfoID = 0;
                        if (ansResponse != null)
                        {
                            AdditonalHairInfoID = ansResponse.Id;
                            ansResponse.HairId = hair.Id;
                            ansResponse.TypeId = hairProfile.SelectedAnswer.TypeId;
                            ansResponse.TypeDescription = hairProfile.SelectedAnswer.TypeDescription;
                            ansResponse.TextureId = hairProfile.SelectedAnswer.TextureId;
                            ansResponse.TextureDescription = hairProfile.SelectedAnswer.TextureDescription;
                            ansResponse.PorosityId = hairProfile.SelectedAnswer.PorosityId;
                            ansResponse.PorosityDescription = hairProfile.SelectedAnswer.PorosityDescription;
                            ansResponse.HealthId = hairProfile.SelectedAnswer.HealthId;
                            ansResponse.HealthDescription = hairProfile.SelectedAnswer.HealthDescription;
                            ansResponse.DensityId = hairProfile.SelectedAnswer.DensityId;
                            ansResponse.DensityDescription = hairProfile.SelectedAnswer.DensityDescription;
                            ansResponse.ElasticityId = hairProfile.SelectedAnswer.ElasticityId;
                            ansResponse.ElasticityDescription = hairProfile.SelectedAnswer.ElasticityDescription;
                            context.SaveChanges();

                            List<CustomerHairChallenge> hairChallenges = context.CustomerHairChallenge.Where(x => x.HairInfoId == ansResponse.Id).ToList();
                            List<CustomerHairGoals> hairGoals = context.CustomerHairGoals.Where(x => x.HairInfoId == ansResponse.Id).ToList();
                            context.RemoveRange(hairChallenges);
                            context.RemoveRange(hairGoals);
                            //context.Remove(ansResponse);
                            context.SaveChanges();
                        }
                        else
                        {
                            AdditionalHairInfo additionalHairInfo = new AdditionalHairInfo();
                            additionalHairInfo.HairId = hair.Id;
                            additionalHairInfo.TypeId = hairProfile.SelectedAnswer.TypeId;
                            additionalHairInfo.TypeDescription = hairProfile.SelectedAnswer.TypeDescription;
                            additionalHairInfo.TextureId = hairProfile.SelectedAnswer.TextureId;
                            additionalHairInfo.TextureDescription = hairProfile.SelectedAnswer.TextureDescription;
                            additionalHairInfo.PorosityId = hairProfile.SelectedAnswer.PorosityId;
                            additionalHairInfo.PorosityDescription = hairProfile.SelectedAnswer.PorosityDescription;
                            additionalHairInfo.HealthId = hairProfile.SelectedAnswer.HealthId;
                            additionalHairInfo.HealthDescription = hairProfile.SelectedAnswer.HealthDescription;
                            additionalHairInfo.DensityId = hairProfile.SelectedAnswer.DensityId;
                            additionalHairInfo.DensityDescription = hairProfile.SelectedAnswer.DensityDescription;
                            additionalHairInfo.ElasticityId = hairProfile.SelectedAnswer.ElasticityId;
                            additionalHairInfo.ElasticityDescription = hairProfile.SelectedAnswer.ElasticityDescription;
                            context.Add(additionalHairInfo);
                            context.SaveChanges();
                            AdditonalHairInfoID = additionalHairInfo.Id;
                        }

                        List<CustomerHairGoals> customerHairGoals = new List<CustomerHairGoals>();
                        foreach (var hairGoal in hairProfile.SelectedAnswer.Goals)
                        {
                            if (hairGoal != "")
                            {
                                CustomerHairGoals customerHairGoal = new CustomerHairGoals();
                                customerHairGoal.HairInfoId = AdditonalHairInfoID;//additionalHairInfo.Id;
                                customerHairGoal.Description = hairGoal;
                                customerHairGoal.CreatedOn = DateTime.Now;
                                customerHairGoal.IsActive = true;
                                customerHairGoals.Add(customerHairGoal);
                            }
                        }
                        context.AddRange(customerHairGoals);
                        List<CustomerHairChallenge> customerHairChallenges = new List<CustomerHairChallenge>();
                        foreach (var challenge in hairProfile.SelectedAnswer.Challenges)
                        {
                            if (challenge != "")
                            {
                                CustomerHairChallenge customerHairChallenge = new CustomerHairChallenge();
                                customerHairChallenge.HairInfoId = AdditonalHairInfoID;//additionalHairInfo.Id;
                                customerHairChallenge.Description = challenge;
                                customerHairChallenge.CreatedOn = DateTime.Now;
                                customerHairChallenge.IsActive = true;
                                customerHairChallenges.Add(customerHairChallenge);
                            }
                        }
                        context.AddRange(customerHairChallenges);
                        context.SaveChanges();
                    }
                }

                if (TabNo.Equals("Tab3"))
                {
                    if (hairProfile.RecommendedProducts.Count() == 0)
                    {
                        List<RecommendedProducts> OldProducts = context.RecommendedProducts.Where(x => x.HairProfileId == hair.Id && x.IsAllEssential != true).ToList();
                        context.RemoveRange(OldProducts);
                        context.SaveChanges();
                    }
                    if (hairProfile.RecommendedProducts.Count() > 0)
                    {

                        List<RecommendedProducts> OldProducts = context.RecommendedProducts.Where(x => x.HairProfileId == hair.Id && x.IsAllEssential != true).ToList();

                        List<RecommendedProducts> SelectedProducts = hairProfile.RecommendedProducts.ToList();
                        List<RecommendedProducts> NewProductsToAdd = new List<RecommendedProducts>();
                        NewProductsToAdd = SelectedProducts.Where(item1 => OldProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                        List<RecommendedProducts> DeselectedProducts = new List<RecommendedProducts>();
                        DeselectedProducts = OldProducts.Where(item1 => SelectedProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                        if (DeselectedProducts.Count() > 0)
                        {
                            context.RemoveRange(DeselectedProducts);
                            context.SaveChanges();
                        }
                        if (NewProductsToAdd.Count() > 0)
                        {
                            foreach (var prod in NewProductsToAdd)
                            {

                                prod.IsActive = true;
                                prod.CreatedOn = DateTime.Now;
                                prod.HairProfileId = hair.Id;
                                prod.IsAllEssential = false;

                            }
                            context.AddRange(NewProductsToAdd);
                            context.SaveChanges();
                        }

                    }

                    if (hairProfile.RecommendedProductsStylings.Count() == 0)
                    {
                        List<RecommendedProductsStylingRegimen> OldStylingProducts = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hair.Id && x.IsAllStyling != true).ToList();
                        context.RemoveRange(OldStylingProducts);
                        context.SaveChanges();
                    }
                    if (hairProfile.RecommendedProductsStylings.Count() > 0)
                    {

                        List<RecommendedProductsStylingRegimen> OldStylingProducts = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hair.Id && x.IsAllStyling != true).ToList();

                        List<RecommendedProductsStylingRegimen> SelectedStylingProducts = hairProfile.RecommendedProductsStylings.ToList();
                        List<RecommendedProductsStylingRegimen> NewStylingProductsToAdd = new List<RecommendedProductsStylingRegimen>();
                        NewStylingProductsToAdd = SelectedStylingProducts.Where(item1 => OldStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                        List<RecommendedProductsStylingRegimen> DeselectedStylingProducts = new List<RecommendedProductsStylingRegimen>();
                        DeselectedStylingProducts = OldStylingProducts.Where(item1 => SelectedStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                        if (DeselectedStylingProducts.Count() > 0)
                        {
                            context.RemoveRange(DeselectedStylingProducts);
                            context.SaveChanges();
                        }
                        if (NewStylingProductsToAdd.Count() > 0)
                        {
                            foreach (var prod in NewStylingProductsToAdd)
                            {

                                prod.IsActive = true;
                                prod.CreatedOn = DateTime.Now;
                                prod.HairProfileId = hair.Id;
                                prod.IsAllStyling = false;

                            }
                            context.AddRange(NewStylingProductsToAdd);
                            context.SaveChanges();
                        }


                    }


                    //--------------All Products Essential------------
                    if (hairProfile.AllRecommendedProductsEssential.Count() == 0)
                    {

                        List<RecommendedProducts> OldAllProductsEssen = context.RecommendedProducts.Where(x => x.HairProfileId == hair.Id && x.IsAllEssential == true).ToList();
                        context.RemoveRange(OldAllProductsEssen);
                        context.SaveChanges();
                    }
                    if (hairProfile.AllRecommendedProductsEssential.Count() > 0)
                    {

                        List<RecommendedProducts> OldAllProductsEssen = context.RecommendedProducts.Where(x => x.HairProfileId == hair.Id && x.IsAllEssential == true).ToList();

                        List<RecommendedProducts> SelectedProducts = hairProfile.AllRecommendedProductsEssential.ToList();
                        List<RecommendedProducts> NewEssenProductsToAdd = new List<RecommendedProducts>();
                        NewEssenProductsToAdd = SelectedProducts.Where(item1 => OldAllProductsEssen.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                        List<RecommendedProducts> DeselectedEssenProducts = new List<RecommendedProducts>();
                        DeselectedEssenProducts = OldAllProductsEssen.Where(item1 => SelectedProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                        if (DeselectedEssenProducts.Count() > 0)
                        {
                            context.RemoveRange(DeselectedEssenProducts);
                            context.SaveChanges();
                        }
                        if (NewEssenProductsToAdd.Count() > 0)
                        {
                            foreach (var prod in NewEssenProductsToAdd)
                            {

                                prod.IsActive = true;
                                prod.CreatedOn = DateTime.Now;
                                prod.HairProfileId = hair.Id;
                                prod.IsAllEssential = true;

                            }
                            context.AddRange(NewEssenProductsToAdd);
                            context.SaveChanges();
                        }

                    }

                    //---All Products styling
                    if (hairProfile.AllRecommendedProductsStyling.Count() == 0)
                    {

                        List<RecommendedProductsStylingRegimen> OldAllStylingProducts = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hair.Id && x.IsAllStyling == true).ToList();
                        context.RemoveRange(OldAllStylingProducts);
                        context.SaveChanges();
                    }
                    if (hairProfile.AllRecommendedProductsStyling.Count() > 0)
                    {

                        List<RecommendedProductsStylingRegimen> OldAllStylingProducts = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hair.Id && x.IsAllStyling == true).ToList();

                        List<RecommendedProductsStylingRegimen> SelectedStylingProducts = hairProfile.AllRecommendedProductsStyling.ToList();
                        List<RecommendedProductsStylingRegimen> NewAllStylingProductsToAdd = new List<RecommendedProductsStylingRegimen>();
                        NewAllStylingProductsToAdd = SelectedStylingProducts.Where(item1 => OldAllStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();

                        List<RecommendedProductsStylingRegimen> DeselectedAllStylingProducts = new List<RecommendedProductsStylingRegimen>();
                        DeselectedAllStylingProducts = OldAllStylingProducts.Where(item1 => SelectedStylingProducts.All(item2 => item1.ProductId != item2.ProductId)).ToList();
                        if (DeselectedAllStylingProducts.Count() > 0)
                        {
                            context.RemoveRange(DeselectedAllStylingProducts);
                            context.SaveChanges();
                        }
                        if (NewAllStylingProductsToAdd.Count() > 0)
                        {
                            foreach (var prod in NewAllStylingProductsToAdd)
                            {

                                prod.IsActive = true;
                                prod.CreatedOn = DateTime.Now;
                                prod.HairProfileId = hair.Id;
                                prod.IsAllStyling = true;

                            }
                            context.AddRange(NewAllStylingProductsToAdd);
                            context.SaveChanges();
                        }


                    }
                }

                if (TabNo.Equals("Tab4"))
                {
                    if (hairProfile.RecommendedIngredients.Count() == 0)
                    {
                        List<RecommendedIngredients> OldIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hair.Id).ToList();
                        context.RemoveRange(OldIngredients);
                        context.SaveChanges();
                    }
                    if (hairProfile.RecommendedIngredients.Count() > 0)
                    {
                        List<RecommendedIngredients> OldIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hair.Id).ToList();

                        List<RecommendedIngredients> SelectedIngredients = hairProfile.RecommendedIngredients.ToList();
                        List<RecommendedIngredients> NewIngredientsToAdd = new List<RecommendedIngredients>();
                        NewIngredientsToAdd = SelectedIngredients.Where(item1 => OldIngredients.All(item2 => item1.IngredientId != item2.IngredientId)).ToList();

                        List<RecommendedIngredients> DeselectedIngredients = new List<RecommendedIngredients>();
                        DeselectedIngredients = OldIngredients.Where(item1 => SelectedIngredients.All(item2 => item1.IngredientId != item2.IngredientId)).ToList();
                        if (DeselectedIngredients.Count() > 0)
                        {
                            context.RemoveRange(DeselectedIngredients);
                            context.SaveChanges();
                        }
                        if (NewIngredientsToAdd.Count() > 0)
                        {
                            foreach (var ing in NewIngredientsToAdd)
                            {
                                ing.IsActive = true;
                                ing.CreatedOn = DateTime.Now;
                                ing.HairProfileId = hair.Id;
                                // objIngredient.IngredientId = ing.IngredientId;
                                //objIngredients.Add(objIngredient);
                            }
                            context.AddRange(NewIngredientsToAdd);
                            context.SaveChanges();
                        }
                    }


                    if (hairProfile.RecommendedTools.Count() == 0)
                    {
                        List<RecommendedTools> OldTools = context.RecommendedTools.Where(x => x.HairProfileId == hair.Id).ToList();
                        context.RemoveRange(OldTools);
                        context.SaveChanges();
                    }
                    if (hairProfile.RecommendedTools.Count() > 0)
                    {
                        //------------------------------------------------------


                        List<RecommendedTools> OldTools = context.RecommendedTools.Where(x => x.HairProfileId == hair.Id).ToList();
                        List<RecommendedTools> SelectedTools = hairProfile.RecommendedTools.ToList();

                        List<RecommendedTools> NewToolsToAdd = new List<RecommendedTools>();
                        NewToolsToAdd = SelectedTools.Where(item1 => OldTools.All(item2 => item1.Id != item2.ToolId)).ToList();

                        List<RecommendedTools> DeselectedTools = new List<RecommendedTools>();
                        DeselectedTools = OldTools.Where(item1 => SelectedTools.All(item2 => item1.ToolId != item2.Id)).ToList();
                        if (DeselectedTools.Count() > 0)
                        {
                            context.RemoveRange(DeselectedTools);
                            context.SaveChanges();
                        }
                        if (NewToolsToAdd.Count() > 0)
                        {
                            List<RecommendedTools> lstTools = new List<RecommendedTools>();
                            foreach (var tool in NewToolsToAdd)
                            {

                                RecommendedTools objTools = new RecommendedTools();
                                objTools.Name = tool.Name;
                                objTools.IsActive = true;
                                objTools.CreatedOn = DateTime.Now;
                                objTools.HairProfileId = hair.Id;
                                objTools.ToolId = tool.Id;
                                lstTools.Add(objTools);
                            }
                            context.AddRange(lstTools);
                            context.SaveChanges();
                        }
                        //-----------------------------------------

                    }

                    if (hairProfile.RecommendedRegimens.Count() == 0)
                    {
                        List<RecommendedRegimens> OldRegimens = context.RecommendedRegimens.Where(x => x.HairProfileId == hair.Id).ToList();
                        context.RemoveRange(OldRegimens);
                        context.SaveChanges();
                    }
                    if (hairProfile.RecommendedRegimens.Count() > 0)
                    {
                        List<RecommendedRegimens> OldRegimens = context.RecommendedRegimens.Where(x => x.HairProfileId == hair.Id).ToList();
                        List<RecommendedRegimens> SelectedRegimens = hairProfile.RecommendedRegimens.ToList();

                        List<RecommendedRegimens> NewRegimensToAdd = new List<RecommendedRegimens>();
                        NewRegimensToAdd = SelectedRegimens.Where(item1 => OldRegimens.All(item2 => item1.RegimenId != item2.RegimenId)).ToList();

                        List<RecommendedRegimens> DeselectedRegimens = new List<RecommendedRegimens>();
                        DeselectedRegimens = OldRegimens.Where(item1 => SelectedRegimens.All(item2 => item1.RegimenId != item2.RegimenId)).ToList();

                        if (DeselectedRegimens.Count() > 0)
                        {
                            context.RemoveRange(DeselectedRegimens);
                            context.SaveChanges();
                        }

                        if (NewRegimensToAdd.Count() > 0)
                        {
                            foreach (var regim in NewRegimensToAdd)
                            {
                                regim.IsActive = true;
                                regim.CreatedOn = DateTime.Now;
                                regim.HairProfileId = hair.Id;
                                // regim.RegimenId = regim.RegimenId;

                            }
                            context.AddRange(NewRegimensToAdd);
                            context.SaveChanges();
                        }


                    }

                    if (hairProfile.RecommendedVideos.Count() == 0)
                    {
                        List<RecommendedVideos> OldVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hair.Id).ToList();
                        context.RemoveRange(OldVideos);
                        context.SaveChanges();
                    }
                    if (hairProfile.RecommendedVideos.Count() > 0)
                    {
                        List<RecommendedVideos> OldVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hair.Id).ToList();
                        List<RecommendedVideos> SelectedVideos = hairProfile.RecommendedVideos.ToList();

                        List<RecommendedVideos> NewVideosToAdd = new List<RecommendedVideos>();
                        NewVideosToAdd = SelectedVideos.Where(item1 => OldVideos.All(item2 => item1.MediaLinkEntityId != item2.MediaLinkEntityId)).ToList();

                        List<RecommendedVideos> DeselectedVideos = new List<RecommendedVideos>();
                        DeselectedVideos = OldVideos.Where(item1 => SelectedVideos.All(item2 => item1.MediaLinkEntityId != item2.MediaLinkEntityId)).ToList();

                        if (DeselectedVideos.Count() > 0)
                        {
                            context.RemoveRange(DeselectedVideos);
                            context.SaveChanges();
                        }

                        if (NewVideosToAdd.Count() > 0)
                        {
                            foreach (var vid in NewVideosToAdd)
                            {

                                vid.IsActive = true;
                                vid.CreatedOn = DateTime.Now;
                                vid.HairProfileId = hair.Id;
                                //  vid.MediaLinkEntityId = vid.MediaLinkEntityId;

                            }
                            context.AddRange(NewVideosToAdd);
                            context.SaveChanges();
                        }

                    }

                    if (hairProfile.RecommendedStylists.Count() == 0)
                    {
                        List<RecommendedStylist> OldStylists = context.RecommendedStylists.Where(x => x.HairProfileId == hair.Id).ToList();
                        context.RemoveRange(OldStylists);
                        context.SaveChanges();
                    }
                    if (hairProfile.RecommendedStylists.Count() > 0)
                    {
                        List<RecommendedStylist> OldStylists = context.RecommendedStylists.Where(x => x.HairProfileId == hair.Id).ToList();
                        List<RecommendedStylist> SelectedStylists = hairProfile.RecommendedStylists.ToList();

                        List<RecommendedStylist> NewStylistsToAdd = new List<RecommendedStylist>();
                        NewStylistsToAdd = SelectedStylists.Where(item1 => OldStylists.All(item2 => item1.StylistId != item2.StylistId)).ToList();

                        List<RecommendedStylist> DeselectedStylists = new List<RecommendedStylist>();
                        DeselectedStylists = OldStylists.Where(item1 => SelectedStylists.All(item2 => item1.StylistId != item2.StylistId)).ToList();

                        if (DeselectedStylists.Count() > 0)
                        {
                            context.RemoveRange(DeselectedStylists);
                            context.SaveChanges();
                        }

                        if (NewStylistsToAdd.Count() > 0)
                        {
                            foreach (var styl in NewStylistsToAdd)
                            {

                                styl.IsActive = true;
                                styl.CreatedOn = DateTime.Now;
                                styl.HairProfileId = hair.Id;


                            }
                            context.AddRange(NewStylistsToAdd);
                            context.SaveChanges();
                        }

                    }
                }

                if (hairProfile.NotifyUser.ToLower() == "true") {


                    EmailInformation emailInformation = new EmailInformation
                    {
                        //Code = activationCode,
                        Email = user.Email,
                        Name = user.FirstName + " " + user.LastName,

                    };

                    var emailRes = _emailService.SendEmail("HHCPUPDT", emailInformation);
                }
            }
            return hairProfile;
        }


        public HairProfileAdminModel GetHairProfileAdmin(HairProfileAdminModel hairProfileModel)
        {
            HairProfileAdminModel profile = new HairProfileAdminModel();
            int hairId = context.HairProfiles.Where(x => x.UserId == hairProfileModel.UserId && x.IsActive == true).Select(x => x.Id).FirstOrDefault();
            string TabNo = hairProfileModel.TabNo;
            if (TabNo.Equals("Tab1"))
            {
                profile = new HairProfileAdminModel();
                profile = (from hr in context.HairProfiles
                           where hr.UserId == hairProfileModel.UserId
                           && hr.IsActive == true
                           select new HairProfileAdminModel()
                           {
                               UserId = hr.UserId,
                               HairId = hr.HairId,
                               HealthSummary = hr.HealthSummary,
                               ConsultantNotes = hr.ConsultantNotes,
                           }).FirstOrDefault();
            }
            if(TabNo.Equals("Tab2"))
            {
                profile = new HairProfileAdminModel();
                profile = (from hr in context.HairProfiles
                                                 join st in context.HairStrands
                                                 on hr.Id equals st.HairProfileId
                                                 where hr.UserId == hairProfileModel.UserId
                                                 && hr.IsActive == true
                                                 select new HairProfileAdminModel()
                                                 {
                                                     TopLeft = new TopLeftAdmin()
                                                     {
                                                         TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "").Select(x => x.TopLeftImage).ToList(),

                                                         TopLeftHealthText = st.TopLeftHealthText,
                                                         TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                                         Health = (from hb in context.HairHealths
                                                                   join ob in context.Healths
                                                                   on hb.HealthId equals ob.Id
                                                                   where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                   select new HealthModel()
                                                                   {
                                                                       Id = ob.Id,
                                                                       Description = ob.Description
                                                                   }).ToList(),
                                                         Observation = (from hb in context.HairObservations
                                                                        join ob in context.Observations
                                                                        on hb.ObservationId equals ob.Id
                                                                        where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                        select new Observation()
                                                                        {
                                                                            Id = ob.Id,
                                                                            Description = ob.Description
                                                                        }).ToList(),
                                                         obsElasticities = (from hb in context.HairObservations
                                                                            join ob in context.ObsElasticities
                                                                            on hb.ObsElasticityId equals ob.Id
                                                                            where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                            select new ObsElasticity()
                                                                            {
                                                                                Id = ob.Id,
                                                                                Description = ob.Description
                                                                            }).ToList(),
                                                         obsChemicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsChemicalProducts
                                                                                on hb.ObsChemicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                                select new ObsChemicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsPhysicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsPhysicalProducts
                                                                                on hb.ObsPhysicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                                select new ObsPhysicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsDamages = (from hb in context.HairObservations
                                                                       join ob in context.ObsDamage
                                                                       on hb.ObsDamageId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                       select new ObsDamage()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).ToList(),
                                                         obsBreakages = (from hb in context.HairObservations
                                                                         join ob in context.ObsBreakage
                                                                         on hb.ObsBreakageId equals ob.Id
                                                                         where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                         select new ObsBreakage()
                                                                         {
                                                                             Id = ob.Id,
                                                                             Description = ob.Description
                                                                         }).ToList(),
                                                         obsSplittings = (from hb in context.HairObservations
                                                                          join ob in context.ObsSplitting
                                                                          on hb.ObsSplittingId equals ob.Id
                                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                          select new ObsSplitting()
                                                                          {
                                                                              Id = ob.Id,
                                                                              Description = ob.Description
                                                                          }).ToList(),
                                                         Pororsity = (from hb in context.HairPorosities
                                                                      join ob in context.Pororsities
                                                                      on hb.PorosityId equals ob.Id
                                                                      where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                      select new Pororsity()
                                                                      {
                                                                          Id = ob.Id,
                                                                          Description = ob.Description
                                                                      }).FirstOrDefault(),
                                                         Elasticity = (from hb in context.HairElasticities
                                                                       join ob in context.Elasticities
                                                                       on hb.ElasticityId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                                       select new Elasticity()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).FirstOrDefault()

                                                     },
                                                     TopRight = new TopRightAdmin()
                                                     {
                                                         TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "").Select(x => x.TopRightImage).ToList(),
                                                         TopRightHealthText = st.TopRightHealthText,
                                                         TopRightStrandDiameter = st.TopRightStrandDiameter,
                                                         Health = (from hb in context.HairHealths
                                                                   join ob in context.Healths
                                                                   on hb.HealthId equals ob.Id
                                                                   where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                   select new HealthModel()
                                                                   {
                                                                       Id = ob.Id,
                                                                       Description = ob.Description
                                                                   }).ToList(),
                                                         Observation = (from hb in context.HairObservations
                                                                        join ob in context.Observations
                                                                        on hb.ObservationId equals ob.Id
                                                                        where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                        select new Observation()
                                                                        {
                                                                            Id = ob.Id,
                                                                            Description = ob.Description
                                                                        }).ToList(),
                                                         obsElasticities = (from hb in context.HairObservations
                                                                            join ob in context.ObsElasticities
                                                                            on hb.ObsElasticityId equals ob.Id
                                                                            where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                            select new ObsElasticity()
                                                                            {
                                                                                Id = ob.Id,
                                                                                Description = ob.Description
                                                                            }).ToList(),
                                                         obsChemicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsChemicalProducts
                                                                                on hb.ObsChemicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                                select new ObsChemicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsPhysicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsPhysicalProducts
                                                                                on hb.ObsPhysicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                                select new ObsPhysicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsDamages = (from hb in context.HairObservations
                                                                       join ob in context.ObsDamage
                                                                       on hb.ObsDamageId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                       select new ObsDamage()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).ToList(),
                                                         obsBreakages = (from hb in context.HairObservations
                                                                         join ob in context.ObsBreakage
                                                                         on hb.ObsBreakageId equals ob.Id
                                                                         where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                         select new ObsBreakage()
                                                                         {
                                                                             Id = ob.Id,
                                                                             Description = ob.Description
                                                                         }).ToList(),
                                                         obsSplittings = (from hb in context.HairObservations
                                                                          join ob in context.ObsSplitting
                                                                          on hb.ObsSplittingId equals ob.Id
                                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                          select new ObsSplitting()
                                                                          {
                                                                              Id = ob.Id,
                                                                              Description = ob.Description
                                                                          }).ToList(),
                                                         Pororsity = (from hb in context.HairPorosities
                                                                      join ob in context.Pororsities
                                                                      on hb.PorosityId equals ob.Id
                                                                      where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                      select new Pororsity()
                                                                      {
                                                                          Id = ob.Id,
                                                                          Description = ob.Description
                                                                      }).FirstOrDefault(),
                                                         Elasticity = (from hb in context.HairElasticities
                                                                       join ob in context.Elasticities
                                                                       on hb.ElasticityId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                                       select new Elasticity()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).FirstOrDefault()
                                                     },
                                                     BottomLeft = new BottomLeftAdmin()
                                                     {
                                                         BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "").Select(x => x.BottomLeftImage).ToList(),
                                                         BottomLeftHealthText = st.BottomLeftHealthText,
                                                         BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                                         Health = (from hb in context.HairHealths
                                                                   join ob in context.Healths
                                                                   on hb.HealthId equals ob.Id
                                                                   where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                   select new HealthModel()
                                                                   {
                                                                       Id = ob.Id,
                                                                       Description = ob.Description
                                                                   }).ToList(),
                                                         Observation = (from hb in context.HairObservations
                                                                        join ob in context.Observations
                                                                        on hb.ObservationId equals ob.Id
                                                                        where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                        select new Observation()
                                                                        {
                                                                            Id = ob.Id,
                                                                            Description = ob.Description
                                                                        }).ToList(),
                                                         obsElasticities = (from hb in context.HairObservations
                                                                            join ob in context.ObsElasticities
                                                                            on hb.ObsElasticityId equals ob.Id
                                                                            where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                            select new ObsElasticity()
                                                                            {
                                                                                Id = ob.Id,
                                                                                Description = ob.Description
                                                                            }).ToList(),
                                                         obsChemicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsChemicalProducts
                                                                                on hb.ObsChemicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                                select new ObsChemicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsPhysicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsPhysicalProducts
                                                                                on hb.ObsPhysicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                                select new ObsPhysicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsDamages = (from hb in context.HairObservations
                                                                       join ob in context.ObsDamage
                                                                       on hb.ObsDamageId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                       select new ObsDamage()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).ToList(),
                                                         obsBreakages = (from hb in context.HairObservations
                                                                         join ob in context.ObsBreakage
                                                                         on hb.ObsBreakageId equals ob.Id
                                                                         where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                         select new ObsBreakage()
                                                                         {
                                                                             Id = ob.Id,
                                                                             Description = ob.Description
                                                                         }).ToList(),
                                                         obsSplittings = (from hb in context.HairObservations
                                                                          join ob in context.ObsSplitting
                                                                          on hb.ObsSplittingId equals ob.Id
                                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                          select new ObsSplitting()
                                                                          {
                                                                              Id = ob.Id,
                                                                              Description = ob.Description
                                                                          }).ToList(),
                                                         Pororsity = (from hb in context.HairPorosities
                                                                      join ob in context.Pororsities
                                                                      on hb.PorosityId equals ob.Id
                                                                      where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                      select new Pororsity()
                                                                      {
                                                                          Id = ob.Id,
                                                                          Description = ob.Description
                                                                      }).FirstOrDefault(),
                                                         Elasticity = (from hb in context.HairElasticities
                                                                       join ob in context.Elasticities
                                                                       on hb.ElasticityId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                                       select new Elasticity()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).FirstOrDefault()
                                                     },
                                                     BottomRight = new BottomRightAdmin()
                                                     {
                                                         BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "").Select(x => x.BottomRightImage).ToList(),
                                                         BottomRightHealthText = st.BottomRightHealthText,
                                                         BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                                         Health = (from hb in context.HairHealths
                                                                   join ob in context.Healths
                                                                   on hb.HealthId equals ob.Id
                                                                   where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                   select new HealthModel()
                                                                   {
                                                                       Id = ob.Id,
                                                                       Description = ob.Description
                                                                   }).ToList(),
                                                         Observation = (from hb in context.HairObservations
                                                                        join ob in context.Observations
                                                                        on hb.ObservationId equals ob.Id
                                                                        where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                        select new Observation()
                                                                        {
                                                                            Id = ob.Id,
                                                                            Description = ob.Description
                                                                        }).ToList(),
                                                         obsElasticities = (from hb in context.HairObservations
                                                                            join ob in context.ObsElasticities
                                                                            on hb.ObsElasticityId equals ob.Id
                                                                            where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                            select new ObsElasticity()
                                                                            {
                                                                                Id = ob.Id,
                                                                                Description = ob.Description
                                                                            }).ToList(),
                                                         obsChemicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsChemicalProducts
                                                                                on hb.ObsChemicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                                select new ObsChemicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsPhysicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsPhysicalProducts
                                                                                on hb.ObsPhysicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                                select new ObsPhysicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsDamages = (from hb in context.HairObservations
                                                                       join ob in context.ObsDamage
                                                                       on hb.ObsDamageId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                       select new ObsDamage()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).ToList(),
                                                         obsBreakages = (from hb in context.HairObservations
                                                                         join ob in context.ObsBreakage
                                                                         on hb.ObsBreakageId equals ob.Id
                                                                         where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                         select new ObsBreakage()
                                                                         {
                                                                             Id = ob.Id,
                                                                             Description = ob.Description
                                                                         }).ToList(),
                                                         obsSplittings = (from hb in context.HairObservations
                                                                          join ob in context.ObsSplitting
                                                                          on hb.ObsSplittingId equals ob.Id
                                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                          select new ObsSplitting()
                                                                          {
                                                                              Id = ob.Id,
                                                                              Description = ob.Description
                                                                          }).ToList(),
                                                         Pororsity = (from hb in context.HairPorosities
                                                                      join ob in context.Pororsities
                                                                      on hb.PorosityId equals ob.Id
                                                                      where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                      select new Pororsity()
                                                                      {
                                                                          Id = ob.Id,
                                                                          Description = ob.Description
                                                                      }).FirstOrDefault(),
                                                         Elasticity = (from hb in context.HairElasticities
                                                                       join ob in context.Elasticities
                                                                       on hb.ElasticityId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                                       select new Elasticity()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).FirstOrDefault()
                                                     },
                                                     CrownStrand = new CrownStrandAdmin()
                                                     {
                                                         CrownHealthText = st.CrownHealthText,
                                                         CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "").Select(x => x.CrownImage).ToList(),
                                                         CrownStrandDiameter = st.CrownStrandDiameter,
                                                         Health = (from hb in context.HairHealths
                                                                   join ob in context.Healths
                                                                   on hb.HealthId equals ob.Id
                                                                   where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                   select new HealthModel()
                                                                   {
                                                                       Id = ob.Id,
                                                                       Description = ob.Description
                                                                   }).ToList(),
                                                         Observation = (from hb in context.HairObservations
                                                                        join ob in context.Observations
                                                                        on hb.ObservationId equals ob.Id
                                                                        where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                        select new Observation()
                                                                        {
                                                                            Id = ob.Id,
                                                                            Description = ob.Description
                                                                        }).ToList(),
                                                         obsElasticities = (from hb in context.HairObservations
                                                                            join ob in context.ObsElasticities
                                                                            on hb.ObsElasticityId equals ob.Id
                                                                            where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                            select new ObsElasticity()
                                                                            {
                                                                                Id = ob.Id,
                                                                                Description = ob.Description
                                                                            }).ToList(),
                                                         obsChemicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsChemicalProducts
                                                                                on hb.ObsChemicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                                select new ObsChemicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsPhysicalProducts = (from hb in context.HairObservations
                                                                                join ob in context.ObsPhysicalProducts
                                                                                on hb.ObsPhysicalProductId equals ob.Id
                                                                                where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                                select new ObsPhysicalProducts()
                                                                                {
                                                                                    Id = ob.Id,
                                                                                    Description = ob.Description
                                                                                }).ToList(),
                                                         obsDamages = (from hb in context.HairObservations
                                                                       join ob in context.ObsDamage
                                                                       on hb.ObsDamageId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                       select new ObsDamage()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).ToList(),
                                                         obsBreakages = (from hb in context.HairObservations
                                                                         join ob in context.ObsBreakage
                                                                         on hb.ObsBreakageId equals ob.Id
                                                                         where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                         select new ObsBreakage()
                                                                         {
                                                                             Id = ob.Id,
                                                                             Description = ob.Description
                                                                         }).ToList(),
                                                         obsSplittings = (from hb in context.HairObservations
                                                                          join ob in context.ObsSplitting
                                                                          on hb.ObsSplittingId equals ob.Id
                                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                          select new ObsSplitting()
                                                                          {
                                                                              Id = ob.Id,
                                                                              Description = ob.Description
                                                                          }).ToList(),
                                                         Pororsity = (from hb in context.HairPorosities
                                                                      join ob in context.Pororsities
                                                                      on hb.PorosityId equals ob.Id
                                                                      where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                      select new Pororsity()
                                                                      {
                                                                          Id = ob.Id,
                                                                          Description = ob.Description
                                                                      }).FirstOrDefault(),
                                                         Elasticity = (from hb in context.HairElasticities
                                                                       join ob in context.Elasticities
                                                                       on hb.ElasticityId equals ob.Id
                                                                       where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                                       select new Elasticity()
                                                                       {
                                                                           Id = ob.Id,
                                                                           Description = ob.Description
                                                                       }).FirstOrDefault()
                                                     },
                                                     
                                                 }).FirstOrDefault();
              
            }
            if(TabNo.Equals("Tab4"))
            {
                profile = new HairProfileAdminModel();
                profile.RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hairId).ToList();
                profile.RecommendedIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hairId).ToList();
                profile.RecommendedTools = context.RecommendedTools.Where(x => x.HairProfileId == hairId).ToList();
                profile.RecommendedRegimens = context.RecommendedRegimens.Where(x => x.HairProfileId == hairId).ToList();
                profile.RecommendedStylist = context.RecommendedStylists.Where(x => x.HairProfileId == hairId).ToList(); 
            }

            if (hairId != 0)
            {
                if (TabNo.Equals("Tab3"))
                {
                    profile = new HairProfileAdminModel();
                    //Healthy hair regimens
                    List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == hairId).Select(x => x.ProductId).ToList();
                    List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                    List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsModel> productsTypesList = new List<RecommendedProductsModel>();
                    foreach (var parentProduct in parents)
                    {
                        RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();

                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Select(x => new ProductsModels
                                {
                                    Id = x.Id,
                                    BrandName = x.BrandName,
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(p => p.ProductClassificationId).ToList(),
                                    HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(p => p.HairChallengeId).ToList(),
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesList.Add(productsTypes);
                    }

                    profile.RecommendedProducts = productsTypesList;
                    profile.AllRecommendedProductsEssential = context.RecommendedProducts.Where(x => x.HairProfileId == hairId && x.IsAllEssential == true).ToList();
                    profile.AllRecommendedProductsStyling = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId && x.IsAllStyling == true).ToList();

                    //Styling Regimens Code
                    List<int> rProductIds = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId).Select(x => x.ProductId).ToList();
                    List<int?> pTypes = context.ProductEntities.Where(x => rProductIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                    List<int?> pParentIds = context.ProductTypes.Where(x => pTypes.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var pParents = context.ProductTypeCategories.Where(x => pParentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsStylingModel> productsTypesStylingList = new List<RecommendedProductsStylingModel>();
                    foreach (var parentProduct in pParents)
                    {
                        RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();

                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Select(x => new ProductsStylingModels
                                {
                                    Id = x.Id,
                                    BrandName = x.BrandName,
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(p => p.ProductClassificationId).ToList(),
                                    HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(p => p.HairChallengeId).ToList(),
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesStylingList.Add(productsTypes);
                    }
                    profile.RecommendedProductsStyling = productsTypesStylingList;
                }
                if (TabNo.Equals("Tab1"))
                {
                    QuestionaireSelectedAnswer additionalHairInfo = new QuestionaireSelectedAnswer();
                    AdditionalHairInfo hairInfo = context.AdditionalHairInfo.Where(x => x.HairId == hairId).FirstOrDefault();
                    if (hairInfo != null)
                    {
                        additionalHairInfo.TypeId = hairInfo.TypeId;
                        additionalHairInfo.TypeDescription = hairInfo.TypeDescription;
                        additionalHairInfo.TextureId = hairInfo.TextureId;
                        additionalHairInfo.TextureDescription = hairInfo.TextureDescription;
                        additionalHairInfo.PorosityId = hairInfo.PorosityId;
                        additionalHairInfo.PorosityDescription = hairInfo.PorosityDescription;
                        additionalHairInfo.HealthId = hairInfo.HealthId;
                        additionalHairInfo.HealthDescription = hairInfo.HealthDescription;
                        additionalHairInfo.DensityId = hairInfo.DensityId;
                        additionalHairInfo.DensityDescription = hairInfo.DensityDescription;
                        additionalHairInfo.ElasticityId = hairInfo.ElasticityId;
                        additionalHairInfo.ElasticityDescription = hairInfo.ElasticityDescription;
                        additionalHairInfo.Goals = context.CustomerHairGoals.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                        additionalHairInfo.Challenges = context.CustomerHairChallenge.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                        profile.SelectedAnswers = additionalHairInfo;
                    }
                }
            }
            return profile;
        }

        public QuestionaireSelectedAnswer GetQuestionaireAnswer(QuestionaireSelectedAnswer hairProfileModel)
        {
            try
            {
                var userid = context.Users.Where(x => x.UserName == hairProfileModel.UserEmail).Select(y => y.Id).FirstOrDefault();
                QuestionaireSelectedAnswer selectedAnswer = new QuestionaireSelectedAnswer();
                var result = context.Questionaires.Include(x => x.Answer).Where(x => (x.QuestionId == 16 || x.QuestionId == 25) && x.UserId == userid.ToString() ).ToList();
                selectedAnswer.Goals = result.Where(y => y.QuestionId == 25).Select(x => x.Answer.Description).ToList();
                selectedAnswer.Challenges = result.Where(y => y.QuestionId == 16).Select(x => x.Answer.Description).ToList();

                return selectedAnswer;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public List<RecommendedRegimensCustomer> RecommendedRegimensCustomer(int hairId)
        {
            List<RecommendedRegimensCustomer> recommendedRegimensModelList = new List<RecommendedRegimensCustomer>();
            try
            {
                List<int> regimenIdList = context.RecommendedRegimens.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.RegimenId).ToList();
                foreach (int regimenId in regimenIdList)
                {
                    Regimens regimens = context.Regimens.Where(x => x.RegimensId == regimenId).FirstOrDefault();
                    if (regimens != null)
                    {
                        RegimenSteps regimenList = context.RegimenSteps.Where(z => z.RegimenStepsId == regimens.RegimenStepsId).FirstOrDefault();

                        RegimenSteps regimenSteps = regimenList;

                        List<RegimenStepsModel> regimenStepsModels = GetRegimenSteps(regimenSteps);

                        RecommendedRegimensCustomer recommendedRegimensModel = new RecommendedRegimensCustomer();
                        recommendedRegimensModel.RegimenId = regimens.RegimensId;
                        recommendedRegimensModel.RegimenName = regimens.Name;
                        recommendedRegimensModel.RegimenSteps = regimenStepsModels;
                        recommendedRegimensModel.RegimenTitle = regimens.Title;
                        recommendedRegimensModel.Description = regimens.Description;

                        recommendedRegimensModelList.Add(recommendedRegimensModel);
                    }
                }
            }
            catch (Exception ex) { }

            return recommendedRegimensModelList;
        }


        public async Task<HairProfileCustomerModel> GetHairProfileCustomer(HairProfileCustomerModel hairProfileModel)
        {
            HairProfileCustomerModel profile = new HairProfileCustomerModel();
            var userName = await _userManager.FindByIdAsync(hairProfileModel.UserId);
            
            try
            {
                profile = (from hr in context.HairProfiles
                           join sts in context.HairStrands
                           on hr.Id equals sts.HairProfileId into hs from st in hs.DefaultIfEmpty()
                           where hr.UserId == userName.UserName
                           && hr.IsActive == true && hr.IsDrafted == false
                           select new HairProfileCustomerModel()
                           {
                               UserId = hr.UserId,
                               HairId = hr.HairId,
                               UserName = userName.FirstName + " " + userName.LastName,
                               AIResult = userName.AIResult,
                               HealthSummary = hr.HealthSummary,
                               ConsultantNotes = hr.ConsultantNotes,
                               TopLeft = st != null ? new TopLeftAdmin()
                               {
                                   TopLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopLeftImage != null && x.TopLeftImage != "").Select(x => ("http://admin.myavana.com/HairProfile/" + x.TopLeftImage).Replace(" ","")).ToList(),
                                   TopLeftHealthText = st.TopLeftHealthText,
                                   TopLeftStrandDiameter = st.TopLeftStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsTopLeft == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()

                               } : new TopLeftAdmin(),
                               TopRight = st != null ? new TopRightAdmin()
                               {
                                   TopRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.TopRightImage != null && x.TopRightImage != "").Select(x => ("http://admin.myavana.com/HairProfile/" + x.TopRightImage).Replace(" ","")).ToList(),
                                   TopRightHealthText = st.TopRightHealthText,
                                   TopRightStrandDiameter = st.TopRightStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsTopRight == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } :  new TopRightAdmin(),
                               BottomLeft = st != null ? new BottomLeftAdmin() 
                               {
                                   BottomLeftPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomLeftImage != null && x.BottomLeftImage != "").Select(x => ("http://admin.myavana.com/HairProfile/" + x.BottomLeftImage).Replace(" ","")).ToList(),
                                   BottomLeftHealthText = st.BottomLeftHealthText,
                                   BottomLeftStrandDiameter = st.BottomLeftStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsBottomLeft == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new BottomLeftAdmin(),
                               BottomRight = st != null  ? new BottomRightAdmin()
                               {
                                   BottomRightPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.BottomRightImage != null && x.BottomRightImage != "").Select(x => ("http://admin.myavana.com/HairProfile/" + x.BottomRightImage).Replace(" ","")).ToList(),
                                   BottomRightHealthText = st.BottomRightHealthText,
                                   BottomRightStrandDiameter = st.BottomRightStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsBottomRight == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new BottomRightAdmin(),
                               CrownStrand = st != null ? new CrownStrandAdmin()
                               {
                                   CrownHealthText = st.CrownHealthText,
                                   CrownPhoto = context.HairStrandsImages.Where(x => x.Id == st.Id && x.CrownImage != null && x.CrownImage != "").Select(x => ("http://admin.myavana.com/HairProfile/" + x.CrownImage).Replace(" ","")).ToList(),
                                   CrownStrandDiameter = st.CrownStrandDiameter,
                                   Health = (from hb in context.HairHealths
                                             join ob in context.Healths
                                             on hb.HealthId equals ob.Id
                                             where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                             select new HealthModel()
                                             {
                                                 Id = ob.Id,
                                                 Description = ob.Description
                                             }).ToList(),
                                   Observation = (from hb in context.HairObservations
                                                  join ob in context.Observations
                                                  on hb.ObservationId equals ob.Id
                                                  where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                  select new Observation()
                                                  {
                                                      Id = ob.Id,
                                                      Description = ob.Description
                                                  }).ToList(),
                                   obsElasticities = (from hb in context.HairObservations
                                                      join ob in context.ObsElasticities
                                                      on hb.ObsElasticityId equals ob.Id
                                                      where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                      select new ObsElasticity()
                                                      {
                                                          Id = ob.Id,
                                                          Description = ob.Description
                                                      }).ToList(),
                                   obsChemicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsChemicalProducts
                                                          on hb.ObsChemicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                          select new ObsChemicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   obsPhysicalProducts = (from hb in context.HairObservations
                                                          join ob in context.ObsPhysicalProducts
                                                          on hb.ObsPhysicalProductId equals ob.Id
                                                          where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                          select new ObsPhysicalProducts()
                                                          {
                                                              Id = ob.Id,
                                                              Description = ob.Description
                                                          }).ToList(),
                                   //obsDamages = (from hb in context.HairObservations
                                   //              join ob in context.ObsDamage
                                   //              on hb.ObsDamageId equals ob.Id
                                   //              where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                   //              select new ObsDamage()
                                   //              {
                                   //                  Id = ob.Id,
                                   //                  Description = ob.Description
                                   //              }).ToList(),
                                   obsBreakages = (from hb in context.HairObservations
                                                   join ob in context.ObsBreakage
                                                   on hb.ObsBreakageId equals ob.Id
                                                   where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                   select new ObsBreakage()
                                                   {
                                                       Id = ob.Id,
                                                       Description = ob.Description
                                                   }).ToList(),
                                   obsSplittings = (from hb in context.HairObservations
                                                    join ob in context.ObsSplitting
                                                    on hb.ObsSplittingId equals ob.Id
                                                    where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                    select new ObsSplitting()
                                                    {
                                                        Id = ob.Id,
                                                        Description = ob.Description
                                                    }).ToList(),
                                   Pororsity = (from hb in context.HairPorosities
                                                join ob in context.Pororsities
                                                on hb.PorosityId equals ob.Id
                                                where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                select new Pororsity()
                                                {
                                                    Id = ob.Id,
                                                    Description = ob.Description
                                                }).FirstOrDefault(),
                                   Elasticity = (from hb in context.HairElasticities
                                                 join ob in context.Elasticities
                                                 on hb.ElasticityId equals ob.Id
                                                 where hb.HairProfileId == hr.Id && hb.IsCrown == true
                                                 select new Elasticity()
                                                 {
                                                     Id = ob.Id,
                                                     Description = ob.Description
                                                 }).FirstOrDefault()
                               } : new CrownStrandAdmin(),
                               RecommendedVideos = context.RecommendedVideos.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedVideosCustomer
                               {
                                   Id = s.Id,
                                   MediaLinkEntityId = s.MediaLinkEntityId,
                                   HairProfileId = s.HairProfileId,
                                   // Videos = context.MediaLinkEntities.Where(x => x.MediaLinkEntityId == s.MediaLinkEntityId).Select(x => x.VideoId).ToList().ToString().Replace("watch","embed")
                                   Videos = (from media in context.MediaLinkEntities
                                             where media.MediaLinkEntityId == s.MediaLinkEntityId
                                             select media.VideoId.ToString().Replace("watch?v=", "embed/")).ToList()
                               }).ToList(),
                               RecommendedIngredients = context.RecommendedIngredients.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedIngredientsCustomer
                               {
                                   Id = s.Id,
                                   IngredientId = s.IngredientId,
                                   HairProfileId = s.HairProfileId,
                                   Ingredients = context.IngedientsEntities.Where(x => x.IngedientsEntityId == s.IngredientId).Select(x => new IngredientsModels
                                   {
                                       Name = x.Name,
                                       ImageName = "http://admin.myavana.com/Ingredients/" + x.Image,
                                       Description = x.Description,
                                   }).ToList()
                               }).ToList(),
                               //--------------------------------------------
                               RecommendedTools = context.RecommendedTools.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedToolsCustomer
                               {
                                   Id = s.Id,
                                   ToolId = s.ToolId,
                                   HairProfileId = s.HairProfileId,
                                   ToolList = context.Tools.Where(x => x.Id == s.ToolId).Select(x => new ToolsModels
                                   {
                                       Name = x.ToolName,
                                       ImageName =  x.Image,
                                       ToolDetail =x.ToolDetails
                                   }).ToList()
                               }).ToList(),


                               //  ImageName = "https://localhost:44322/tools/" + x.Image,  
                               //---------------------------------------------
                               recommendedStylistCustomers = context.RecommendedStylists.Where(x => x.HairProfileId == hr.Id).OrderByDescending(x => x.CreatedOn)
                               .Select(s => new RecommendedStylistCustomer
                               {
                                   Id = s.Id,
                                   StylistId = s.StylistId,
                                   HairProfileId = s.HairProfileId,
                                   Stylist = context.Stylists.Where(x => x.StylistId == s.StylistId).Select(x => new StylistCustomerModel
                                   {
                                       StylistName = x.StylistName,
                                       Salon = x.SalonName,
                                       Email = x.Email,
                                       Phone = x.PhoneNumber,
                                       Website = x.Website,
                                       Instagram = x.Instagram
                                   }).ToList()
                               }).ToList()

                           }).FirstOrDefault();
            }
            catch (Exception ex) {
                throw ex;
            }

            int hairId = context.HairProfiles.Where(x => x.UserId == userName.UserName && x.IsActive == true && x.IsDrafted == false).Select(x => x.Id).FirstOrDefault();
            if (hairId != 0)
            {
                try
                {
                    profile.RecommendedRegimens = RecommendedRegimensCustomer(hairId);

                    List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.ProductId).ToList();
                    List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                    List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsCustomer> productsTypesList = new List<RecommendedProductsCustomer>();
                    foreach (var parentProduct in parents)
                    {
                        RecommendedProductsCustomer productsTypes = new RecommendedProductsCustomer();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();

                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsModels
                                {
                                    Id = x.Id,
                                    BrandName = x.BrandName,
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductClassifications = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.ProductClassificationId != null).Select(p => p.ProductClassificationId).ToList(),
                                    HairChallenges = context.ProductCommons.Where(p => p.ProductEntityId == x.Id && p.HairChallengeId != null).Select(p => p.HairChallengeId).ToList(),
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesList.Add(productsTypes);
                    }

                    profile.RecommendedProducts = productsTypesList;


                    //Styling Regimens Code
                    List<int> rProductIds = context.RecommendedProductsStyleRegimens.Where(x => x.HairProfileId == hairId).OrderByDescending(x => x.CreatedOn).Select(x => x.ProductId).ToList();
                    List<int?> pTypes = context.ProductEntities.Where(x => rProductIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
                    List<int?> pParentIds = context.ProductTypes.Where(x => pTypes.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                    var pParents = context.ProductTypeCategories.Where(x => pParentIds.Contains(x.Id)).ToList();
                    List<RecommendedProductsStylingModel> productsTypesStylingList = new List<RecommendedProductsStylingModel>();
                    foreach (var parentProduct in pParents)
                    {
                        RecommendedProductsStylingModel productsTypes = new RecommendedProductsStylingModel();
                        productsTypes.ProductParentName = parentProduct.CategoryName;
                        productsTypes.ProductId = parentProduct.Id;
                        List<ProductsTypesStylingModels> productsTypesModels = new List<ProductsTypesStylingModels>();
                        List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id))
                            .Select(x => x.ProductTypesId).Distinct().ToList();

                        foreach (var type in productByTypes)
                        {
                            if (type != null)
                            {
                                ProductsTypesStylingModels productsTypesModel = new ProductsTypesStylingModels();
                                var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && rProductIds.Contains(x.Id)).ToList();
                                if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                                {
                                    productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                                    productsTypesModel.ProductId = type;
                                }

                                productsTypesModel.Products = products.Where(x => x.IsActive == true).Select(x => new ProductsStylingModels
                                {
                                    Id = x.Id,
                                    BrandName = x.BrandName,
                                    ImageName = x.ImageName,
                                    ProductLink = x.ProductLink,
                                    ProductDetails = x.ProductDetails,
                                    ProductName = x.ProductName,
                                    ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                                }).ToList();
                                productsTypesModels.Add(productsTypesModel);
                            }
                        }
                        productsTypes.ProductsTypes = productsTypesModels;
                        productsTypesStylingList.Add(productsTypes);
                    }
                    profile.RecommendedProductsStyling = productsTypesStylingList;

                    QuestionaireSelectedAnswer additionalHairInfo = new QuestionaireSelectedAnswer();
                    AdditionalHairInfo hairInfo = context.AdditionalHairInfo.Where(x => x.HairId == hairId).FirstOrDefault();
                    if (hairInfo != null)
                    {
                        additionalHairInfo.TypeId = hairInfo.TypeId;
                        additionalHairInfo.TypeDescription = hairInfo.TypeDescription;
                        additionalHairInfo.TextureId = hairInfo.TextureId;
                        additionalHairInfo.TextureDescription = hairInfo.TextureDescription;
                        additionalHairInfo.PorosityId = hairInfo.PorosityId;
                        additionalHairInfo.PorosityDescription = hairInfo.PorosityDescription;
                        additionalHairInfo.HealthId = hairInfo.HealthId;
                        additionalHairInfo.HealthDescription = hairInfo.HealthDescription;
                        additionalHairInfo.DensityId = hairInfo.DensityId;
                        additionalHairInfo.DensityDescription = hairInfo.DensityDescription;
                        additionalHairInfo.ElasticityId = hairInfo.ElasticityId;
                        additionalHairInfo.ElasticityDescription = hairInfo.ElasticityDescription;
                        additionalHairInfo.Goals = context.CustomerHairGoals.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                        additionalHairInfo.Challenges = context.CustomerHairChallenge.Where(x => x.HairInfoId == hairInfo.Id).Select(y => y.Description).ToList();
                        profile.SelectedAnswers = additionalHairInfo;
                    }
                }
                catch (Exception ex) { }
            }
            if (profile != null)
            {
                string uploadedImage = context.Questionaires.Where(x => x.UserId == hairProfileModel.UserId && x.QuestionId == 22).Select(x => x.DescriptiveAnswer).FirstOrDefault();
                profile.UserUploadedImage = uploadedImage;
            }
            return profile;
        }

        public QuestionaireModel GetQuestionaireDetails(QuestionaireModel questionaire)
        {
            try
            {
                var objCode = context.Questionaires.FirstOrDefault(x => x.UserId == questionaire.Userid && x.IsActive == true);

                if (objCode != null)
                    questionaire.IsExist = true;
                else
                    questionaire.IsExist = false;


            }
            catch (Exception Ex)
            {

            }
            return questionaire;
        }

        public async Task<QuestionaireModel> GetQuestionaireDetailsMobile(string userId)
        {
            QuestionaireModel questionaire = new QuestionaireModel();

            try
            {
                var user = await _userManager.FindByEmailAsync(userId);

                var objCode = context.Questionaires.FirstOrDefault(x => x.UserId == user.Id.ToString() && x.IsActive == true);

                if (objCode != null)
                {
                    questionaire.Userid = user.Id.ToString();
                    questionaire.IsExist = true;
                }
                else
                {
                    questionaire.Userid = user.Id.ToString();
                    questionaire.IsExist = false;
                }
            }
            catch (Exception Ex)
            {

            }
            return questionaire;
        }

        public CollaboratedDetailModelLocal CollaboratedDetailsLocal(string hairProfileId)
        {
            int profileId = Convert.ToInt32(hairProfileId);
            CollaboratedDetailModelLocal collaboratedDetailModel = new CollaboratedDetailModelLocal();

            List<int> productIds = context.RecommendedProducts.Where(x => x.HairProfileId == profileId).Select(x => x.ProductId).ToList();
            List<int?> types = context.ProductEntities.Where(x => productIds.Contains(x.Id)).Select(x => x.ProductTypesId).Distinct().ToList();
            List<int?> parentIds = context.ProductTypes.Where(x => types.Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
            var parents = context.ProductTypeCategories.Where(x => parentIds.Contains(x.Id)).ToList();
            List<RecommendedProductsModel> productsTypesList = new List<RecommendedProductsModel>();
            foreach (var parentProduct in parents)
            {
                RecommendedProductsModel productsTypes = new RecommendedProductsModel();
                productsTypes.ProductParentName = parentProduct.CategoryName;
                productsTypes.ProductId = parentProduct.Id;
                List<ProductsTypesModels> productsTypesModels = new List<ProductsTypesModels>();
                List<int?> productByTypes = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id))
                    .Select(x => x.ProductTypesId).Distinct().ToList();

                foreach (var type in productByTypes)
                {
                    if (type != null)
                    {
                        ProductsTypesModels productsTypesModel = new ProductsTypesModels();
                        var products = context.ProductEntities.Include(x => x.ProductTypes).Where(x => x.ProductTypesId == type && x.ProductTypes.ParentId == parentProduct.Id && productIds.Contains(x.Id)).ToList();
                        if (products.Select(x => x.ProductTypes).FirstOrDefault() != null)
                        {
                            productsTypesModel.ProductTypeName = products.Select(x => x.ProductTypes.ProductName).FirstOrDefault();
                            productsTypesModel.ProductId = type;
                        }

                        productsTypesModel.Products = products.Select(x => new ProductsModels
                        {
                            Id = x.Id,
                            BrandName = x.BrandName,
                            ImageName = x.ImageName,
                            ProductLink = x.ProductLink,
                            ProductDetails = x.ProductDetails,
                            ProductName = x.ProductName,
                            ProductType = context.ProductTypes.Where(y => y.Id == x.ProductTypesId).Select(y => y.ProductName).FirstOrDefault()
                        }).ToList();
                        productsTypesModels.Add(productsTypesModel);
                    }
                }
                productsTypes.ProductsTypes = productsTypesModels;
                productsTypesList.Add(productsTypes);
            }

            collaboratedDetailModel.ProductDetailModel = productsTypesList;

            collaboratedDetailModel.IngredientDetailModel = (from rprod in context.RecommendedIngredients
                                                             join ing in context.IngedientsEntities
                                                             on rprod.IngredientId equals ing.IngedientsEntityId
                                                             where rprod.HairProfileId == profileId
                                                             select new IngredientDetailModel()
                                                             {
                                                                 IngredientId = ing.IngedientsEntityId,
                                                                 Name = ing.Name,
                                                                 ImageUrl = "http://admin.myavana.com/Ingredients/" + ing.Image
                                                             }).ToList();
            collaboratedDetailModel.RegimenDetailModel = (from rprod in context.RecommendedRegimens
                                                          join reg in context.Regimens
                                                          on rprod.RegimenId equals reg.RegimensId
                                                          where rprod.HairProfileId == profileId
                                                          select new RegimenDetailModel()
                                                          {
                                                              RegimenId = reg.RegimensId,
                                                              RegimenName = reg.Name
                                                          }).ToList();
            return collaboratedDetailModel;
        }

        public List<HairProfileCustomersModel> GetHairProfileCustomerList()
        {
            try
            {
                List<HairProfileCustomersModel> list = (from hr in context.HairProfiles
                                                        join usr in context.Users
                                                        on hr.UserId equals usr.UserName
                                                        where hr.IsActive == true && hr.IsDrafted == false
                                                        select new HairProfileCustomersModel()
                                                        {
                                                            UserId = usr.Id,
                                                            UserName = usr.FirstName + " " + usr.LastName,
                                                            UserEmail = usr.Email,
                                                            CreatedOn = hr.CreatedOn,
                                                            CustomerType = usr.CustomerType
                                                        }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ObsCommonData> GetData(int hairId, string type)
        {
            List<ObsCommonData> commonData = new List<ObsCommonData>();

            if (type == "topLeft")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsTopLeft == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsTopLeft == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsTopLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsTopLeft == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            if (type == "topRight")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsTopRight == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsTopRight == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsTopRight == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsTopRight == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsTopRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsTopRight == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            if (type == "bottomLeft")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsBottomLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsBottomLeft == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            if (type == "bottomRight")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsBottomRight == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsBottomRight == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsBottomRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsBottomRight == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            if (type == "crown")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsCrown == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsCrown == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsCrown == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsCrown == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsCrown == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsCrown == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsCrown == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsCrown == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsCrown == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            return commonData;
        }


        public List<ObsCommonData> GetData2(int hairId, string type)
        {
            List<ObsCommonData> commonData = new List<ObsCommonData>();

            if (type == "topLeft")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsTopLeft == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                //var damage = (from hb in context.HairObservations
                //              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                //              where hb.HairProfileId == hairId && hb.IsTopLeft == true
                //              select new ObsCommonData()
                //              {
                //                  Id = ob.Id,
                //                  Name = "Damage",
                //                  Description = ob.Description
                //              }).FirstOrDefault();
                //if (damage != null)
                //{
                //    commonData.Add(damage);
                //}

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsTopLeft == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsTopLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsTopLeft == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            if (type == "topRight")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsTopRight == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsTopRight == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsTopRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsTopRight == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsTopRight == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsTopRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsTopRight == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            if (type == "bottomLeft")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsBottomLeft == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsBottomLeft == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsBottomLeft == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            if (type == "bottomRight")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsBottomRight == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsBottomRight == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsBottomRight == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsBottomRight == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsBottomRight == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            if (type == "crown")
            {
                //var observation = (from hb in context.HairObservations
                //                   join ob in context.Observations on hb.ObservationId equals ob.Id
                //                   where hb.HairProfileId == hairId && hb.IsCrown == true
                //                   select new ObsCommonData()
                //                   {
                //                       Id = ob.Id,
                //                       Name = "Observation",
                //                       Description = ob.Description
                //                   }).FirstOrDefault();
                //commonData.Add(observation);

                var breakage = (from hb in context.HairObservations
                                join ob in context.ObsBreakage on hb.ObsBreakageId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsCrown == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Breakage",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (breakage != null)
                {
                    commonData.Add(breakage);
                }

                var damage = (from hb in context.HairObservations
                              join ob in context.ObsDamage on hb.ObsDamageId equals ob.Id
                              where hb.HairProfileId == hairId && hb.IsCrown == true
                              select new ObsCommonData()
                              {
                                  Id = ob.Id,
                                  Name = "Damage",
                                  Description = ob.Description
                              }).FirstOrDefault();
                if (damage != null)
                {
                    commonData.Add(damage);
                }

                var chemical = (from hb in context.HairObservations
                                join ob in context.ObsChemicalProducts on hb.ObsChemicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsCrown == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Chemical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (chemical != null)
                {
                    commonData.Add(chemical);
                }

                var physical = (from hb in context.HairObservations
                                join ob in context.ObsPhysicalProducts on hb.ObsPhysicalProductId equals ob.Id
                                where hb.HairProfileId == hairId && hb.IsCrown == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Physical",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (physical != null)
                {
                    commonData.Add(physical);
                }

                var splitting = (from hb in context.HairObservations
                                 join ob in context.ObsSplitting on hb.ObsPhysicalProductId equals ob.Id
                                 where hb.HairProfileId == hairId && hb.IsCrown == true
                                 select new ObsCommonData()
                                 {
                                     Id = ob.Id,
                                     Name = "Splitting",
                                     Description = ob.Description
                                 }).FirstOrDefault();
                if (splitting != null)
                {
                    commonData.Add(splitting);
                }

                var elasticity = (from hb in context.HairObservations
                                  join ob in context.ObsElasticities on hb.ObsElasticityId equals ob.Id
                                  where hb.HairProfileId == hairId && hb.IsCrown == true
                                  select new ObsCommonData()
                                  {
                                      Id = ob.Id,
                                      Name = "Elasticitiy",
                                      Description = ob.Description
                                  }).FirstOrDefault();
                if (elasticity != null)
                {
                    commonData.Add(elasticity);
                }

                var porosity = (from hp in context.HairPorosities
                                join ob in context.Pororsities on hp.PorosityId equals ob.Id
                                where hp.HairProfileId == hairId && hp.IsCrown == true
                                select new ObsCommonData()
                                {
                                    Id = ob.Id,
                                    Name = "Porosity",
                                    Description = ob.Description
                                }).FirstOrDefault();
                if (porosity != null)
                {
                    commonData.Add(porosity);
                }

                var density = (from he in context.HairElasticities
                               join ob in context.Elasticities on he.ElasticityId equals ob.Id
                               where he.HairProfileId == hairId && he.IsCrown == true
                               select new ObsCommonData()
                               {
                                   Id = ob.Id,
                                   Name = "Density",
                                   Description = ob.Description
                               }).FirstOrDefault();
                if (density != null)
                {
                    commonData.Add(density);
                }
            }
            return commonData;
        }
    }
}
