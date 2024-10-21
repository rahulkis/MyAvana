using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class NotificationModel
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("isAndroiodDevice")]
        public bool IsAndroiodDevice { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }
    public class VideoCallNotificationModel
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("isAndroiodDevice")]
        public bool IsAndroiodDevice { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("RoomId")]
        public string RoomId { get; set; }
        [JsonProperty("RoomClass")]
        public string RoomClass { get; set; }
    }
    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
        }
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public DataPayload Data { get; set; }
        [JsonProperty("notification")]
        public DataPayload Notification { get; set; }
    }
    public class GoogleVideoCallNotification
    {
        public class VideoDataPayload
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
            [JsonProperty("RoomId")]
            public string RoomId { get; set; }
            [JsonProperty("RoomClass")]
            public string RoomClass { get; set; }
        }
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public VideoDataPayload Data { get; set; }
        [JsonProperty("notification")]
        public VideoDataPayload Notification { get; set; }
    }
    public class ResponseModel
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class FcmNotificationSetting
    {
        public string SenderId { get; set; }
        public string ServerKey { get; set; }
    }
    public class MobileNotificationModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
