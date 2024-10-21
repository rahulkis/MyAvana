using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HairAnalysisStatusController : ControllerBase
    {
        private readonly IHairAnalysisStatusService _HairAnalysisStatus;
        private readonly IBaseBusiness _baseBusiness;
        //private readonly IHostingEnvironment _environment;
        //private readonly HttpClient _httpClient;
        //private readonly AvanaContext _context;
        public HairAnalysisStatusController(IHairAnalysisStatusService HairAnalysisService, IBaseBusiness baseBusiness)
        {
            _HairAnalysisStatus = HairAnalysisService;
            _baseBusiness = baseBusiness;
        }

        [HttpGet("GetStatusTrackerList")]
        public JObject GetStatusTrackerList()
        {
            List<StatusTrackerModel> result = _HairAnalysisStatus.GetStatusTrackerList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetHairAnalysisStatusList")]
        public JObject GetHairAnalysisStatusList()
        {
            List<HairAnalysisStatusModel> result = _HairAnalysisStatus.GetHairAnalysisStatusList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost("ChangeHairAnalysisStatus")]
        public JObject ChangeHairAnalysisStatus(StatusTrackerModel trackerModel)
        {
            StatusTrackerModel result = _HairAnalysisStatus.ChangeHairAnalysisStatus(trackerModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetHairAnalysisStatusHistoryList")]
        public JObject GetHairAnalysisStatusHistoryList(string id)
        {
            List<HairAnalysisStatusHistoryList> result = _HairAnalysisStatus.GetHairAnalysisStatusHistoryList(new Guid(id));
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost("AddToStatusTracker")]
        public JObject AddToStatusTracker(StatusTracker trackerEntity)
        {
            StatusTracker result = _HairAnalysisStatus.AddToStatusTracker(trackerEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
    }
}
