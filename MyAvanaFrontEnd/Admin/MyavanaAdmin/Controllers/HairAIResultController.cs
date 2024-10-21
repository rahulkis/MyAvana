using Microsoft.AspNetCore.Mvc;
using MyavanaAdmin.Factory;
using MyavanaAdminModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyavanaAdmin.Controllers
{
    public class HairAIResultController : Controller  
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> HairAIResult(string id, string hairProfileId)
        {
            
                HairProfileCustomerModel hairProfileModel = new HairProfileCustomerModel();
                hairProfileModel.UserId = id;          
                var response = await MyavanaAdminApiClientFactory.Instance.GetLatestCustomerAIResult(hairProfileModel.UserId.ToString());
                if (response != null && !String.IsNullOrEmpty(response.AIResult))
                {
                    var r1 = (Newtonsoft.Json.Linq.JObject)JObject.Parse(JsonConvert.DeserializeObject<dynamic>(response.AIResult.ToString()));

                    if (response.IsVersion2 == true)
                    {
                        var rn = (object)r1["type"]["score"];
                        response.HairTextureLabelAIResult = (string)r1["hairTextureLabel"];
                        response.HairTypeLabelAIResult = (string)r1["hairTypeLabel"];
                        response.LabelAIResult = (string)r1["label"];
                        response.AIResultNewDecoded = (JObject)rn;

                        var texture = (object)r1["texture"]["score"];
                        response.AIResultTextureDecoded = (JObject)texture;

                        response.UserAIImage = (string)r1["imageLink"];
                    }
                    else
                    {
                        var rn = (object)r1["score"];
                        response.HairTextureLabelAIResult = (string)r1["hairTextureLabel"];
                        response.HairTypeLabelAIResult = (string)r1["hairTypeLabel"];
                        response.LabelAIResult = (string)r1["label"];
                        response.AIResultNewDecoded = (JObject)rn;
                    }
                }
                else if (response != null && !String.IsNullOrEmpty(response.AIResult))
                {
                    var result1 = JsonConvert.DeserializeObject<dynamic>(response.AIResult.ToString());

                    if (response.IsAIV2Mobile != true)
                    {
                        var pn = (object)result1["item2"];
                        if (pn == null)
                        {
                            pn = (object)result1["Item2"];
                        }
                        response.AIResultDecoded = (JObject)pn;
                    }
                    else
                    {
                        var rn = (object)result1["type"]["score"];
                        response.AIResultDecoded = (JObject)rn;
                        var texture = (object)result1["texture"]["score"];
                        response.AIResultTextureDecoded = (JObject)texture;
                    }
                }
                return View(response);
          //  }
           
        }
        [HttpPost]
        public async Task<IActionResult> CreateHHCPUsingScalpAnalysis(HHCPParam HHCPParam)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.CreateHHCPUsingScalpAnalysis(HHCPParam);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

    }
}
