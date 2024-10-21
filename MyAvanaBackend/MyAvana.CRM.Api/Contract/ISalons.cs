using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface ISalons
    {
     
        List<SalonDetails> GetSalons(int start, int length);
        List<SalonsListModel> GetSalonsList();
        SalonModel AddNewSalon(SalonModel salon);
        Salons GetSalonByid(Salons salon);
        Task<(bool success, string error)> UpdateHairProfileSalon(SalonHairProfileModel salonHairProfileModel);
    }
}
