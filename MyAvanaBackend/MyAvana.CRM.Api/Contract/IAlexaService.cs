using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
	public interface IAlexaService
	{
		List<AlexaFAQModel> GeAlexaFAQs(int start, int length);
		AlexaFAQ AddAlexaFAQ(AlexaFAQ alexaFAQ);
		AlexaFAQ GetAlexaFAQById(AlexaFAQ alexaFAQ);
		bool DeleteAlexaFAQ(AlexaFAQ alexaFAQ);
		FAQFullDetailsModel GetFAQFullDetails(string keywords, string category);
		FAQShortResponseModel GetFAQShortResponse(string keywords, string category);
		AlexaSalonModel GetSalonResponse(string zipcode);

	}
}
