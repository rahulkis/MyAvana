using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class RegimensModel
    {
        public int RegimensId { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
		[JsonProperty(PropertyName = "Title")]
		public string Title { get; set; }
		[JsonProperty(PropertyName = "Description")]
		public string Description { get; set; }
		public int Step1 { get; set; }
        public int Step2 { get; set; }
        public int Step3 { get; set; }
        public int Step4 { get; set; }
        public int Step5 { get; set; }
        public int RegimenStepsId { get; set; }
        [JsonProperty(PropertyName = "Step1Instruction")]
        public string Step1Instruction { get; set; }
        public IFormFile Step1Photo { get; set; }
        [JsonProperty(PropertyName = "Step1PhotoName")]
        public string Step1PhotoName { get; set; }
        [JsonProperty(PropertyName = "Step2Instruction")]
        public string Step2Instruction { get; set; }
        public IFormFile Step2Photo { get; set; }
        [JsonProperty(PropertyName = "Step2PhotoName")]
        public string Step2PhotoName { get; set; }
        [JsonProperty(PropertyName = "Step3Instruction")]
        public string Step3Instruction { get; set; }
        public IFormFile Step3Photo { get; set; }
        [JsonProperty(PropertyName = "Step3PhotoName")]
        public string Step3PhotoName { get; set; }
        [JsonProperty(PropertyName = "Step4Instruction")]
        public string Step4Instruction { get; set; }
        public IFormFile Step4Photo { get; set; }
        [JsonProperty(PropertyName = "Step4PhotoName")]
        public string Step4PhotoName { get; set; }
        [JsonProperty(PropertyName = "Step5Instruction")]
        public string Step5Instruction { get; set; }
        public IFormFile Step5Photo { get; set; }
        [JsonProperty(PropertyName = "Step5PhotoName")]
        public string Step5PhotoName { get; set; }
		public string Step6Instruction { get; set; }
		public IFormFile Step6Photo { get; set; }
		[JsonProperty(PropertyName = "Step6PhotoName")]
		public string Step6PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step7Instruction")]
		public string Step7Instruction { get; set; }
		public IFormFile Step7Photo { get; set; }
		[JsonProperty(PropertyName = "Step7PhotoName")]
		public string Step7PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step8Instruction")]
		public string Step8Instruction { get; set; }
		public IFormFile Step8Photo { get; set; }
		[JsonProperty(PropertyName = "Step8PhotoName")]
		public string Step8PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step9Instruction")]
		public string Step9Instruction { get; set; }
		public IFormFile Step9Photo { get; set; }
		[JsonProperty(PropertyName = "Step9PhotoName")]
		public string Step9PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step10Instruction")]
		public string Step10Instruction { get; set; }
		public IFormFile Step10Photo { get; set; }
		[JsonProperty(PropertyName = "Step10PhotoName")]
		public string Step10PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step11Instruction")]
		public string Step11Instruction { get; set; }
		public IFormFile Step11Photo { get; set; }
		[JsonProperty(PropertyName = "Step11PhotoName")]
		public string Step11PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step12Instruction")]
		public string Step12Instruction { get; set; }
		public IFormFile Step12Photo { get; set; }
		[JsonProperty(PropertyName = "Step12PhotoName")]
		public string Step12PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step13Instruction")]
		public string Step13Instruction { get; set; }
		public IFormFile Step13Photo { get; set; }
		[JsonProperty(PropertyName = "Step13PhotoName")]
		public string Step13PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step14Instruction")]
		public string Step14Instruction { get; set; }
		public IFormFile Step14Photo { get; set; }
		[JsonProperty(PropertyName = "Step14PhotoName")]
		public string Step14PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step15Instruction")]
		public string Step15Instruction { get; set; }
		public IFormFile Step15Photo { get; set; }
		[JsonProperty(PropertyName = "Step15PhotoName")]
		public string Step15PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step16Instruction")]
		public string Step16Instruction { get; set; }
		public IFormFile Step16Photo { get; set; }
		[JsonProperty(PropertyName = "Step16PhotoName")]
		public string Step16PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step17Instruction")]
		public string Step17Instruction { get; set; }
		public IFormFile Step17Photo { get; set; }
		[JsonProperty(PropertyName = "Step17PhotoName")]
		public string Step17PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step18Instruction")]
		public string Step18Instruction { get; set; }
		public IFormFile Step18Photo { get; set; }
		[JsonProperty(PropertyName = "Step18PhotoName")]
		public string Step18PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step19Instruction")]
		public string Step19Instruction { get; set; }
		public IFormFile Step19Photo { get; set; }
		[JsonProperty(PropertyName = "Step19PhotoName")]
		public string Step19PhotoName { get; set; }
		[JsonProperty(PropertyName = "Step20Instruction")]
		public string Step20Instruction { get; set; }
		public IFormFile Step20Photo { get; set; }
		[JsonProperty(PropertyName = "Step20PhotoName")]
		public string Step20PhotoName { get; set; }

		public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
