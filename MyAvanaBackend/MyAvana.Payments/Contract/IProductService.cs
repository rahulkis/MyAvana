using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Contract
{
    public interface IProductService
    {
        (JsonResult result, bool success, string error) GetSuggestions(string hairType, string productType, string hairChallenge, int pageNumber, string userId);
        (JsonResult result, bool success, string error) GetProductDetails(string id);
        (JsonResult result, bool success, string error) GetAllHairTypes();
        (JsonResult result, bool success, string error) GetAllProductTypes(string hairType);
        (JsonResult result, bool success, string error) GetProductsByTypes(string productType);
        (JsonResult result, bool success, string error) GetSuggestionsSP(string hairType, string productType, string hairChallenge, int pageNumber, string userId);
    }
}
