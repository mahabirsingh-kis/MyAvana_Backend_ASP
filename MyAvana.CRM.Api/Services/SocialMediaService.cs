using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly ILogger _logger;
        private readonly AvanaContext _context;
        public SocialMediaService(ILogger logger, AvanaContext avanaContext)
        {
            _logger = logger;
            _context = avanaContext;
        }
        public (JsonResult result, bool success, string error) GetTvLinks(string settingName, string subSettingName)
        {
            try
            {
                var result = _context.MediaLinkEntities.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
                if (result != null)
                    return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                return (new JsonResult(""), false, "No settings found.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in GetTvLinks API.", Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }

        public (JsonResult result, bool success, string error) GetVideoCategories()
        {
            try
            {
                var result = _context.VideoCategories.Where(x => x.IsActive == true).ToList();
                if (result != null)
                    return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                return (new JsonResult(""), false, "No settings found.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in GetTvLinks API.", Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }
        public (JsonResult result, bool success, string error) GetTvLinks2(string settingName, string subSettingName)
        {
            try
            {
                var result = _context.MediaLinkEntities.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
                if (result != null)
                {
                    //foreach (var res in result)
                    //{
                    //    if (res.VideoId.Contains("instagram"))
                    //    {
                    //        res.ImageLink = "http://admin.myavana.com/images/instagram.jpg";
                    //    }
                    //}
                    return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                return (new JsonResult(""), false, "No settings found.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in GetTvLinks API.", Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }

        public (JsonResult result, bool success, string error) GetTvLinksCategories(string settingName, string subSettingName)
        {
            try
            {
                //var result = _context.MediaLinkEntities.Include(x => x.VideoCategory).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
                var result = (from ml in _context.MediaLinkEntities
                              join cat in _context.VideoCategories
                              on ml.VideoCategoryId equals cat.Id
                              select new {
                                  CategoryId = cat.Id,
                                  Category = cat.Description,
                                  CreatedOn = ml.CreatedOn,
                                  UserId = ml.Id,
                                  IsActive = ml.IsActive,
                                  VideoId = ml.VideoId,
                                  Title = ml.Title,
                                  ImageLink = ml.ImageLink,
                                  Description = ml.Description,
                                  Header = ml.Header,
                                  IsFeatured = ml.IsFeatured
                              }).ToList().OrderByDescending(ml => ml.CreatedOn);
                List <MediaLinkCategory> mediaLinkCategories = new List<MediaLinkCategory>();
                List<string> categories = new List<string>();
                if (result != null)
                {
                    foreach (var res in result)
                    {
                        if (!categories.Contains(res.Category))
                        {
                            categories.Add(res.Category);
                            MediaLinkCategory mediaLinkCategory = new MediaLinkCategory();
                            mediaLinkCategory.Category = res.Category;
                            var mediaLink = result.Where(x => x.CategoryId == res.CategoryId).ToList();
                            List<MediaLinkEntity> mediaEntities = new List<MediaLinkEntity>();
                            foreach (var media in mediaLink)
                            {
                                MediaLinkEntity mediaEntity = new MediaLinkEntity();
                                mediaEntity.CreatedOn = media.CreatedOn;
                                mediaEntity.IsActive = media.IsActive;
                                mediaEntity.VideoId = media.VideoId;
                                mediaEntity.Title = media.Title;
                                mediaEntity.ImageLink = media.ImageLink;
                                mediaEntity.Description = media.Description;
                                mediaEntity.Header = media.Header;
                                mediaEntity.IsFeatured = media.IsFeatured;
                                mediaEntities.Add(mediaEntity);
                                if (media.VideoId.Contains("instagram"))
                                {
                                    mediaEntity.ImageLink = "http://admin.myavana.com/images/instagram.jpg";
                                }
                            }
                            mediaLinkCategory.MediaEntity = mediaEntities;
                            mediaLinkCategories.Add(mediaLinkCategory);

                        }
                    }
                    return (new JsonResult(mediaLinkCategories) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                return (new JsonResult(""), false, "No settings found.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in GetTvLinks API.", Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }
        public MediaLinkEntity GetMediaById(MediaLinkEntity mediaLinkEntity)
        {
            try
            {
                MediaLinkEntity mediaLinkEntityModel = _context.MediaLinkEntities.Where(x => x.Id == mediaLinkEntity.Id).FirstOrDefault();
                return mediaLinkEntityModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public MediaLinkEntity SaveMediaLink(MediaLinkEntity mediaLinkEntity)
        {
            try
            {
                if (mediaLinkEntity.Id != Guid.Empty)
                {
                    var data = _context.MediaLinkEntities.Where(x => x.Id == mediaLinkEntity.Id).FirstOrDefault();
                    data.VideoId = mediaLinkEntity.VideoId;
                    data.Title = mediaLinkEntity.Title;
                    data.Description = mediaLinkEntity.Description;
                    data.Header = mediaLinkEntity.Header;
                    data.IsFeatured = mediaLinkEntity.IsFeatured;
                    data.ImageLink = mediaLinkEntity.ImageLink;
                }
                else
                {
                    _context.MediaLinkEntities.Add(new MediaLinkEntity()
                    {
                        Id = Guid.NewGuid(),
                        VideoId = mediaLinkEntity.VideoId,
                        Title = mediaLinkEntity.Title,
                        Description = mediaLinkEntity.Description,
                        Header = mediaLinkEntity.Header,
                        IsFeatured = mediaLinkEntity.IsFeatured,
                        ImageLink = mediaLinkEntity.ImageLink,
                        VideoCategoryId = mediaLinkEntity.VideoCategoryId,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow
                    });
                }
                _context.SaveChanges();
                return mediaLinkEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteMediaLink(MediaLinkEntity mediaLink)
        {
            try
            {
                var objCode = _context.MediaLinkEntities.FirstOrDefault(x => x.Id == mediaLink.Id);
                {
                    if (objCode != null)
                    {
                        objCode.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }
    }
}
