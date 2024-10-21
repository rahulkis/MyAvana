using Flurl;
using Microsoft.EntityFrameworkCore;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class AlexaService : IAlexaService
    {
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;
        public AlexaService(AvanaContext avanaContext, Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _logger = logger;
        }
        public List<AlexaFAQModel> GeAlexaFAQs(int start, int length)
        {
            try
            {
                List<AlexaFAQModel> alexaFAQList = _context.AlexaFAQs.Where(x => x.IsDeleted == false).Skip(start).Take(length).Select(x => new AlexaFAQModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    ShortResponse = x.ShortResponse,
                    DetailedResponse = x.DetailedResponse,
                    Keywords = x.Keywords,
                    Category = x.Category,
                    TotalRecords = _context.AlexaFAQs.Where(p => p.IsDeleted == false).Count()
                }).ToList();

                return alexaFAQList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GeAlexaFAQs, Error: " + ex.Message, ex);
                return null;
            }
        }

        public FAQFullDetailsModel GetFAQFullDetails(string keywords, string category)
        {
            try
            {
                List<String> keywordsList = keywords.Split(',').ToList();
                FAQFullDetailsModel alexaFAQModel = _context.AlexaFAQs.Where(x => x.Category == category && keywordsList.Any(t => x.Keywords.Contains(t))).Select(x => new FAQFullDetailsModel
                {
                    DetailedResponse = x.DetailedResponse,
                    ShortResponse = x.ShortResponse,
                    Link = ""
                }).FirstOrDefault();
                return alexaFAQModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetFAQFullDetails, Error: " + ex.Message, ex);
                return null;
            }
        }

        public FAQShortResponseModel GetFAQShortResponse(string keywords, string category)
        {
            try
            {
                List<String> keywordsList = keywords.Split(',').ToList();
                FAQShortResponseModel alexaFAQModel = _context.AlexaFAQs.Where(x => x.Category == category && keywordsList.Any(t => x.Keywords.Contains(t))).Select(x => new FAQShortResponseModel
                {
                    ShortResponse = x.ShortResponse,
                    Link = ""
                }).FirstOrDefault();
                return alexaFAQModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetFAQShortResponse, Error: " + ex.Message, ex);
                return null;
            }
        }

        public AlexaFAQ AddAlexaFAQ(AlexaFAQ alexaFAQ)
        {
            try
            {
                if (alexaFAQ.Id != 0)
                {
                    var objAlexaFAQ = _context.AlexaFAQs.Where(x => x.Id == alexaFAQ.Id).FirstOrDefault();
                    objAlexaFAQ.Description = alexaFAQ.Description;
                    objAlexaFAQ.ShortResponse = alexaFAQ.ShortResponse;
                    objAlexaFAQ.DetailedResponse = alexaFAQ.DetailedResponse;
                    objAlexaFAQ.Category = alexaFAQ.Category;
                    objAlexaFAQ.Keywords = alexaFAQ.Keywords;
                    objAlexaFAQ.IsDeleted = false;
                }
                else
                {
                    _context.AlexaFAQs.Add(new AlexaFAQ()
                    {
                        Description = alexaFAQ.Description,
                        ShortResponse = alexaFAQ.ShortResponse,
                        DetailedResponse = alexaFAQ.DetailedResponse,
                        Keywords = alexaFAQ.Keywords,
                        Category = alexaFAQ.Category,
                        IsDeleted = false,

                    });
                }
                _context.SaveChanges();
                return alexaFAQ;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: AddAlexaFAQ, Error: " + ex.Message, ex);
                return null;
            }
        }

        public AlexaFAQ GetAlexaFAQById(AlexaFAQ alexaFAQ)
        {
            try
            {
                AlexaFAQ alexaFAQModel = _context.AlexaFAQs.Where(x => x.Id == alexaFAQ.Id).FirstOrDefault();
                return alexaFAQModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetAlexaFAQById, AlexaFAQId:"+ alexaFAQ.Id + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteAlexaFAQ(AlexaFAQ alexaFAQ)
        {
            try
            {
                var alexaFAQModel = _context.AlexaFAQs.FirstOrDefault(x => x.Id == alexaFAQ.Id);
                {
                    if (alexaFAQModel != null)
                    {
                        alexaFAQModel.IsDeleted = true;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteAlexaFAQ, AlexaFAQId:" + alexaFAQ.Id + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public AlexaSalonModel GetSalonResponse(string zipcode)
        {
            try
            {
                zipcode = zipcode.ToLower();
                AlexaSalonModel alexaSalonModel = _context.Salons.Where(x => x.Address.Contains(zipcode)).Select(x => new AlexaSalonModel
                {
                    Id = x.SalonId.ToString(),
                    Name = x.SalonName,
                    Address = x.Address,
                    Email = x.EmailAddress,
                    Phonenumber = x.PhoneNumber
                }).FirstOrDefault();

                return alexaSalonModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetSalonResponse, Zipcode:" + zipcode + ", Error: " + ex.Message, ex);
                return null;
            }
        }
    }
}
