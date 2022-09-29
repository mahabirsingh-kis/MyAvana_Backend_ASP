using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
		private readonly IBaseBusiness _baseBusiness;
		private readonly IHostingEnvironment _environment;
		public ArticleController(IArticleService articleService, IBaseBusiness baseBusiness, IHostingEnvironment hostingEnvironment)
        {
			_baseBusiness = baseBusiness;
			_articleService = articleService;
			_environment = hostingEnvironment;
		}
        [HttpGet("GetArticles")]
        public IActionResult GetArticles()
        {
			var file = _environment.ContentRootPath + "/StaticFiles/articleJSONArr.json";
			if (System.IO.File.Exists(file))
			{
				var data = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(file));
				return Ok(new JsonResult(data) { StatusCode = (int)HttpStatusCode.OK });
			}
			return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
			
		}

		[HttpGet("GetBlogArticles")]
		public IActionResult GetBlogArticles()
		{
			BlogArticleModel result = _articleService.GetArticles();
			if (result != null)
				return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

			return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });

		}

		[HttpPost("UploadArticles")]
		public JObject UploadArticles(BlogArticle blogArticle)
		{
			BlogArticle result = _articleService.UploadArticles(blogArticle);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

		}

		[HttpPost("GetArticleById")]
		public JObject GetArticleById(BlogArticle blogArticle)
		{
			BlogArticle result = _articleService.GetArticleById(blogArticle);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

		}

        [HttpPost]
        [Route("DeleteArticle")]
        public JObject DeleteArticle(BlogArticle blogArticle)
        {
            bool result = _articleService.DeleteArticle(blogArticle);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", blogArticle);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
    }
}