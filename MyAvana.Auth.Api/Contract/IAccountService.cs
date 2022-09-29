using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.Auth.Api.Contract
{
    public interface IAccountService
    {
        (bool success, string error) SignUp(Signup signup);
        (string userId, string error) WebSignup(Signup signup);
        (JsonResult result, bool success, string error) SignIn(Authentication authentication);
        (JsonResult result, bool success, string error) ForgetPass(string email);
        (JsonResult result, bool success, string error) ActivateUser(CodeVerify codeVerify);
        Task<(bool success, string error)> SetPass(SetPassword setPassword);
        UserEntity GetAccountNo(ClaimsPrincipal user);
        (JsonResult result, bool success, string error) ResendCode(string email);
        (UserEntity user, bool success, string error) GetAccountNo(string email);
        bool updateDeviceId(UserEntity userEntity, string deviceId);
        bool saveAIResult(UserEntity userEntity, string aiResult);
    }
}
