using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;
        private readonly IBaseBusiness _baseBusiness;
        private readonly IHostingEnvironment _environment;

        public IngredientsController(IIngredientsService ingredientsService, IBaseBusiness baseBusiness, IHostingEnvironment environment)
        {
            _ingredientsService = ingredientsService;
            _baseBusiness = baseBusiness;
            _environment = environment;
        }

        [HttpGet("GetIngredients")]
        public IActionResult GetIngredients()
        {
            IngredientsModel result = _ingredientsService.GetIngredients();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveIngredients")]
        public JObject SaveIngredients([FromForm]IngredientEntityModel ingredientEntityModel)
        {
            if (Request.HasFormContentType)
            {
                if (ingredientEntityModel.File != null)
                {
                    string fileName = ingredientEntityModel.File.FileName;
                    const string UPLOAD_FOLDER = "Ingredients";
                    if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
                    {
                        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
                    }
                    var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        ingredientEntityModel.File.CopyToAsync(stream);
                    }
                    ingredientEntityModel.Image = ingredientEntityModel.File.FileName;
                }
            }

            IngredientEntityModel result = _ingredientsService.SaveIngredients(ingredientEntityModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetIngredientById")]
        public JObject GetIngredientsId(IngedientsEntity ingedientsEntity)
        {
            IngedientsEntity result = _ingredientsService.GetIngredientById(ingedientsEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteIngredients")]
        public JObject DeleteIngredients(IngedientsEntity ingedientsEntity)
        {
            bool result = _ingredientsService.DeleteIngredients(ingedientsEntity);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", ingedientsEntity);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
    }
}