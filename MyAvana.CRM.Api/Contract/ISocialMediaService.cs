using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface ISocialMediaService
    {
        (JsonResult result, bool success, string error) GetTvLinks(string settingName, string subSettingName); 
         (JsonResult result, bool success, string error) GetTvLinks2(string settingName, string subSettingName); 
        (JsonResult result, bool success, string error) GetTvLinksCategories(string settingName, string subSettingName);
        MediaLinkEntity GetMediaById(MediaLinkEntity mediaLinkEntity);
        MediaLinkEntity SaveMediaLink(MediaLinkEntity mediaLinkEntity);
        bool DeleteMediaLink(MediaLinkEntity mediaLink);
        (JsonResult result, bool success, string error) GetVideoCategories();
    }
}
