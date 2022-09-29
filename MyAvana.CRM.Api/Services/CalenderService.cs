using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class CalenderService : ICalenderService
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly AvanaContext _context;
        private readonly Dictionary<int, string> priority;
        private readonly HttpClient _client;
        public CalenderService(AvanaContext context, ITokenService tokenService, IConfiguration configuration, IHttpClientFactory httpClient)
        {
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
            priority = new Dictionary<int, string>();
            _client = httpClient.CreateClient();
        }
        public bool SaveUserDailyRoutine(DailyRoutineTracker dailyRoutineTracker, ClaimsPrincipal user)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(user);
                if (usr != null)
                {
                    var routine = _context.DailyRoutineTracker.Where(x => x.UserId == usr.Email && x.TrackTime == dailyRoutineTracker.TrackTime.Date).FirstOrDefault();
                    if (routine != null)
                    {
                        routine.TrackTime = dailyRoutineTracker.TrackTime.Date;
                        routine.Description = dailyRoutineTracker.Description;
                        routine.HairStyle = dailyRoutineTracker.HairStyle;
                        routine.IsCompleted = false;
                        routine.Notes = dailyRoutineTracker.Notes;
                    }
                    else
                    {
                        dailyRoutineTracker.UserId = usr.Email;
                        dailyRoutineTracker.IsCompleted = false;
                        dailyRoutineTracker.TrackTime = dailyRoutineTracker.TrackTime.Date;
                        dailyRoutineTracker.CreatedOn = DateTime.Now;
                        dailyRoutineTracker.IsActive = true;
                        //dailyRoutineTracker.Notes = dailyRoutineTracker.Notes;
                        _context.Add(dailyRoutineTracker);
                    }
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SaveHairStyle(DailyRoutineTracker trackingDetails, ClaimsPrincipal user)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(user);

                if (usr != null)
                {
                    DailyRoutineTracker hairstyle = _context.DailyRoutineTracker.Where(x => x.UserId == usr.Email).FirstOrDefault();
                    if (hairstyle != null)
                    {
                        hairstyle.HairStyle = trackingDetails.HairStyle;
                    }
                    else
                    {
                        trackingDetails.UserId = usr.Email;
                        trackingDetails.CreatedOn = DateTime.Now.Date;
                        trackingDetails.IsCompleted = false;
                        trackingDetails.IsActive = true;
                        _context.Add(trackingDetails);
                    }
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DailyRoutineContent GetDailyRoutine(DailyRoutineTracker dailyRoutineTracker, ClaimsPrincipal user)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(user);

                if (usr != null)
                {
                    DailyRoutineContent dailyRoutineContent = new DailyRoutineContent();
                    dailyRoutineContent.dailyRoutineTracker = _context.DailyRoutineTracker.Where(x => x.UserId == usr.Email && x.TrackTime == dailyRoutineTracker.TrackTime.Date).FirstOrDefault();
                    dailyRoutineContent.trackTimes = _context.DailyRoutineTracker.Where(x => x.UserId == usr.Email).Select(y => y.TrackTime).ToList();
                    dailyRoutineContent.blogArticleModel = _context.BlogArticles.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
                    return dailyRoutineContent;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public HairCareParts DifferentHairCareParts(string selectedDate, ClaimsPrincipal user)
        {
            try
            {
                DateTime currentDate = (Convert.ToDateTime(selectedDate)).Date;
                var usr = _tokenService.GetAccountNo(user);
                DailyRoutineTracker dailyRoutineTracker = _context.DailyRoutineTracker.Where(x => x.TrackTime == currentDate && x.UserId.ToLower() == usr.Email.ToLower()).FirstOrDefault();
                HairCareParts hairCareParts = new HairCareParts();
                hairCareParts.routineProducts = _context.ProductEntities.Where(x => x.IsActive == true).Select(x => new RoutineProducts
                {
                    Id = x.Id,
                    Name = x.ActualName,
                    Image = x.ImageName,
                    BrandName = x.BrandName,
                    Description = x.ProductDetails

                }).ToList();

                hairCareParts.routineIngredients = _context.IngedientsEntities.Where(x => x.IsActive == true).Select(x => new RoutineIngredients
                {
                    Id = x.IngedientsEntityId,
                    Name = x.Name,
                    Description = x.Description,
                    Image = "http://admin.myavana.com/Ingredients/" + x.Image
                }).ToList();

                hairCareParts.routineRegimens = _context.Regimens.Include(x => x.RegimenSteps).Where(x => x.IsActive == true).Select(x => new RoutineRegimens
                {
                    Id = x.RegimensId,
                    Name = x.Name,
                    Description = x.Description,
                    Image = "http://admin.myavana.com/Regimens/" + x.RegimenSteps.Step1Photo
                }).ToList();

                hairCareParts.hairStyles = _context.HairStyles.Where(x => x.IsActive == true).ToList();
                if (dailyRoutineTracker != null)
                {
                    hairCareParts.selectedProducts = _context.DailyRoutineProducts.Where(x => x.IsActive == true && x.IsSelected == false && x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => new RoutineProducts
                    {
                        Id = x.ProductId,
                        Name = x.Name,
                        Image = x.Image,

                    }).ToList();
                    hairCareParts.selectedNewProducts = _context.DailyRoutineProducts.Where(x => x.IsActive == true && x.ProductId.ToString().Length == 7 && x.IsSelected == true && x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => new RoutineProducts
                    {
                        Id = x.ProductId,
                        Name = x.Name,
                        Image = x.Image
                    }).ToList();
                    hairCareParts.selectedIngredients = _context.DailyRoutineIngredients.Where(x => x.IsActive == true && x.IsSelected == false && x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => new RoutineIngredients
                    {
                        Id = x.IngredientId,
                        Name = x.Name,
                        Image = x.Image
                    }).ToList();
                    hairCareParts.selectedNewIngredients = _context.DailyRoutineIngredients.Where(x => x.IsActive == true && x.IngredientId.ToString().Length == 7 && x.IsSelected == true && x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => new RoutineIngredients
                    {
                        Id = x.IngredientId,
                        Name = x.Name,
                        Image = x.Image
                    }).ToList();
                    hairCareParts.selectedRegimens = _context.DailyRoutineRegimens.Where(x => x.IsActive == true && x.IsSelected == false && x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => new RoutineRegimens
                    {
                        Id = x.RegimenId,
                        Name = x.Name,
                        Image = x.Image
                    }).ToList();
                    hairCareParts.selectedNewRegimens = _context.DailyRoutineRegimens.Where(x => x.IsActive == true && x.RegimenId.ToString().Length == 7 && x.IsSelected == true && x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => new RoutineRegimens
                    {
                        Id = x.RegimenId,
                        Name = x.Name,
                        Image = x.Image
                    }).ToList();
                    hairCareParts.selectedHairStyles = _context.DailyRoutineHairStyles.Where(x => x.IsActive == true && x.IsSelected == false && x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => new RoutineHairStyles
                    {
                        Id = x.HairStyleId,
                        Name = x.Name,
                        Image = x.Image
                    }).ToList();
                    hairCareParts.selectedNewHairStyles = _context.DailyRoutineHairStyles.Where(x => x.IsActive == true && x.HairStyleId.ToString().Length == 7 && x.IsSelected == true && x.RoutineTrackerId == dailyRoutineTracker.Id).Select(x => new RoutineHairStyles
                    {
                        Id = x.HairStyleId,
                        Name = x.Name,
                        Image = x.Image
                    }).ToList();
                }
                return hairCareParts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public bool SaveUserRoutineHairCare(IEnumerable<UserRoutineHairCare> userRoutineHairCareList, ClaimsPrincipal user)
        {
            try
            {

                List<DailyRoutineIngredients> dailyUserRoutineList = new List<DailyRoutineIngredients>();
                List<DailyRoutineProducts> dailyUserProductRoutineList = new List<DailyRoutineProducts>();
                List<DailyRoutineRegimens> dailyUserRegimenRoutineList = new List<DailyRoutineRegimens>();
                List<DailyRoutineHairStyles> dailyUserHairStyleRoutineList = new List<DailyRoutineHairStyles>();

                int trackerId = userRoutineHairCareList.Select(x => x.RoutineTrackerId).FirstOrDefault();
                if (trackerId == 0)
                {
                    var usr = _tokenService.GetAccountNo(user);
                    DailyRoutineTracker dailyRoutineTracker = new DailyRoutineTracker();
                    if (usr != null)
                        dailyRoutineTracker = _context.DailyRoutineTracker.Where(x => x.UserId == usr.UserName && x.TrackTime.Date == userRoutineHairCareList.Select(y => y.TrackDate.Date).FirstOrDefault()).FirstOrDefault();
                    if (dailyRoutineTracker != null)
                        trackerId = dailyRoutineTracker.Id;
                    else
                    {
                        DailyRoutineTracker routineTracker = new DailyRoutineTracker();
                        routineTracker.UserId = usr.UserName;
                        routineTracker.TrackTime = userRoutineHairCareList.Select(y => y.TrackDate.Date).FirstOrDefault();
                        routineTracker.CreatedOn = DateTime.Now;
                        routineTracker.IsCompleted = false;
                        routineTracker.IsActive = true;
                        _context.Add(routineTracker);
                        _context.SaveChanges();
                        trackerId = routineTracker.Id;
                    }
                }
                foreach (UserRoutineHairCare userRoutineHairCare in userRoutineHairCareList)
                {
                    if (userRoutineHairCare.IsIngredient)
                    {
                        DailyRoutineIngredients routineIngredients = _context.DailyRoutineIngredients.Where(x => x.RoutineTrackerId == trackerId && x.IngredientId == userRoutineHairCare.Id).FirstOrDefault();
                        if (routineIngredients != null && routineIngredients.IngredientId.ToString().Length == 7)
                        {
                            routineIngredients.IsSelected = true;
                        }
                        else
                        {
                            DailyRoutineIngredients dailyUserRoutine = new DailyRoutineIngredients();
                            dailyUserRoutine.IngredientId = userRoutineHairCare.Id;
                            dailyUserRoutine.Name = userRoutineHairCare.Name;
                            dailyUserRoutine.Image = userRoutineHairCare.Image;
                            dailyUserRoutine.RoutineTrackerId = trackerId;
                            dailyUserRoutine.CreatedOn = DateTime.Now;
                            dailyUserRoutine.IsActive = true;
                            dailyUserRoutineList.Add(dailyUserRoutine);
                        }
                    }
                    else if (userRoutineHairCare.IsProduct)
                    {
                        DailyRoutineProducts routineProducts = _context.DailyRoutineProducts.Where(x => x.RoutineTrackerId == trackerId && x.ProductId == userRoutineHairCare.Id).FirstOrDefault();
                        if (routineProducts != null && routineProducts.ProductId.ToString().Length == 7)
                        {
                            routineProducts.IsSelected = true;
                        }
                        else
                        {
                            DailyRoutineProducts dailyUserRoutine = new DailyRoutineProducts();
                            dailyUserRoutine.ProductId = userRoutineHairCare.Id;
                            dailyUserRoutine.Name = userRoutineHairCare.Name;
                            dailyUserRoutine.Image = userRoutineHairCare.Image;
                            dailyUserRoutine.RoutineTrackerId = trackerId;
                            dailyUserRoutine.CreatedOn = DateTime.Now;
                            dailyUserRoutine.IsActive = true;
                            dailyUserProductRoutineList.Add(dailyUserRoutine);
                        }
                    }
                    else if (userRoutineHairCare.IsRegimen)
                    {
                        DailyRoutineRegimens routineRegimens = _context.DailyRoutineRegimens.Where(x => x.RoutineTrackerId == trackerId && x.RegimenId == userRoutineHairCare.Id).FirstOrDefault();
                        if (routineRegimens != null && routineRegimens.RegimenId.ToString().Length == 7)
                        {
                            routineRegimens.IsSelected = true;
                        }
                        else
                        {
                            DailyRoutineRegimens dailyUserRoutine = new DailyRoutineRegimens();
                            dailyUserRoutine.RegimenId = userRoutineHairCare.Id;
                            dailyUserRoutine.Name = userRoutineHairCare.Name;
                            dailyUserRoutine.Image = userRoutineHairCare.Image;
                            dailyUserRoutine.RoutineTrackerId = trackerId;
                            dailyUserRoutine.CreatedOn = DateTime.Now;
                            dailyUserRoutine.IsActive = true;
                            dailyUserRegimenRoutineList.Add(dailyUserRoutine);
                        }
                    }
                    else if (userRoutineHairCare.IsHairStyle)
                    {
                        DailyRoutineHairStyles routineHairStyles = _context.DailyRoutineHairStyles.Where(x => x.RoutineTrackerId == trackerId && x.HairStyleId == userRoutineHairCare.Id).FirstOrDefault();
                        if (routineHairStyles != null && routineHairStyles.HairStyleId.ToString().Length == 7)
                        {
                            routineHairStyles.IsSelected = true;
                        }
                        else
                        {
                            DailyRoutineHairStyles dailyUserRoutine = new DailyRoutineHairStyles();
                            dailyUserRoutine.HairStyleId = userRoutineHairCare.Id;
                            dailyUserRoutine.Name = userRoutineHairCare.Name;
                            dailyUserRoutine.Image = userRoutineHairCare.Image;
                            dailyUserRoutine.RoutineTrackerId = trackerId;
                            dailyUserRoutine.CreatedOn = DateTime.Now;
                            dailyUserRoutine.IsActive = true;
                            dailyUserHairStyleRoutineList.Add(dailyUserRoutine);
                        }
                    }
                }
                _context.AddRange(dailyUserRoutineList);
                _context.AddRange(dailyUserProductRoutineList);
                _context.AddRange(dailyUserRegimenRoutineList);
                _context.AddRange(dailyUserHairStyleRoutineList);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public (bool succeeded, string error) SaveUserHairCareItem(UserRoutineHairCareModel userRoutineHairCare)
        {
            try
            {
                int trackerId;
                DailyRoutineTracker dailyRoutineTracker = _context.DailyRoutineTracker.Where(x => x.UserId == userRoutineHairCare.UserName && x.TrackTime.Date == userRoutineHairCare.TrackDate.Date).FirstOrDefault();
                if (dailyRoutineTracker != null)
                    trackerId = dailyRoutineTracker.Id;
                else
                {
                    DailyRoutineTracker routineTracker = new DailyRoutineTracker();
                    routineTracker.UserId = userRoutineHairCare.UserName;
                    routineTracker.TrackTime = userRoutineHairCare.TrackDate.Date;
                    routineTracker.CreatedOn = DateTime.Now;
                    routineTracker.IsCompleted = false;
                    routineTracker.IsActive = true;
                    _context.Add(routineTracker);
                    _context.SaveChanges();
                    trackerId = routineTracker.Id;
                }

                Random generator = new Random();
                string random = generator.Next(0, 10000).ToString("D4");
                if (userRoutineHairCare.IsIngredient)
                {
                    DailyRoutineIngredients dailyUserRoutine = new DailyRoutineIngredients();
                    dailyUserRoutine.IngredientId = Convert.ToInt32(random) + 1000000;
                    dailyUserRoutine.Name = userRoutineHairCare.Name;
                    dailyUserRoutine.Image = userRoutineHairCare.Image;
                    dailyUserRoutine.RoutineTrackerId = trackerId;
                    dailyUserRoutine.CreatedOn = DateTime.Now;
                    dailyUserRoutine.IsActive = true;
                    dailyUserRoutine.IsSelected = false;
                    _context.Add(dailyUserRoutine);
                }
                else if (userRoutineHairCare.IsProduct)
                {
                    DailyRoutineProducts dailyUserRoutine = new DailyRoutineProducts();
                    dailyUserRoutine.ProductId = Convert.ToInt32(random) + 1000000;
                    dailyUserRoutine.Name = userRoutineHairCare.Name;
                    dailyUserRoutine.Image = userRoutineHairCare.Image;
                    dailyUserRoutine.RoutineTrackerId = trackerId;
                    dailyUserRoutine.CreatedOn = DateTime.Now;
                    dailyUserRoutine.IsActive = true;
                    dailyUserRoutine.IsSelected = false;
                    _context.Add(dailyUserRoutine);
                }
                else if (userRoutineHairCare.IsRegimen)
                {
                    DailyRoutineRegimens dailyUserRoutine = new DailyRoutineRegimens();
                    dailyUserRoutine.RegimenId = Convert.ToInt32(random) + 1000000;
                    dailyUserRoutine.Name = userRoutineHairCare.Name;
                    dailyUserRoutine.Image = userRoutineHairCare.Image;
                    dailyUserRoutine.RoutineTrackerId = trackerId;
                    dailyUserRoutine.CreatedOn = DateTime.Now;
                    dailyUserRoutine.IsActive = true;
                    dailyUserRoutine.IsSelected = false;
                    _context.Add(dailyUserRoutine);
                }
                _context.SaveChanges();
                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, "");
            }
        }

        public bool RoutineCompleted(DailyRoutineTracker dailyRoutineTracker, ClaimsPrincipal user)
        {
            try
            {
                var routine = _context.DailyRoutineTracker.Where(x => x.Id == dailyRoutineTracker.Id).FirstOrDefault();
                routine.IsCompleted = dailyRoutineTracker.IsCompleted;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public (bool succeeded, string error) AddProfileImage(DailyRoutineTracker userRoutineHairCare)
        {
            try
            {
                int trackerId;
                DailyRoutineTracker dailyRoutineTracker = _context.DailyRoutineTracker.Where(x => x.UserId == userRoutineHairCare.UserId && x.TrackTime.Date == userRoutineHairCare.TrackTime.Date).FirstOrDefault();
                if (dailyRoutineTracker != null)
                    dailyRoutineTracker.ProfileImage = userRoutineHairCare.ProfileImage;
                else
                {
                    DailyRoutineTracker routineTracker = new DailyRoutineTracker();
                    routineTracker.UserId = userRoutineHairCare.UserId;
                    routineTracker.TrackTime = userRoutineHairCare.TrackTime.Date;
                    routineTracker.ProfileImage = userRoutineHairCare.ProfileImage;
                    routineTracker.CreatedOn = DateTime.Now;
                    routineTracker.IsCompleted = false;
                    routineTracker.IsActive = true;
                    _context.Add(routineTracker);
                }
                _context.SaveChanges();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, "");
            }
        }

    }
}
