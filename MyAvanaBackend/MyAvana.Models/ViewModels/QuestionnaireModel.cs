using Microsoft.AspNetCore.Http;
using MyAvana.Models.Entities;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class QuestionnaireModel
    {
        public int QuestionaireId { get; set; }
        public string UserId { get; set; }
        public int? QuestionId { get; set; }
        public string Question { get; set; }
        public int? AnswerId { get; set; }
        public string Answer { get; set; }
        public string DescriptiveAnswer { get; set; }
        public int QA { get; set; }
        public string QADb { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

    }

    public class QuestionAnswerModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int? QA { get; set; }
        public bool? IsDraft { get; set; }
        public DateTime? CreatedOn { get; set; }
        public ICollection<QuestionModels> questionModel { get; set; }
        public bool? CustomerType { get; set; }
        public int TotalRecords { get; set; }
        public int? CustomerTypeId { get; set; }
        public bool? IsHHCPExist { get; set; }
        public string CustomerQAFrom { get; set; }
        public int? QuestionCount { get; set; }
    }

    public class QuestionModels
    {
        public int? QuestionId { get; set; }
        public string Question { get; set; }
        public int SerialNo { get; set; }
        public string QAType { get; set; }
        public int? QA { get; set; }
        public ICollection<AnswerModel> AnswerList { get; set; }

    }

    public class AnswerModel
    {
        public int? AnswerId { get; set; }
        public string Answer { get; set; }
        public string Description { get; set; }
        public int QA { get; set; }
        public DateTime? QAdate { get; set; }
    }
    public class QuestionnaireCustomerList
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public bool? CustomerType { get; set; }
        public bool? IsProCustomer { get; set; }
        public string CustomerTypeDescription { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public bool? IsQuestionnaire { get; set; }
        public bool? Active { get; set; }
        public string StatusTrackerStatus { get; set; }
        public int CustomerAIResultsCount { get; set; }
        public bool? IsInfluencer { get; set; }
    }
    public class CustomerTypeHistoryModel
    {
        public string OldCustomerType { get; set; }
        public string NewCustomerType { get; set; }
        public string UpdatedBy { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class Entity
    {
        public List<UserEntity> entity { get; set; }
    }

    public class QuestModel
    {
        public string Email { get; set; }

        public int? QA { get; set; }
    }

    public class QuestionGraph
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<AnswerCount> AnswerCounts { get; set; }
    }

    public class AnswerCount
    {
        public int? AnswerId { get; set; }
        public string Answer { get; set; }
        public int Count { get; set; }
    }
    public class CustomerMessageModel
    {
        public int CustomerMessageId { get; set; }
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AttachmentFile { get; set; }
        public bool IsActive { get; set; }
        public string emailBody { get; set; }
        public string emailTemplate { get; set; } = "EMAILTEMPLATE";
        public IFormFile Attachment { get; set; }
    }

    public class CustomerMessageList
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AttachmentFile { get; set; }
    }
    public class SearchCustomerResponse
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
        public string searchValue { get; set; }
        public string userId { get; set; }
        public IList<QuestionnaireCustomerList> Data { get; set; }
    }

    public class QuestionnaireAbsenceModel
    {
        public int userId { get; set; }
        public List<QuestionAnswerModel> QuestionAnswerModels { get; set; }
    }

    public class QuestionaireImageModel
    {
        public IFormFile File { get; set; }
        public string userId { get; set; }
    }
}
