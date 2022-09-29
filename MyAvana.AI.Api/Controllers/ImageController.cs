using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Extensions;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;

namespace MyAvana.AI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private const string UPLOAD_FOLDER = "uploads";
        private readonly HttpClient _httpClient;
        private readonly AvanaContext _context;
        private readonly UserManager<UserEntity> _userManager;
        public ImageController(AvanaContext avanaContext, UserManager<UserEntity> userManager)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            _context = avanaContext;
            _userManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("classifyimage")]
        public async Task<IActionResult> ClassifyImage(IFormFile imageFile)
        {
            if (imageFile.Length == 0)
                return BadRequest();

            MemoryStream imageMemoryStream = new MemoryStream();
            await imageFile.CopyToAsync(imageMemoryStream);

            //Check that the image is valid
            byte[] imageData = imageMemoryStream.ToArray();
            if (!imageData.IsValidImage())
                return StatusCode(StatusCodes.Status415UnsupportedMediaType);

            //Convert to Image
            Image image = Image.FromStream(imageMemoryStream);

            //Convert to Bitmap
            Bitmap bitmapImage = (Bitmap)image;

            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
            }

            string name = Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ".jpg");
            SaveJpeg(name, bitmapImage);
            var result = TensorFlowModelConfigurator.PredictImage(name);

            return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("ImageClassify")]
        public IActionResult ImageClassify([FromBody] Imagerequest imagerequest)
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;

                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;
                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);

                if (imagerequest.fileData.Length == 0)
                    return BadRequest();
                var bytes = Convert.FromBase64String(imagerequest.fileData);

                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
                }

                string name = Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ".jpg");
                if (bytes.Length > 0)
                {
                    using (var stream = new FileStream(name, FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                }
                var result = TensorFlowModelConfigurator.PredictImage(name);
                if (entity != null)
                {
                    UserEntity us = _context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                    us.HairType = result.ToTuple().Item1;
                    us.AIResult = JsonConvert.SerializeObject(result).ToString();
                    _context.SaveChanges();
                }
                return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });
            }
            catch (Exception Ex)
            {
                return (new JsonResult("Erro in image classification.") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
        private static void SaveJpeg(string path, Bitmap image)
        {
            SaveJpeg(path, image, 95L);
        }
        private static void SaveJpeg(string path, Bitmap image, long quality)
        {
            using (EncoderParameters encoderParameters = new EncoderParameters(1))
            using (EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, quality))
            {
                ImageCodecInfo codecInfo = ImageCodecInfo.GetImageDecoders().First(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                encoderParameters.Param[0] = encoderParameter;
                image.Save(path, codecInfo, encoderParameters);
            }
        }
    }
}