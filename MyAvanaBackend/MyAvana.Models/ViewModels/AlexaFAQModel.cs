using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class AlexaFAQModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string ShortResponse { get; set; }
        public string DetailedResponse { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalRecords { get; set; }
    }
    public class FAQFullDetailsModel
    {
        public string ShortResponse { get; set; }
        public string DetailedResponse { get; set; }
        public string Link { get; set; }
    }
    public class FAQShortResponseModel
    {
        public string ShortResponse { get; set; }
        public string Link { get; set; }
    }
}
