using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
        private readonly HttpClient _httpClient;
        public GroupsController(IGroupsService _groupService, IBaseBusiness baseBusiness)
        {
            groupService = _groupService;
            _baseBusiness = baseBusiness;
            _httpClient = new HttpClient();
        }
        [HttpGet("getPostsUsers")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject GetPostsUsers(string hairType)
        {
            var (result, succeeded, error) = groupService.GetPostsUsers(HttpContext.User,hairType);
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
        [HttpPost("CreatePostForMobile")]
        //[Authorize(AuthenticationSchemes = "TestKey")]
        public JObject CreatePostForMobile([FromBody] GroupPostModelParaMeters groupPost)
        {
            var (succeeded, error) = groupService.CreatePostForMobile(groupPost);
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

        [HttpPost("RequestAccessToGroup")]
        public JObject RequestAccessToGroup(Models.ViewModels.GroupModel groupModel)
        {
            bool result = groupService.RequestAccessToGroup(groupModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }

        [HttpGet("GetGroupList")]
        public JObject GetGroupList()
        {
            string email = null;
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;

                email = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            }
           
            List<GroupsModel> result =  groupService.GetGroupList(email);
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

        [HttpPost]
        [Route("DeleteComment")]
        public JObject DeleteComment(CommentsModel commenstModel)
        {
            bool result = groupService.DeleteComment(commenstModel);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", commenstModel);
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

        [HttpPost]
        [Route("UpdateGroupPost")]
        public JObject UpdateGroupPost([FromBody] GroupPostModelParaMeters groupPost)
        {
            bool result = groupService.UpdateGroupPost(groupPost);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", "Updated Successfully");
            else
                return _baseBusiness.AddDataOnJson("Something is wrong!", "0", string.Empty);
        }

        [HttpPost("reportPost")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject ReportPost(ReportPosts reportPosts)
        {
            var (succeeded, error) = groupService.ReportPost(reportPosts);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", true);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", false);
        }

        [HttpGet("GetGroupRequestList")]
        public JObject GetGroupRequestList()
        {
            List<Group> result = groupService.GetGroupRequestList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("ApproveRequest")]
        public JObject ApproveRequest(Models.ViewModels.GroupModel grpModel)
        {
            bool result = groupService.ApproveRequest(grpModel);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", grpModel);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("GetActiveUserList")]
        public JObject GetActiveUserList()
        {
            string email = null;
           // _httpClient.BaseAddress = new Uri("http://localhost:5002/");
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;

                email = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            }
            List<UserList> result = groupService.GetActiveUserList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("SendVideoCallNotification")]
        public JObject SendVideoCallNotification(VideoCallModel Model)
        {
            var result = groupService.SendVideoCallNotification(Model);
            if (result!=null)
                return _baseBusiness.AddDataOnJson("Success", "1", Model);
            else
                return _baseBusiness.AddDataOnJson("Something went wrong!", "0", string.Empty);
        }
        [HttpPost]
        [Route("SendVideoCallNotificationTestAPI")]
        public JObject SendVideoCallNotificationTestAPI(VideoCallModel Model)
        {
            var result = groupService.SendVideoCallNotification(Model);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", Model);
            else
                return _baseBusiness.AddDataOnJson("Something went wrong!", "0", string.Empty);
        }
        #region Mobile API to save/get notification
        [HttpPost]
        [Route("SaveMobileNotification")]
        public JObject SaveMobileNotification([FromBody] MobileNotificationModel Model)
        {
            var result = groupService.SaveMobileNotification(Model);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", Model);
            else
                return _baseBusiness.AddDataOnJson("Something went wrong!", "0", string.Empty);
        }
        [HttpGet("GetMobileNotification")]
        public JObject GetMobileNotification()
        {
            List<MobileNotificationModel> result = groupService.GetMobileNotification();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        #endregion
    }
}