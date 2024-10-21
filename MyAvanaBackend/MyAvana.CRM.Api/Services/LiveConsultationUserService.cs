using System;
using System.Collections.Generic;
using System.Linq;
using MyAvana.CRM.Api.Contract;
using System.Threading.Tasks;
using MyAvana.Models.Entities;
using MyAvana.DAL.Auth;

namespace MyAvana.CRM.Api.Services
{
    public class LiveConsultationUserService : ILiveConsultationUserService
    {
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;
        public LiveConsultationUserService(AvanaContext avanaContext, Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _logger = logger;
        }
        public LiveConsultationUserDetails SaveConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            try
            {
                var objConsUser = _context.LiveConsultationUserDetails;
                //objConsUser. = LiveConsultationUserDetails.;
                //objConsUser. = LiveConsultationUserDetails.;
                //objConsUser. = LiveConsultationUserDetails.;
                //objConsUser. = LiveConsultationUserDetails.;
                return LiveConsultationUserDetails;
            }
            catch(Exception Ex)
            {
                
                _logger.LogError("Method: SaveConsultationDetails, Error: " + Ex.Message, Ex);
                return null;
            }
        }
    }
}
