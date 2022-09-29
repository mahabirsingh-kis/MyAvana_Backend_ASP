using HubSpot.NET.Api.Contact.Dto;
using HubSpot.NET.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAvana.Auth.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvanaApi.Contract;
using MyAvanaApi.IServices;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyAvana.Auth.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly Logger.Contract.ILogger _logger;
        private readonly AvanaContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly ICryptoService _cryptoService;
        private readonly IOptions<Audience> _settings;
        private readonly HubSpotApi _hubSpotApi;
        public AccountService(IConfiguration configuration,
                                Logger.Contract.ILogger logger,
                                AvanaContext context,
                                UserManager<UserEntity> userManager,
                                SignInManager<UserEntity> signInManager,
                                ICryptoService cryptoService,
                                IHttpClientFactory httpClientFactory,
                                IEmailService emailService,
                                IOptions<Audience> settings)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpClientFactory = httpClientFactory;
            _settings = settings;
            _cryptoService = cryptoService;
            _configuration = configuration;
            _hubSpotApi = new HubSpotApi(_configuration.GetSection("Hubspot:Key").Value);
        }

        public (JsonResult result, bool success, string error) SignIn(Authentication authentication)
        {
            try
            {
                var result = _signInManager.PasswordSignInAsync(authentication.UserName, authentication.Password, false, false).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var user = _userManager.FindByEmailAsync(authentication.UserName).GetAwaiter().GetResult();
                    if (user.Active)
                    {
                        var Token = GenerateToken(user);
                        return (Token, result.Succeeded, "");
                    }
                    return (new JsonResult(""), false, "Please activate your account.");
                }
                return (new JsonResult(""), false, "Invalid Credentials");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (null, false, "Something went wrong. Please try again later.");
            }
        }
        public (bool success, string error) SignUp(Signup signup)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    UserEntity entity;
                    Claim[] claims;

                    IdentityResult result = CreateUser(signup, out entity, out claims);
                    if (!result.Succeeded)
                    {
                        var firstError = result.Errors.FirstOrDefault()?.Description;
                        return (result.Succeeded, firstError);
                    }
                    else
                    {
                        _userManager.AddToRoleAsync(entity, "User").Wait();
                        _userManager.AddClaimsAsync(entity, claims).Wait();
                        _userManager.UpdateAsync(entity).Wait();
                        _context.SaveChanges();
                    }


                    var emailRes = SendEmail(entity, Operation.CodeVerify, "REG");


                    if (!emailRes.success)
                        _logger.LogError(emailRes.error);
                    dbContextTransaction.Commit();
                    return (result.Succeeded, "");

                }
                catch (Exception Ex)
                {
                    _logger.LogError(Ex.Message, Ex);
                    return (false, "Something went wrong. Please try again later.");
                }
            }
        }
        public (JsonResult result, bool success, string error) ForgetPass(string email)
        {
            try
            {
                var result = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
                if (result != null)
                {
                    var emailRes = SendEmail(result, Operation.ForgetPassword, "FGTPASS");
                    if (!emailRes.success)
                        _logger.LogError(emailRes.error);
                    return (new JsonResult("We have sent you a secure code on your email. Please use that to reset your password.")
                    { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                else
                    return (new JsonResult(""), false, "Email id not exist in application!!!");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (new JsonResult(""), false, "Some server error occured. Please try again!!");
            }
        }
        public (JsonResult result, bool success, string error) ActivateUser(CodeVerify codeVerify)
        {
            try
            {
                var codeResponse = GetCodeEntity(codeVerify, Operation.CodeVerify);
                if (codeResponse.CodeEntity != null)
                {
                    if (codeResponse.CodeEntity.Code.ToLower() == codeVerify.Code.ToLower())
                    {
                        if (string.IsNullOrEmpty(codeResponse.entity.HubSpotContactId))
                        {
                            var res = CreateContact(codeResponse.entity);
                        }
                        codeResponse.CodeEntity.IsActive = false;
                        codeResponse.entity.Active = true;
                        codeResponse.entity.EmailConfirmed = true;
                        _context.SaveChanges();
                        return (GenerateToken(codeResponse.entity), true, "");
                    }
                    return (new JsonResult(""), false, "Invalid Code.");
                }
                return (new JsonResult(""), false, codeResponse.error);
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again later.");
            }
        }

        private bool CreateContact(UserEntity entity)
        {
            try
            {

                var contact = _hubSpotApi.Contact.CreateOrUpdate(new ContactHubSpotModel()
                {
                    Email = entity.Email,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Phone = entity.PhoneNumber
                });
                entity.HubSpotContactId = contact.Id.ToString();
                return true;
            }
            catch (HubSpot.NET.Core.HubSpotException Ex)
            {
                if (!string.IsNullOrEmpty(Ex.RawJsonResponse))
                {
                    var response = JsonConvert.DeserializeObject<MyAvanaApi.Models.ViewModels.HubSpotException>(Ex.RawJsonResponse);
                    if (response.message == "Contact already exists")
                    {
                        entity.HubSpotContactId = response.identityProfile.vid.ToString();
                    }
                }
                _logger.LogError(Ex.Message, Ex);
                return false;
            }
        }


        private IdentityResult CreateUser(Signup signUp, out UserEntity entity, out Claim[] claims)
        {
            string accountNo = signUp.FirstName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            entity = new UserEntity
            {
                AccountNo = accountNo,
                Email = signUp.Email,
                UserName = signUp.Email,
                Active = false,
                FirstName = signUp.FirstName,
                LastName = signUp.LastName,
                EmailConfirmed = false,
                PhoneNumber = signUp.PhoneNo,
                CreatedAt = DateTimeOffset.UtcNow,
                CountryCode = signUp.CountryCode
            };
            claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, entity.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, entity.UserName)
            };
            return _userManager.CreateAsync(entity, signUp.Password).GetAwaiter().GetResult();

        }
        private (bool success, string error) SendEmail(UserEntity entity, Operation operation, string template)
        {
            string activationCode = _cryptoService.GetRandomKey(4);
            _context.CodeEntities.Where(s => s.AccountId == entity.AccountNo && s.OpCode == operation).ToList().ForEach(s => s.IsActive = false);
            _context.SaveChanges();
            _context.CodeEntities.Add(new CodeEntity() { Id = Guid.NewGuid(), AccountId = entity.AccountNo, Code = activationCode, OpCode = operation, IsActive = true, CreatedDate = DateTime.UtcNow });
            _context.SaveChanges();

            EmailInformation emailInformation = new EmailInformation
            {
                Code = activationCode,
                Email = entity.Email,
                Name = entity.FirstName + " " + entity.LastName,

            };

            var emailRes = _emailService.SendEmail(template, emailInformation);
            return emailRes;

        }
        private JsonResult GenerateToken(UserEntity user)
        {
            var now = DateTime.UtcNow;


            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.Secret));

            var jwt = new JwtSecurityToken(
                    issuer: _settings.Value.Iss,
                    audience: _settings.Value.Aud,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromDays(14)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);



            var response = new JsonResult(new Dictionary<string, object>
            {
                { "access_token" , encodedJwt },
                { "expires_in" , (int) TimeSpan.FromMinutes(200).TotalSeconds },
                {"user_name",user.UserName },
                {"Email",user.Email },
                {"Name",user.FirstName +" "+ user.LastName },
                {"AccountNo",user.AccountNo },
                {"TwoFactor",user.TwoFactorEnabled },
                {"hairType",user.HairType },
                {"imageURL",user.ImageURL },
                {"Id", user.Id }
            });

            return response;

        }
        private (CodeEntity CodeEntity, string error, UserEntity entity) GetCodeEntity(CodeVerify codeVerify, Operation operation)
        {
            var user = _context.Users.Where(s => s.Email.ToLower() == codeVerify.Email.ToLower()).FirstOrDefault();
            if (user != null)
            {
                var result = _context.CodeEntities.Where(s => s.AccountId == user.AccountNo && s.IsActive && s.OpCode == operation && s.Code == codeVerify.Code.Trim()).FirstOrDefault();
                if (result != null)
                    return (result, "", user);
            }
            return (null, "Invalid email address.", null);
        }
        public async Task<(bool success, string error)> SetPass(SetPassword setPassword)
        {
            try
            {
                var codeResponse = GetCodeEntity(new CodeVerify() { Email = setPassword.Email, Code = setPassword.Code }, Operation.ForgetPassword);
                if (codeResponse.CodeEntity != null)
                {
                    var user = await _userManager.FindByEmailAsync(setPassword.Email);
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, setPassword.Password);
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return (true, "");
                    return (false, "Error in updating the password.");
                }
                return (false, codeResponse.error);
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
        public UserEntity GetAccountNo(ClaimsPrincipal user)
        {
            Claim claim = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            UserEntity usr = _userManager.GetUsersForClaimAsync(claim).GetAwaiter().GetResult().FirstOrDefault();
            if (usr == null)
            {
                var email = user.Identities.FirstOrDefault().Name;
                usr = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            }
            return usr;
        }

        public (JsonResult result, bool success, string error) ResendCode(string email)
        {
            try
            {
                var user = _context.Users.Where(s => s.Email.ToLower() == email.ToLower()).FirstOrDefault();

                if (user != null)
                {
                    if (!user.Active)
                    {
                        var mailStatus = SendEmail(user, Operation.CodeVerify, "REG");
                        if (mailStatus.success)
                            return (new JsonResult("Activation code has been sent to your email address.") { StatusCode = (int)HttpStatusCode.OK }, true, "");
                        return (new JsonResult(""), false, "Error in sending the emails.");
                    }
                    return (new JsonResult(""), false, "User is already activated. Please use forget password link, If you forget your password.");
                }
                return (new JsonResult(""), false, "Invalid user.");

            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message);
                return (new JsonResult(""), false, "Something went wrong. Please try again later.");
            }
        }




        public (UserEntity user, bool success, string error) GetAccountNo(string email)
        {
            var resValue = _context.Users.Where(s => s.Email.ToLower() == email.ToLower()).FirstOrDefault();
            return (resValue, true, "");
        }

        public (string userId, string error) WebSignup(Signup signup)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    UserEntity entity;
                    Claim[] claims;

                    IdentityResult result = CreateWebUser(signup, out entity, out claims);
                    if (!result.Succeeded)
                    {
                        var firstError = result.Errors.FirstOrDefault()?.Description;
                        return (null, firstError);
                    }
                    else
                    {
                        _userManager.AddToRoleAsync(entity, "User").Wait();
                        _userManager.AddClaimsAsync(entity, claims).Wait();
                        _userManager.UpdateAsync(entity).Wait();
                        _context.SaveChanges();
                    }


                    var emailRes = SendEmail(entity, Operation.CodeVerify, "REG");
                    if (!emailRes.success)
                        _logger.LogError(emailRes.error);
                    dbContextTransaction.Commit();
                    var user = _userManager.FindByEmailAsync(signup.Email);
                    return (user.Result.Id.ToString(), "");

                }
                catch (Exception Ex)
                {
                    _logger.LogError(Ex.Message, Ex);
                    return (null, "Something went wrong. Please try again later.");
                }
            }
        }

        private IdentityResult CreateWebUser(Signup signUp, out UserEntity entity, out Claim[] claims)
        {
            string accountNo = signUp.FirstName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            entity = new UserEntity
            {
                AccountNo = accountNo,
                Email = signUp.Email,
                UserName = signUp.Email,
                Active = true,
                FirstName = signUp.FirstName,
                LastName = signUp.LastName,
                EmailConfirmed = false,
                PhoneNumber = signUp.PhoneNo,
                CreatedAt = DateTimeOffset.UtcNow,
                CountryCode = signUp.CountryCode,
                CustomerType = signUp.CustomerType == null ? true : false
            };
            claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, entity.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, entity.UserName)
            };
            var response = _userManager.CreateAsync(entity, signUp.Password).GetAwaiter().GetResult();
            return response;
        }

        public bool updateDeviceId(UserEntity userEntity, string deviceId)
        {
            try
            {
                var user = _context.Users.Where(s => s.Email.ToLower() == userEntity.UserName.ToLower()).FirstOrDefault();
                user.DeviceId = deviceId;
                    _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            { return false; }
        }
        public bool saveAIResult(UserEntity userEntity, string aiResult)
        {
            try
            {
                var user = _context.Users.Where(s => s.Email.ToLower() == userEntity.UserName.ToLower()).FirstOrDefault();
                user.AIResult = aiResult;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
    }
}
