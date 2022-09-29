using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;
        private readonly IBaseBusiness _baseBusiness;
        private readonly IHostingEnvironment _environment;
        private readonly HttpClient _httpClient;
        private readonly AvanaContext _context;
        public ProductsController(IProductsService productService, IBaseBusiness baseBusiness, AvanaContext avanaContext)
        {
            _productService = productService;
            _baseBusiness = baseBusiness;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            _context = avanaContext;
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            List<ProductsModelList> result = _productService.GetProducts();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetBrands")]
        public IActionResult GetBrands()
        {
            List<ProductsModelList> result = _productService.GetBrands();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveProducts")]
        public JObject SaveProducts(ProductEntityModel productEntity)
        {
            ProductEntityModel result = _productService.SaveProducts(productEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetProductById")]
        public JObject GetProductById(ProductEntityEditModel productEntity)
        {
            var result = _productService.GetProductById(productEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteProduct")]
        public JObject DeleteProduct(ProductEntity productEntity)
        {
            bool result = _productService.DeleteProduct(productEntity);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", productEntity);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }


        [HttpGet("GetProductType")]
        public IActionResult GetProductsType()
        {
            List<ProductTypeCategoryModelList> result = _productService.GetProductsType();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });

        }

        [HttpPost("SaveProductType")]
        public JObject SaveProductType(ProductTypeCategoryModel productType)
        {
            ProductTypeCategoryModel result = _productService.SaveProductType(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("SaveProductCategory")]
        public JObject SaveProductCategory(ProductTypeCategoriesList productType)
        {
            ProductTypeCategoriesList result = _productService.SaveProductCategory(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }


        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            ProductsListings result = _productService.GetAllProducts();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpPost("GetProductTypeById")]
        public JObject GetProductTypeById(ProductType productType)
        {
            ProductType result = _productService.GetProductTypeById(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteProductType")]
        public JObject DeleteProductType(ProductType productType)
        {
            bool result = _productService.DeleteProductType(productType);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", productType);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteProductCategory")]
        public JObject DeleteProductCategory(ProductType productType)
        {
            bool result = _productService.DeleteProductCategory(productType);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", productType);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("GetProductTypes")]
        public IActionResult GetProductTypes()
        {
            List<ProductTypesList> result = _productService.GetProductTypes();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("GetProductCategoryById")]
        public JObject GetProductCategoryById(ProductTypeCategoriesList productType)
        {
            ProductTypeCategoriesList result = _productService.GetProductCategoryById(productType);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetProductsCategoryList")]
        public IActionResult GetProductsCategoryList()
        {
            List<ProductTypeCategoriesList> result = _productService.GetProductsCategoryList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("AddProductList")]
        public JObject AddProductList(IEnumerable<ProductEntityModel> productData)
        {
            var result = _productService.AddProductList(productData);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("UploadFile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UploadFile([FromBody] Imagerequest imagerequest)
        {
            var file = Request.Form.Files[0];
            //UserEntity entity;
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            //if (token != "")
            //{
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
            string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
            //}
            var result = _productService.UploadFile(file, entity);
            if (result.success)
                return Ok(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [Route("FileUpload")]
        public JObject FileUpload(fileData file)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(file.access_token);
                var tokenS = handler.ReadToken(file.access_token) as JwtSecurityToken;
                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
                if (entity != null)
                {
                    UserEntity us = _context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                    us.ImageURL = "http://admin.myavana.com/imageUpload/" + file.ImageURL;
                    _context.SaveChanges();
                }
                file.user_name = entity.UserName;
                file.Email = entity.Email;
                file.Name = entity.FirstName + " " + entity.LastName;
                file.AccountNo = entity.AccountNo;
                file.TwoFactor = entity.TwoFactorEnabled;
                file.HairType = entity.HairType;
                file.ImageURL = "http://admin.myavana.com/imageUpload/" + file.ImageURL;
                return _baseBusiness.AddDataOnJson("Success", "1", file);
            }
            catch (Exception Ex)
            {
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
            }
        }

        [HttpGet("GetIngredientsList")]
        public IActionResult GetIngredientsList()
        {
            List<IngedientsEntity> result = _productService.GetIngredientsList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetHairTypesList")]
        public IActionResult GetHairTypesList()
        {
            List<HairType> result = _productService.GetHairTypesList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetHairChallengesList")]
        public IActionResult GetHairChallengesList()
        {
            List<HairChallenges> result = _productService.GetHairChallengesList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetProductIndicatorsList")]
        public IActionResult GetProductIndicatorsList()
        {
            List<ProductIndicator> result = _productService.GetProductIndicatorsList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetProductTagList")]
        public IActionResult GetProductTagList()
        {
            List<ProductTags> result = _productService.GetProductTagList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetProductClassificationList")]
        public IActionResult GetProductClassificationList()
        {
            List<ProductClassification> result = _productService.GetProductClassificationList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetProductCategory")]
        public IActionResult GetProductCategory()
        {
            List<ProductTypeCategory> result = _productService.GetProductCategory();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
    }
}
