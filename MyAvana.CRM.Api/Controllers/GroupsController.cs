using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private IGroupsService groupService;
        private IBaseBusiness _baseBusiness;
        public GroupsController(IGroupsService _groupService, IBaseBusiness baseBusiness)
        {
            groupService = _groupService;
            _baseBusiness = baseBusiness;
        }
        [HttpGet("getPostsUsers")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject GetPostsUsers()
        {
            var (result, succeeded, error) = groupService.GetPostsUsers(HttpContext.User);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }

        [HttpPost("createpost")]
        //[Authorize(AuthenticationSchemes = "TestKey")]
        public JObject CreatePost([FromBody]GroupPost groupPost)
        {
            var (succeeded, error) = groupService.CreatePost(groupPost);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", groupPost);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }

        [HttpPost("createcomment")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject CreateComment([FromBody]Comments comment)
        {
            var (succeeded, error) = groupService.CreateComment(HttpContext.User, comment);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", true);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }

        [HttpGet("getcomments")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject GetComments(int postId)
        {
            var (result, succeeded, error) = groupService.GetComments(HttpContext.User, postId);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }

        [HttpPost("creategroup")]
        public JObject CreateGroup(IEnumerable<Group> group)
        {
            IEnumerable<Group> result = groupService.CreateGroup(group);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }
        [HttpPost("updategroup")]
        public JObject UpdateGroup(IEnumerable<Group> group)
        {
            IEnumerable<Group> result = groupService.UpdateGroup(group);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }

        [HttpGet("GetGroupList")]
        public JObject GetGroupList()
        {
            List<GroupsModel> result =  groupService.GetGroupList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("gethairtypeusers")]
       /// [Authorize(AuthenticationSchemes = "TestKey")]
        public IActionResult GetHairTypeUsers()
        {
            List<HairTypeUserEntity> hairTypeList = groupService.GetHairTypeUsers();
            if (hairTypeList != null)
                return Ok(new JsonResult(hairTypeList) { StatusCode = (int)HttpStatusCode.OK });
            else
                return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("likedislikepost")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject LikeDislikePost(LikePosts likePosts)
        {
            var (succeeded, error) = groupService.LikeDislikePost(likePosts);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", true);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }

        [HttpPost]
        [Route("DeleteGroup")]
        public JObject DeleteGroup(GroupsModel grpModel)
        {
            bool result = groupService.DeleteGroup(grpModel);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", grpModel);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("isuserexist")]
         [Authorize(AuthenticationSchemes = "TestKey")]
        public IActionResult IsUserExist(string hairType)
        {
            Group user = groupService.IsUserExist(HttpContext.User, hairType);
            if (user != null)
                return Ok(new JsonResult(user) { StatusCode = (int)HttpStatusCode.OK });
            else
                return BadRequest(new JsonResult("User doesn't exist") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [Route("deletepost")]
        public JObject DeletePost(GroupPost  groupPost)
        {
            bool result = groupService.DeletePost(groupPost);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", "Deleted Successfully");
            else
                return _baseBusiness.AddDataOnJson("Something is wrong!", "0", string.Empty);
        }

    }
}