using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaQuestionaire.Models
{
    public class Questionaire
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
}