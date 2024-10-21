using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly ITokenService _tokenService;
        private readonly AvanaContext _context;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;
        //private readonly ILogger _logger;
        private readonly Logger.Contract.ILogger _logger;

        public GroupsService(ITokenService tokenService, AvanaContext context, IEmailService emailService, INotificationService notificationService, Logger.Contract.ILogger logger)
        {
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
            _notificationService = notificationService;
            _logger = logger;
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
            public int CommentsCount { get; set; }
            public DateTime? LastPingTime { get; set; }
            public List< TaggedUsersList> taggeduserlist { get; set; }            
        }
        public class TaggedUsersList
        {
            public int? PostId { get; set; }
            public Guid UserId { get; set; }
            public string UserName { get; set; }

        }
        public (JsonResult result, bool succeeded, string Error) GetPostsUsers(ClaimsPrincipal claim, string hairType)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr != null)
                {
                    var posts = (from gp in _context.GroupPosts
                                 join us in _context.Users
                                 on gp.UserEmail equals us.Email
                                 //where gp.HairType == usr.HairType
                                 where ((hairType == null || hairType == "") ? gp.HairType != null : gp.HairType == hairType) && gp.IsActive == true
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
                                     LikesCount = _context.LikePosts.Where(x => x.PostId == gp.Id && x.IsLike == true).Count(),
                                     CommentsCount = _context.Comments.Where(x => x.GroupPostId == gp.Id && x.IsActive == true).Count(),
                                     LastPingTime= us.LastPingTime,
                                     taggeduserlist = (from tgu in _context.TaggedUsers
                                                          where tgu.PostId == gp.Id
                                                          select new TaggedUsersList
                                                          {
                                                              PostId = tgu.PostId,
                                                              UserId = tgu.UserId,
                                                              UserName = tgu.UserName
                                                          }).ToList()
                                 }).OrderByDescending(x => x.CreatedOn) ;
                    return (new JsonResult(posts) { StatusCode = (int)HttpStatusCode.OK }, true, "");

                }
                else
                {
                    _logger.LogError("Method: GetPostsUsers, Error: Invalid user.");
                    return (new JsonResult(""), false, "Invalid user.");
                }   
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetPostsUsers, Error: " + Ex.Message, Ex);
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
                else
                {
                    _logger.LogError("Method: CreatePost, Email:" + groupPost.UserEmail + ", Error: Invalid user.");
                    return (false, "Invalid user.");
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreatePost, Email:" + groupPost.UserEmail + ", Error: " + Ex.Message, Ex);
                return (false, Ex.Message);
            }
        }
        public (bool succeeded, string Error) CreatePostForMobile(GroupPostModelParaMeters GroupPostParam)
        {
            try
            {
                GroupPost group = _context.GroupPosts.Where(x => x.HairType == GroupPostParam.HairType && x.UserEmail == GroupPostParam.UserEmail).LastOrDefault();
                ///var usr = _tokenService.GetAccountNo(claim);

                if (GroupPostParam.UserEmail != null)
                {
                    GroupPost groupPost = new GroupPost();
                    groupPost.HairType = GroupPostParam.HairType;
                    groupPost.UserEmail = GroupPostParam.UserEmail;
                    groupPost.Description = GroupPostParam.Description;
                    groupPost.ImageUrl = GroupPostParam.ImageUrl;
                    groupPost.AudioUrl = GroupPostParam.AudioUrl;
                    groupPost.VideoUrl = GroupPostParam.VideoUrl;
                    groupPost.ThumbnailUrl = GroupPostParam.ThumbnailUrl;
                    groupPost.Comments = GroupPostParam.Comments;
                    groupPost.CreatedOn = DateTime.Now;
                    groupPost.IsActive = true;
                    _context.GroupPosts.Add(groupPost);
                    _context.SaveChanges();
                    int postid = groupPost.Id;
                    foreach (var tgitem in GroupPostParam.TaggedUsersList)
                    {
                        TaggedUsers tgu = new TaggedUsers();
                        tgu.PostId = postid;
                        tgu.UserId = tgitem.UserId;
                        tgu.UserName = tgitem.UserName;
                        _context.TaggedUsers.Add(tgu);
                        _context.SaveChanges();
                        var user = _context.Users.Where(x => x.Email.ToLower() == groupPost.UserEmail.ToLower()).FirstOrDefault();
                        var TgUser = _context.Users.Where(x => x.Id == tgitem.UserId).FirstOrDefault();
                        if (!string.IsNullOrEmpty(TgUser.DeviceId))
                        {
                            var bodyText = $"{user.FirstName +" "+ user.LastName} mentioned you in their community post.";
                            NotificationModel notificationModel = new NotificationModel();
                            notificationModel.Title = "You've been tagged in a community post!";
                            notificationModel.Body = bodyText;
                            notificationModel.DeviceId = TgUser.DeviceId;
                            //notificationModel.IsAndroiodDevice = true;
                            var result = _notificationService.SendNotification(notificationModel);
                        }
                    }

                    return (true, "");
                }
                else
                {
                    _logger.LogError("Method: CreatePostForMobile, Email:" + GroupPostParam.UserEmail + ", Error: Invalid user.");
                    return (false, "Invalid user.");
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreatePostForMobile, Email:" + GroupPostParam.UserEmail + ", Error: " + Ex.Message, Ex);
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
                else
                {
                    _logger.LogError("Method: CreateComment, Error: Invalid user.");
                    return (false, "Invalid user.");
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateComment, Error: " + Ex.Message, Ex);
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
                                    UserName = usr.FirstName + " " + usr.LastName,
                                    Id = cmnt.Id
                                }).OrderBy(x => x.CreatedOn).ToList();
                return (new JsonResult(comments) { StatusCode = (int)HttpStatusCode.OK }, true, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetComments, Error: " + Ex.Message, Ex);
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
                    group.IsPublic = group.IsPublic;
                }
                _context.Groups.AddRange(groupList);
                _context.SaveChanges();
                return groupList; // (new JsonResult(groupList) { StatusCode = (int)HttpStatusCode.OK },true, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: CreateGroup, Error: " + Ex.Message, Ex);
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
                    foreach (var group in groupList)
                    {
                        Group matchingGroupUser = groupUsers.Find(g => g.UserEmail == group.UserEmail);
                        if (matchingGroupUser != null)
                        {
                            group.UpdatedOn = matchingGroupUser.UpdatedOn;
                            group.UpdatedByUserId = matchingGroupUser.UpdatedByUserId;
                        }
                    }
                    _context.Groups.RemoveRange(groupUsers);
                    _context.SaveChanges();
                }
                foreach (var group in groupList)
                {
                    group.CreatedOn = DateTime.Now;
                    group.IsActive = true;
                    group.IsPublic = group.IsPublic;
                }
                _context.Groups.AddRange(groupList);
                _context.SaveChanges();
                return groupList; // (new JsonResult(groupList) { StatusCode = (int)HttpStatusCode.OK },true, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: UpdateGroup, Error: " + Ex.Message, Ex);
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
                _logger.LogError("Method: GetGroupUsers, Error: " + Ex.Message, Ex);
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
                _logger.LogError("Method: GetHairTypeUsers, Error: " + Ex.Message, Ex);
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
                _logger.LogError("Method: LikeDislikePost, PostId:"+ likePosts.PostId + ", Error: " + Ex.Message, Ex);
                return (false, Ex.Message);
            }
        }

        public List<GroupsModel> GetGroupList(string email)
        {
            try
            {
                var userGroups = (from grp in _context.Groups
                                  join web in _context.WebLogins on grp.UpdatedByUserId equals web.UserId into webLoginsGroup
                                  from webLogin in webLoginsGroup.DefaultIfEmpty()
                                  where grp.IsActive == true
                                  && (grp.IsPublic || _context.Users.Any(usr => usr.Email == grp.UserEmail))
                                  select new
                                  {
                                      HType = grp.HairType,
                                      UserName = grp.IsPublic ? null : (from usr in _context.Users
                                                                        where usr.Email == grp.UserEmail
                                                                        select usr.FirstName + " " + usr.LastName).FirstOrDefault(),
                                      UserEmail = grp.IsPublic ? null : (from usr in _context.Users
                                                                         where usr.Email == grp.UserEmail
                                                                         select usr.Email).FirstOrDefault(),
                                      UserId = grp.IsPublic ? null : (from usr in _context.Users
                                                                      where usr.Email == grp.UserEmail
                                                                      select usr.Id.ToString()).FirstOrDefault(),
                                      Id = grp.Id,
                                      IsPublic = grp.IsPublic,
                                      ShowOnMobile = grp.ShowOnMobile,
                                      UpdatedOn = grp.UpdatedOn,
                                      UpdatedByUser = webLogin != null ? webLogin.UserEmail : null
                                  }).ToList();

                List<GroupsModel> groups = new List<GroupsModel>();
                List<string> hairtypes = userGroups.Select(x => x.HType).Distinct().ToList();
                foreach (var hairType in hairtypes)
                {
                    GroupsModel gpModel = new GroupsModel();
                    gpModel.HairType = hairType;
                    gpModel.Id = userGroups.Where(x => x.HType == hairType).Select(y => y.Id).FirstOrDefault();
                    gpModel.Users = userGroups.Where(x => x.HType == hairType).Select(y => new GpUsers { UserEmail = y.UserEmail, UserName = y.UserName, UserId = y.UserId, UpdatedOn = y.UpdatedOn, ApprovedBy = y.UpdatedByUser }).ToList();
                    gpModel.IsGroupMember = !string.IsNullOrEmpty(email) ? gpModel.Users.Count() == 1 && gpModel.Users.Select(x => x.UserEmail).FirstOrDefault() == null ? false :
                        gpModel.Users.Where(x => x.UserEmail != null && x.UserEmail.ToLower() == email.ToLower()).FirstOrDefault() != null ? true : false : false;
                    gpModel.IsPublic = userGroups.Where(x => x.HType == hairType).Select(y => y.IsPublic).FirstOrDefault();
                    gpModel.ShowOnMobile = userGroups.Where(x => x.HType == hairType).Select(y => y.ShowOnMobile).FirstOrDefault();
                    groups.Add(gpModel);
                }
                return groups;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetGroupList, Email:" + email + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteGroup(GroupsModel grpModel)
        {
            var users = _context.Groups.Where(x => x.HairType == grpModel.HairType).ToList();
            try
            {
                foreach (var item in users)
                {
                    item.IsActive = false;
                }
                _context.UpdateRange(users);
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteGroup, HairType:" + grpModel.HairType + ", Error: " + Ex.Message, Ex);
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
                _logger.LogError("Method: IsUserExist, HairType:" + hairType + ", Error: " + ex.Message, ex);
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

            catch (Exception ex)
            {
                _logger.LogError("Method: DeletePost, PostId:" + groupPost.Id + ", Error: " + ex.Message, ex);
                return false;
            }
        }

        public bool UpdateGroupPost(GroupPostModelParaMeters GroupPostParam)
        {
            var commentposts = _context.GroupPosts.Where(x => x.Id== GroupPostParam.Id).LastOrDefault();            
            try
            {
                bool IsTaggedUserExist = false;
                if (commentposts != null)
                {                   
                    commentposts.Description = GroupPostParam.Description;           
                    _context.SaveChanges();
                    int postid = commentposts.Id;
                    var Alltaggeduser = _context.TaggedUsers.Where(x => x.PostId == postid).ToList();
                    _context.RemoveRange(Alltaggeduser);
                    _context.SaveChanges();                
                    foreach (var tgitem in GroupPostParam.TaggedUsersList)
                    {
                        IsTaggedUserExist = false;                      
                        for (int i = 0; i < Alltaggeduser.Count; i++)
                        {
                            if (Alltaggeduser[i].UserId == tgitem.UserId)
                            {
                                IsTaggedUserExist = true;
                                break;
                            }
                        }                    
                        TaggedUsers tgu = new TaggedUsers();
                        tgu.PostId = postid;
                        tgu.UserId = tgitem.UserId;
                        tgu.UserName = tgitem.UserName;
                        _context.TaggedUsers.Add(tgu);
                        _context.SaveChanges();                
                        if (!IsTaggedUserExist)
                        {
                            var user = _context.Users.Where(x => x.Email.ToLower() == commentposts.UserEmail.ToLower()).FirstOrDefault();
                            var TgUser = _context.Users.Where(x => x.Id == tgitem.UserId).FirstOrDefault();
                            if (!string.IsNullOrEmpty(TgUser.DeviceId))
                            {
                                var bodyText = $"{user.FirstName + " " + user.LastName} mentioned you in their community post.";
                                NotificationModel notificationModel = new NotificationModel();
                                notificationModel.Title = "You've been tagged in a community post!";
                                notificationModel.Body = bodyText;
                                notificationModel.DeviceId = TgUser.DeviceId;
                                //notificationModel.IsAndroiodDevice = true;
                                var result = _notificationService.SendNotification(notificationModel);
                            }
                        }
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError("Method: UpdateGroupPost, PostId:" + GroupPostParam.Id + ", Error: " + ex.Message, ex);
                return false;
            }
        }
        public bool RequestAccessToGroup(MyAvana.Models.ViewModels.GroupModel groupModel)
        {
            var group = _context.Groups.Where(y => y.Id == groupModel.Id).FirstOrDefault();
            bool AlreadyExistUser = _context.Groups.Where(y => y.UserEmail == groupModel.UserEmail && y.HairType == group.HairType).Any();
            try
            {
                if (group != null)
                {
                    if (!AlreadyExistUser)
                    {
                        if (groupModel.IsPublic)
                        {

                            var obj = new Group();
                            obj.UserEmail = groupModel.UserEmail;
                            obj.AccessRequested = false;
                            obj.IsActive = true;
                            obj.IsPublic = true;
                            obj.ShowOnMobile = group.ShowOnMobile;
                            obj.CreatedOn = DateTime.Now;
                            obj.HairType = group.HairType;
                            _context.Groups.Add(obj);
                            _context.SaveChanges();
                        }
                        else
                        {
                            var obj = new Group();
                            obj.UserEmail = groupModel.UserEmail;
                            obj.AccessRequested = true;
                            obj.IsActive = false;
                            obj.CreatedOn = DateTime.Now;
                            obj.ShowOnMobile = group.ShowOnMobile;
                            obj.HairType = group.HairType;
                            _context.Groups.Add(obj);
                            _context.SaveChanges();
                            // Send email to Admin
                            EmailInformation emailInformationadmin = new EmailInformation
                            {
                                Name = "Admin",
                                GroupName = group.HairType,
                                userEmail = groupModel.UserEmail
                            };
                            _emailService.SendEmail("GROUPREQUEST", emailInformationadmin);
                        }
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError("Method: RequestAccessToGroup, GroupId:" + groupModel.Id + ", Error: " + ex.Message, ex);
                return false;
            }
        }

        public bool DeleteComment(CommentsModel commenstModel)
        {
            var comments = _context.Comments.Where(y => y.Id == commenstModel.Id).FirstOrDefault();
            try
            {
                if (comments != null)
                {
                    _context.Remove(comments);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: DeleteComment, CommentId:" + commenstModel.Id + ", Error: " + ex.Message, ex);
                return false;
            }
        }
        public (bool succeeded, string Error) ReportPost(ReportPosts reportPosts)
        {
            try
            {
                var post = _context.GroupPosts.FirstOrDefault(x => x.Id == reportPosts.PostId);
                reportPosts.CreatedOn = DateTime.Now;
                _context.ReportPosts.Add(reportPosts);
                _context.SaveChanges();

                EmailInformation emailInformation = new EmailInformation
                {
                    Code = post.Description
                };

                var emailRes = _emailService.SendEmail("POSTREPORT", emailInformation);
                return (true, "Post reported");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: ReportPost, PostId:" + reportPosts.PostId + ", Error: " + ex.Message, ex);
                return (false, ex.Message);
            }
        }
        public List<Group> GetGroupRequestList()
        {
            try
            {
                //List<Models.ViewModels.GroupModel> groups = new List<Models.ViewModels.GroupModel>();
                var req = _context.Groups.Where(x => x.AccessRequested == true && x.IsActive == false);
                //foreach (var item in req)
                //{
                //    var obj = new Models.ViewModels.GroupModel();
                //    obj.Id = item.Id;
                //    obj.CreatedOn = item.CreatedOn;
                //    obj.IsActive = item.IsActive;
                //    obj.HairType = item.HairType;
                //    obj.UserEmail = item.UserEmail;
                //}
                return req.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetGroupRequestList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool ApproveRequest(Models.ViewModels.GroupModel groupModel)
        {
            var grp = _context.Groups.Where(x => x.Id == groupModel.Id).FirstOrDefault();
            try
            {
                if(grp != null)
                {
                    grp.IsActive = true;
                    grp.UpdatedOn = DateTime.Now;
                    grp.UpdatedByUserId = groupModel.UpdatedByUserId;
                    _context.Groups.Update(grp);
                    _context.SaveChanges();

                    // Send email to Customer
                    EmailInformation emailInformationadmin = new EmailInformation
                    {
                        GroupName = grp.HairType,
                        Email = grp.UserEmail
                    };
                    _emailService.SendEmail("GROUPREQUESTAPPROVED", emailInformationadmin);
                    
                    var user = _context.Users.Where(x => x.Email.ToLower() == grp.UserEmail.ToLower()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(user.DeviceId))
                    {
                        var bodyText = "Congrats! Your Group Access Request is approved";
                        NotificationModel notificationModel = new NotificationModel();
                        notificationModel.Title = "Group Access Request Approved";
                        notificationModel.Body = bodyText;
                       	notificationModel.DeviceId = user.DeviceId;
                        //notificationModel.IsAndroiodDevice = true;
                        var result = _notificationService.SendNotification(notificationModel);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

            catch (Exception ex)
            {
                _logger.LogError("Method: ApproveRequest, GroupId:" + groupModel.Id + ", Error: " + ex.Message, ex);
                return false;
            }
        }
        public List<UserList> GetActiveUserList() 
        {
            List<UserList> guserlist = new List<UserList>();
            guserlist = (from us in _context.Users
                         where us.Active == true
                         select new UserList
                         {
                             UserID=us.Id,
                            EmailID=us.Email,
                             UserName = us.FirstName + " " + us.LastName,
                             imageurl=us.ImageURL
                         }).ToList();       
               return guserlist;       
        }

       public async Task<ResponseModel> SendVideoCallNotification(VideoCallModel model)
        {
            ResponseModel result = new ResponseModel();
            try
            {
              
                var userslist = _context.Users.Where(x => x.Id != model.UserId && x.Active == true).OrderByDescending(x=>x.CreatedAt).ToList();
                foreach (var user in userslist)
                {
                    if (!string.IsNullOrEmpty(user.DeviceId))
                    {
                        var bodyText = "";
                        VideoCallNotificationModel notificationModel = new VideoCallNotificationModel
                        {
                            Title = model.UserName + " is Live on the Community Post",
                            Body = bodyText,
                            DeviceId = user.DeviceId,
                            RoomId=model.RoomId,
                            RoomClass=model.RoomClass
                        };

                         result =await _notificationService.SendVideoNotification(notificationModel);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SendVideoCallNotification, RooomId:" + model.RoomId + ", Error: " + ex.Message, ex);
                return result;
            }
        }
        public async Task<ResponseModel> SendVideoCallNotificationTestAPI(VideoCallModel model)
        {
            ResponseModel result = new ResponseModel();
            try
            {

                var userslist = _context.Users.Where(x => x.Id != model.UserId && x.Active == true).OrderByDescending(x => x.CreatedAt).Take(5).ToList();
                foreach (var user in userslist)
                {
                    if (!string.IsNullOrEmpty(user.DeviceId))
                    {
                        var bodyText = "";
                        VideoCallNotificationModel notificationModel = new VideoCallNotificationModel
                        {
                            Title = model.UserName + " is Live on the Community Post",
                            Body = bodyText,
                            DeviceId = user.DeviceId,
                            RoomId = model.RoomId,
                            RoomClass = model.RoomClass
                        };

                        result = await _notificationService.SendVideoNotification(notificationModel);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SendVideoCallNotificationTestAPI, RooomId:" + model.RoomId + ", Error: " + ex.Message, ex);
                return result;
            }
        }
        public async Task<ResponseModel> SaveMobileNotification(MobileNotificationModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                MobileNotifications mobileNotifications = new MobileNotifications();
                if (model.Title != null)
                {
                    mobileNotifications.Title = model.Title;
                    mobileNotifications.Description=model.Description;
                    mobileNotifications.CreatedOn= DateTime.Now;
                    mobileNotifications.IsActive= true;
                    _context.MobileNotifications.Add(mobileNotifications);
                    _context.SaveChanges();
                    response.IsSuccess = true;
                    response.Message = "Notification save successfully";
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveMobileNotification, Title:" + model.Title + ", Error: " + ex.Message, ex);
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }
        public List<MobileNotificationModel> GetMobileNotification()
        {
            List<MobileNotificationModel> mobileNotification = new List<MobileNotificationModel>();
            mobileNotification = (from mn in _context.MobileNotifications
                         select new MobileNotificationModel
                         {
                             Title= mn.Title,
                             Description= mn.Description,
                             CreatedOn=mn.CreatedOn,
                             IsActive=mn.IsActive
                         }).ToList();
            return mobileNotification;
        }
    }
}
