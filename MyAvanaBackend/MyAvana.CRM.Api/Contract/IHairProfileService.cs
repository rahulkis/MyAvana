using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IHairProfileService
    {
        HairProfileModel GetHairProfile(string userId);
        HairProfileModel2 GetHairProfile2(string userId,int hairProfileId);
        CollaboratedDetailModel CollaboratedDetails(string profileId);
		CollaboratedDetailModelLocal CollaboratedDetailsLocal(string hairProfileId);

		RecommendedRegimensModel RecommendedRegimens(int regimenId);
        RecommendedProductModel RecommendedProducts(int productId);
        RecommendedProductModel2 RecommendedProducts2(int productId);
        HairProfile SaveProfile(HairProfile regimensModel);
        HairProfileAdminModel GetHairProfileAdmin(HairProfileAdminModel hairProfileModel);
        QuestionaireSelectedAnswer GetQuestionaireAnswer(QuestionaireSelectedAnswer hairProfileModel);
        Task<HairProfileCustomerModel> GetHairProfileCustomer(HairProfileCustomerModel hairProfileModel);
        Task<HairProfileCustomerAlexaModel> GetHairProfileCustomerAlexa(string userId);
        QuestionaireModel GetQuestionaireDetails(QuestionaireModel questionaire);
       Task< QuestionaireModel> GetQuestionaireDetailsMobile(string userId);
		List<HairProfileCustomersModel> GetHairProfileCustomerList(int userId);
        DigitalAssessmentModel CreateHHCPByDigitalAssessment(DigitalAssessmentModel digitalAssessmentModel);
        DigitalAssessmentModel CreateHHCPByDigitalAssessmentForMobile(DigitalAssessmentModel digitalAssessmentModel);
        List<HairProfileSelectModel> GetHHCPList(string userId, bool? isRequestedFromCustomer);

        DigitalAssessmentModel CreateHHCPHairKitUser(DigitalAssessmentModel digitalAssessmentModel);
        List<MessageTemplateModel> GetMessageTempleteList();
        SalonNotesModel SaveSalonNotes(SalonNotesModel salonNotesModel);
        HairStrandsImagesModel UploadHairAnalysisImages(HairStrandsImagesModel hairProfileModel);

        bool DeleteHairStrandImage(HairStrandImageInfo strandImageInfo);
        EnableDisableProfileModel EnableDisableProfileView(EnableDisableProfileModel enableDisableProfileModel);

        Task<HairProfileCustomerModel> GetHairProfileCustomerTab2(HairProfileCustomerModel hairProfileModel);

        Task<HairProfileCustomerModel> GetHairProfileCustomerExceptTab2(HairProfileCustomerModel hairProfileModel);
        List<HairStrandUploadNotificationModel> GetHairStrandUploadNotificationList();

        bool UpdateNotificationAsRead(HairStrandUploadNotificationModel notification);
        DigitalAssessmentModel SaveCustomerAIResultForMobile(DigitalAssessmentModel digitalAssessmentModel);
        Models.Entities.CustomerAIResult GetLatestCustomerAIResult(Guid UserID);
        CustomerHairAIModel GetLatestCustomerAIResultAdmin(Guid UserID);
        List<DailyRoutineTrackerNotificationModel> GetHairDiarySubmitNotificationList();
        bool UpdateNotificationHairDiaryAsRead(DailyRoutineTrackerNotificationModel notification);
        StylistNotesHHCPModel SaveHairStylistNotes(StylistNotesHHCPModel HairStylistNotes);
        DigitalAssessmentModelParameters CreateHHCPUsingScalpAnalysis(DigitalAssessmentModelParameters digitalAssessmentModel);
        QuestionaireModelParameters IsQuestionaireExist(QuestionaireModelParameters QuestionaireModelParam);
        Models.Entities.HairProfile AutoGenerateHHCP(Models.ViewModels.HairProfile hairProfile);
        List<SharedHHCPModel> GetSharedHHCPList(Guid userId);
        bool RevokeAccess(SharedHHCPModel sharedHHCP);
        string ShareEmailExist(string email, int hairProfileId, Guid sharedBy);
        List<SharedHHCPModel> GetSharedWithMeHHCPList(Guid userId);
    }
}
