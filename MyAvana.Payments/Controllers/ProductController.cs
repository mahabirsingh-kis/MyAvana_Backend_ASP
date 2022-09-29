using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Payments.Api.Contract;
using MyAvanaApi.Models.Entities;

namespace MyAvana.Payments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("HairTypes")]
        public IActionResult GetAllHairTypes()
        {
            var result = _productService.GetAllHairTypes();
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

        [HttpGet("ProductsTypes")]
        public IActionResult GetAllProductTypes(string hairType)
        {
            var result = _productService.GetAllProductTypes(hairType);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

        [HttpGet("GetProductsByTypes")]
        public IActionResult GetProductsByTypes(string hairTypes)
        {
            var result = _productService.GetProductsByTypes(hairTypes);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

        [HttpGet("suggestions")]
        public IActionResult GetProductSuggetion(string hairType, string productType, string hairChallenge)
        {
            var result = _productService.GetSuggestions(hairType, productType, hairChallenge);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;
        }

        [HttpGet("GetProductDetails")]
        public IActionResult GetProductDetails(string id)
        {
            var result = _productService.GetProductDetails(id);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest }); ;

        }
    }
}