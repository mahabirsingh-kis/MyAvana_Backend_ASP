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
        HairProfileModel2 GetHairProfile2(string userId);
        CollaboratedDetailModel CollaboratedDetails(string profileId);
		CollaboratedDetailModelLocal CollaboratedDetailsLocal(string hairProfileId);

		RecommendedRegimensModel RecommendedRegimens(int regimenId);
        RecommendedProductModel RecommendedProducts(int productId);
        RecommendedProductModel2 RecommendedProducts2(int productId);
        HairProfile SaveProfile(HairProfile regimensModel);
        HairProfileAdminModel GetHairProfileAdmin(HairProfileAdminModel hairProfileModel);
        QuestionaireSelectedAnswer GetQuestionaireAnswer(QuestionaireSelectedAnswer hairProfileModel);
        Task<HairProfileCustomerModel> GetHairProfileCustomer(HairProfileCustomerModel hairProfileModel);

        QuestionaireModel GetQuestionaireDetails(QuestionaireModel questionaire);
       Task< QuestionaireModel> GetQuestionaireDetailsMobile(string userId);
		List<HairProfileCustomersModel> GetHairProfileCustomerList();
	}
}
