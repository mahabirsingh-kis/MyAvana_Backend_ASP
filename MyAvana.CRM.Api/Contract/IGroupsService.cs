using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IGroupsService
    {
        (JsonResult result, bool succeeded, string Error) GetPostsUsers(ClaimsPrincipal user);
        (bool succeeded, string Error) CreatePost(GroupPost groupPost);
        (bool succeeded, string Error) CreateComment(ClaimsPrincipal user, Comments comments);
        (JsonResult result, bool succeeded, string Error) GetComments(ClaimsPrincipal user, int postId);
        IEnumerable<Group> CreateGroup(IEnumerable<Group> group);
        IEnumerable<Group> UpdateGroup(IEnumerable<Group> group);
        List<HairTypeUserEntity> GetHairTypeUsers();
        (bool succeeded, string Error) LikeDislikePost(LikePosts likePosts);
        List<GroupsModel> GetGroupList();
        bool DeleteGroup(GroupsModel quest);
        Group IsUserExist(ClaimsPrincipal user,string hairType);
        bool DeletePost(GroupPost post);

    }
}
