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
        (JsonResult result, bool succeeded, string Error) GetPostsUsers(ClaimsPrincipal user, string hairType);
        (bool succeeded, string Error) CreatePost(GroupPost groupPost);
        (bool succeeded, string Error) CreateComment(ClaimsPrincipal user, Comments comments);
        (JsonResult result, bool succeeded, string Error) GetComments(ClaimsPrincipal user, int postId);
        IEnumerable<Group> CreateGroup(IEnumerable<Group> group);
        IEnumerable<Group> UpdateGroup(IEnumerable<Group> group);
        List<HairTypeUserEntity> GetHairTypeUsers();
        (bool succeeded, string Error) LikeDislikePost(LikePosts likePosts);
        List<GroupsModel> GetGroupList(string email);
        bool DeleteGroup(GroupsModel quest);
        Group IsUserExist(ClaimsPrincipal user,string hairType);
        bool DeletePost(GroupPost post);
        (bool succeeded, string Error) ReportPost(ReportPosts reportPosts);
        bool DeleteComment(CommentsModel commenstModel);
        bool UpdateGroupPost(GroupPostModelParaMeters post);
        bool RequestAccessToGroup(Models.ViewModels.GroupModel groupModel);
        List<Group> GetGroupRequestList();
        bool ApproveRequest(Models.ViewModels.GroupModel groupModel);
        List<UserList> GetActiveUserList();
        (bool succeeded, string Error) CreatePostForMobile(GroupPostModelParaMeters groupPost);
        Task<ResponseModel> SendVideoCallNotification(VideoCallModel model);
        Task<ResponseModel> SendVideoCallNotificationTestAPI(VideoCallModel model);
        Task<ResponseModel> SaveMobileNotification(MobileNotificationModel model);
        List<MobileNotificationModel> GetMobileNotification();
    }
}
