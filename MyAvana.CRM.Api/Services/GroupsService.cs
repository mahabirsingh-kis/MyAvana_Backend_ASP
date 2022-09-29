using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly ITokenService _tokenService;
        private readonly AvanaContext _context;
        //private readonly ILogger _logger;

        public GroupsService(ITokenService tokenService, AvanaContext context)
        {
            _tokenService = tokenService;
            _context = context;
            // _logger = logger;
        }

        public class UserPosts
        {
            public Guid UserId { get; set; }
            public string UserEmail { get; set; }
            public int PostId { get; set; }
            public string UserName { get; set; }
            public string ProfilePic { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public string AudioUrl { get; set; }
            public string VideoUrl { get; set; }
            public long CreatedOn { get; set; }
            public int LikesCount { get; set; }
            public bool IsLike { get; set; }
            public string ThumbnailUrl { get; set; }
        }
        public (JsonResult result, bool succeeded, string Error) GetPostsUsers(ClaimsPrincipal claim)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr.HairType != null)
                {
                    var posts = (from gp in _context.GroupPosts
                                 join us in _context.Users
                                 on gp.UserEmail equals us.Email
                                 where gp.HairType == usr.HairType
                                 select new UserPosts
                                 {
                                     Description = gp.Description,
                                     ImageUrl = gp.ImageUrl,
                                     AudioUrl = gp.AudioUrl,
                                     VideoUrl = gp.VideoUrl,
                                     ThumbnailUrl = gp.ThumbnailUrl,
                                     ProfilePic = us.ImageURL,
                                     PostId = gp.Id,
                                     UserId = us.Id,
                                     UserEmail = us.Email,
                                     UserName = us.FirstName + " " + us.LastName,
                                     CreatedOn = new DateTimeOffset(gp.CreatedOn).ToUnixTimeSeconds(),
                                     IsLike = _context.LikePosts.Where(x => x.PostId == gp.Id && x.UserEmail == usr.Email).Select(z => z.IsLike).FirstOrDefault(),
                                     LikesCount = _context.LikePosts.Where(x => x.PostId == gp.Id && x.IsLike == true).Count()
                                 }).OrderBy(x => x.CreatedOn);
                    return (new JsonResult(posts) { StatusCode = (int)HttpStatusCode.OK }, true, "");

                }
                return (new JsonResult(""), false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                //// _logger.LogError("Error in GetPostsUsers Group Service" + Ex.Message, Ex);
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        public (bool succeeded, string Error) CreatePost(GroupPost groupPost)
        {
            try
            {
                GroupPost group = _context.GroupPosts.Where(x => x.HairType == groupPost.HairType && x.UserEmail == groupPost.UserEmail).LastOrDefault();
                ///var usr = _tokenService.GetAccountNo(claim);
               
                if (groupPost.UserEmail != null)
                {
                    groupPost.CreatedOn = DateTime.Now;
                    groupPost.IsActive = true;
                    _context.GroupPosts.Add(groupPost);
                    _context.SaveChanges();
                    return (true, "");
                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                // _logger.LogError("Error in CreatePost Group Service", Ex);
                return (false, Ex.Message);
            }
        }

        public (bool succeeded, string Error) CreateComment(ClaimsPrincipal claim, Comments comments)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr != null)
                {
                    comments.CreatedOn = DateTime.Now;
                    comments.IsActive = true;
                    comments.UserEmail = usr.Email;
                    _context.Comments.Add(comments);
                    _context.SaveChanges();
                    return (true, "");
                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                // _logger.LogError("Error in CreateComment Group Service", Ex);
                return (false, Ex.Message);
            }
        }

      
        public (JsonResult result, bool succeeded, string Error) GetComments(ClaimsPrincipal claim, int postId)
        {
            try
            {
                var comments = (from cmnt in _context.Comments
                                join usr in _context.Users
                                on cmnt.UserEmail equals usr.Email
                                where cmnt.GroupPostId == postId
                                select new CommentsModel
                                {
                                    Comment = cmnt.Comment,
                                    UserEmail = usr.Email,
                                    CreatedOn = new DateTimeOffset(cmnt.CreatedOn).ToUnixTimeSeconds(),
                                    Image = usr.ImageURL,
                                    UserName = usr.FirstName + " " + usr.LastName
                                }).OrderBy(x => x.CreatedOn).ToList();
                return (new JsonResult(comments) { StatusCode = (int)HttpStatusCode.OK }, true, "");
            }
            catch (Exception Ex)
            {
                //_logger.LogError("Error in GetComments Group Service" + Ex.Message, Ex);
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        public IEnumerable<Group> CreateGroup(IEnumerable<Group> groupList)
        {
            try
            {
                List<string> emails = groupList.Select(x => x.UserEmail).ToList();
                List<Group> groupUsers = _context.Groups.Where(x => x.HairType == groupList.Select(y => y.HairType).FirstOrDefault()).ToList();
                if(groupUsers != null)
                {
                    List<Group> Users = new List<Group>();
                    foreach (var group in groupUsers)
                    {
                        if(emails.Contains(group.UserEmail))
                        {
                            Users.Add(group);
                        }
                    }
                    _context.Groups.RemoveRange(Users);
                    _context.SaveChanges();
                }
                foreach(var group in groupList)
                {
                    group.CreatedOn = DateTime.Now;
                    group.IsActive = true;
                }
                _context.Groups.AddRange(groupList);
                _context.SaveChanges();
                return groupList; // (new JsonResult(groupList) { StatusCode = (int)HttpStatusCode.OK },true, "");
            }
            catch (Exception Ex)
            {
                // _logger.LogError("Error in CreateGroup Group Service", Ex);
                return null; // (new JsonResult(""), false, Ex.Message);
            }
        }

        public IEnumerable<Group> UpdateGroup(IEnumerable<Group> groupList)
        {
            try
            {
                List<Group> groupUsers = _context.Groups.Where(x => x.HairType == groupList.Select(y => y.HairType).FirstOrDefault()).ToList();
                if (groupUsers != null)
                {
                    _context.Groups.RemoveRange(groupUsers);
                    _context.SaveChanges();
                }
                foreach (var group in groupList)
                {
                    group.CreatedOn = DateTime.Now;
                    group.IsActive = true;
                }
                _context.Groups.AddRange(groupList);
                _context.SaveChanges();
                return groupList; // (new JsonResult(groupList) { StatusCode = (int)HttpStatusCode.OK },true, "");
            }
            catch (Exception Ex)
            {
                // _logger.LogError("Error in CreateGroup Group Service", Ex);
                return null; // (new JsonResult(""), false, Ex.Message);
            }
        }
        public (JsonResult result, bool succeeded, string Error) GetGroupUsers(string hairType)
        {
            try
            {
                var users = _context.Groups.Where(x => x.HairType == hairType).ToList();
                return (new JsonResult(users) { StatusCode = (int)HttpStatusCode.OK }, true, "");
            }
            catch (Exception Ex)
            {
                // _logger.LogError("Error in Group Service" + Ex.Message, Ex);
                return (new JsonResult(""), false, Ex.Message);
            }
        }

       
        public List<HairTypeUserEntity> GetHairTypeUsers()
        {
            try
            {
                List<HairTypeUserEntity> hairTypeUserEntities = new List<HairTypeUserEntity>();
                var hairtypes = _context.Users.Where(x => x.HairType != null).ToList();
                foreach(var hairUser in hairtypes)
                {
                    HairTypeUserEntity hairTypeUserEntity = new HairTypeUserEntity();
                    hairTypeUserEntity.HairType = hairUser.HairType;
                    hairTypeUserEntity.UserEmail = hairUser.Email;
                    hairTypeUserEntity.UserName = hairUser.FirstName + " " + hairUser.LastName;
                    hairTypeUserEntity.UserId = hairUser.Id.ToString();
                    hairTypeUserEntities.Add(hairTypeUserEntity);
                }
                return hairTypeUserEntities;
            }
            catch (Exception Ex)
            {
                //_logger.LogError("Error in GetHairTypeUsers Group Service" + Ex.Message, Ex);
                return null;
            }
        }

        public (bool succeeded, string Error) LikeDislikePost(LikePosts likePosts)
        {
            try
            {
                LikePosts result = _context.LikePosts.Where(x => x.PostId == likePosts.PostId && x.UserEmail == likePosts.UserEmail).FirstOrDefault();
                if (result != null)
                {
                    result.IsLike = likePosts.IsLike;
                    result.ModifiedOn = DateTime.Now;
                }
                else
                {
                    likePosts.ModifiedOn = DateTime.Now;
                    _context.LikePosts.Add(likePosts);
                }
                _context.SaveChanges();
                return (true, "Like updated");
            }
            catch (Exception Ex)
            {
                //_logger.LogError("Error in GetHairTypeUsers Group Service" + Ex.Message, Ex);
                return (false, Ex.Message);
            }
        }

        public List<GroupsModel> GetGroupList()
        {
            try
            {
                var userGroups = (from grp in _context.Groups
                                  join usr in _context.Users
                                  on grp.UserEmail equals usr.Email
                                  select new
                                  {
                                      HType = grp.HairType,
                                      UserName = usr.FirstName + " " + usr.LastName,
                                      UserEmail = usr.Email,
                                      UserId = usr.Id.ToString()
                                  }).ToList();
                List<GroupsModel> groups = new List<GroupsModel>();
                List<string> hairtypes = userGroups.Select(x => x.HType).Distinct().ToList();
                foreach(var hairType in hairtypes)
                {
                    GroupsModel gpModel = new GroupsModel();
                    gpModel.HairType = hairType;
                    gpModel.Users = userGroups.Where(x => x.HType == hairType).Select(y => new GpUsers { UserEmail = y.UserEmail, UserName = y.UserName, UserId = y.UserId }).ToList();
                    groups.Add(gpModel);
                }
                return groups;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public bool DeleteGroup(GroupsModel grpModel)
        {
            var users = _context.Groups.Where(x => x.HairType == grpModel.HairType).ToList();
            try
            {
                _context.RemoveRange(users);
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }

        public Group IsUserExist(ClaimsPrincipal user, string hairType)
        {
            try
            {
                Group group = null; 
                var usr = _tokenService.GetAccountNo(user);
                if (usr != null)
                {
                    group = _context.Groups.Where(x => x.UserEmail == usr.Email && x.HairType == hairType).FirstOrDefault();
                }
                return group;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeletePost(GroupPost groupPost)
        {
            var likePosts = _context.LikePosts.Where(x => x.PostId == groupPost.Id).ToList();
            var commentposts = _context.GroupPosts.Include(x => x.Comments).Where(y => y.Id == groupPost.Id).FirstOrDefault();
            try
            {
                if (likePosts.Count > 0)
                    _context.RemoveRange(likePosts);
                if (commentposts != null)
                {
                    _context.Remove(commentposts);
                    _context.SaveChanges();
                }
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }
    }
}
