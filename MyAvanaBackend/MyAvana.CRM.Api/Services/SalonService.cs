using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class SalonService : ISalons
    {
        private readonly AvanaContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IEmailService _emailService;
        private readonly Logger.Contract.ILogger _logger;
        private readonly IConfiguration _configuration;
        public SalonService(AvanaContext avanaContext, UserManager<UserEntity> userManager, IEmailService emailService, Logger.Contract.ILogger logger, IConfiguration configuration)
        {
            _context = avanaContext;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            _configuration = configuration;
            _logger = logger;
        }

        public List<SalonDetails> GetSalons(int start, int length)
        {
            try
            {
                List<SalonDetails> webLogins = _context.Salons.Where(x => x.IsActive == true).Skip(start).Take(length).Select(x => new SalonDetails
                {
                    SalonId = x.SalonId,
                    SalonName = x.SalonName,
                    EmailAddress = x.EmailAddress,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,
                    IsActive = x.IsActive,
                    IsPublicNotes = x.IsPublicNotes,
                    SalonLogo=x.SalonLogo,
                    TotalRecords = _context.Salons.Where(p => p.IsActive == true).Count()

                }).OrderByDescending(x => x.SalonName).ToList();

                return webLogins;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetSalons, Error: " + ex.Message, ex);
                return null;
            }
        }

        public Salons GetSalonByid(Salons salon)
        {
            try
            {
                Salons objSalon = _context.Salons.Where(x => x.SalonId == salon.SalonId).FirstOrDefault();
                return objSalon;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetSalonByid, SalonId:" + salon.SalonId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public SalonModel AddNewSalon(SalonModel salon)
        {
            try
            {
                if (salon.File != null)
                {
                    var _aws3Services = new Aws3Services(_configuration, _context, _logger);
                    string fileName = salon.File.FileName.Substring(0, salon.File.FileName.IndexOf(".")) + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + salon.File.FileName.Substring(salon.File.FileName.IndexOf("."));
                    var result = _aws3Services.UploadSalonLogo(salon.File, fileName).GetAwaiter().GetResult();
                    if (result == true)
                    {
                        salon.SalonLogo = _configuration.GetSection("AWSBucket").Value + "salonLogo/" + fileName;
                    }

                }
                if (salon.SalonId != 0)
                {
                    var objSalon = _context.Salons.Where(x => x.SalonId == salon.SalonId).FirstOrDefault();
                    objSalon.EmailAddress = salon.EmailAddress;
                    objSalon.PhoneNumber = salon.PhoneNumber;
                    objSalon.SalonName = salon.SalonName;
                    objSalon.Address = salon.Address;
                    objSalon.IsActive = salon.IsActive;
                    objSalon.IsPublicNotes = salon.IsPublicNotes;
                    if (salon.File != null)
                    {
                        objSalon.SalonLogo = salon.SalonLogo;
                    }
                }
                else
                {
                    //var password = CreateRandomPassword(9);
                    _context.Salons.Add(new Salons()
                    {
                        EmailAddress = salon.EmailAddress,
                        PhoneNumber = salon.PhoneNumber,
                        SalonName = salon.SalonName,
                        Address = salon.Address,
                        IsActive = true,
                        IsPublicNotes = salon.IsPublicNotes,
                        SalonLogo = salon.SalonLogo
                });
                }
                _context.SaveChanges();
                return salon;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: AddNewSalon, SalonId:" + salon.SalonId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<SalonsListModel> GetSalonsList()
        {
            try
            {
                List<SalonsListModel> salons = _context.Salons.Where(x => x.IsActive == true).Select(x => new SalonsListModel
                {
                    SalonId = x.SalonId,
                    SalonName = x.SalonName

                }).OrderByDescending(x => x.SalonName).ToList();

                return salons;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetSalonsList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public async Task<(bool success, string error)> UpdateHairProfileSalon(SalonHairProfileModel salonHairProfileModel)
        {
            try
            {
                if (salonHairProfileModel.SalonId != 0)
                {
                    var ContainEmail = true;
                    if (!string.IsNullOrWhiteSpace(salonHairProfileModel.userId))
                    {
                        // Regular expression for validating email
                        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                        ContainEmail = Regex.IsMatch(salonHairProfileModel.userId, emailPattern);
                    }
                    if(!ContainEmail)
                    {
                        salonHairProfileModel.userId = _context.Users.Where(a => a.Id.ToString() == salonHairProfileModel.userId).Select(a => a.Email).SingleOrDefault(); ;
                    }
                    var user = await _userManager.FindByEmailAsync(salonHairProfileModel.userId);
                    var AssignUser = _context.Users.Where(a => a.Email == salonHairProfileModel.userId).FirstOrDefault();
                    user.SalonId = salonHairProfileModel.SalonId;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userSalon = _context.Salons.Where(x => x.SalonId == salonHairProfileModel.SalonId).FirstOrDefault();

                        EmailInformation emailInformation = new EmailInformation
                        {
                            Email = userSalon.EmailAddress,
                            Name = AssignUser.FirstName + " " + AssignUser.LastName,
                            GroupName = userSalon.SalonName,
                            userEmail = salonHairProfileModel.userId
                        };
                        _emailService.SendEmail("ASSIGNCUST", emailInformation);
                        return (true, "");
                    }
                    _logger.LogError("Method: UpdateHairProfileSalon, UserId:" + salonHairProfileModel.userId + ", Error: Error in updating the Salon.");
                    return (false, "Error in updating the Salon.");
                }
                return (true, "No need to Update hair Profile option - Select Salon");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UpdateHairProfileSalon, UserId:" + salonHairProfileModel.userId + ", Error: " + ex.Message, ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
    }
}
