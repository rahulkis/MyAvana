using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;

namespace MyAvana.CRM.Api.Contract
{
    public interface IHairAnalysisStatusService
    {
        List<StatusTrackerModel> GetStatusTrackerList();

        List<HairAnalysisStatusModel> GetHairAnalysisStatusList();
        StatusTrackerModel ChangeHairAnalysisStatus(StatusTrackerModel trackerModel);
        (bool success, string error) SaveHairAnalysisStatus(StatusTracker tracker);

        List<HairAnalysisStatusHistoryList> GetHairAnalysisStatusHistoryList(Guid userId);

        StatusTracker AddToStatusTracker(StatusTracker trackerEntity);
    }
}
