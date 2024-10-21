﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class StylistService : IStylistService
    {
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;
        public StylistService(AvanaContext avanaContext, Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _logger = logger;
        }

        public bool DeleteStylist(StylistModel stylist)
        {
            try
            {
                var objStylist = _context.Stylists.FirstOrDefault(x => x.StylistId == stylist.StylistId);
                {
                    if (objStylist != null)
                    {
                        objStylist.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteStylist, StylistId:" + stylist.StylistId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        //public List<StylistModel> GetStylists()
        //{
        //    try
        //    {
        //        List<StylistModel> lstStylistModel = _context.Stylists.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
        //            Select(x => new StylistModel
        //            {
        //                StylistId = x.StylistId,
        //                StylistName = x.StylistName,
        //                SalonName = x.SalonName,
        //                City = x.City,
        //                State = x.State,
        //                ZipCode = x.ZipCode,
        //                Website = x.Website,
        //                Email = x.Email,
        //                PhoneNumber = x.PhoneNumber,
        //                Address = x.Address,
        //                Instagram = x.Instagram,
        //                Facebook = x.Facebook,
        //                Background = x.Background,
        //                Notes = x.Notes,
        //                IsActive = x.IsActive,
        //                CreatedOn = x.CreatedOn,
        //                //StylistSpecialtyId = x.StylistSpecialtyId,
        //                //Description = x.StylistSpecialty.Description
        //            }).ToList();

        //        return lstStylistModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public StylistModel AddUpdateStylist(StylistModel stylist)
        {
            try
            {
                List<StylistSpecialty> listSpecialty = JsonConvert.DeserializeObject<List<StylistSpecialty>>(stylist.StylistSpecialty);
                if (stylist.StylistId != 0)
                {
                    var objStylish = _context.Stylists.Where(x => x.StylistId == stylist.StylistId && x.IsActive == true).FirstOrDefault();
                    if (objStylish != null)
                    {
                        objStylish.StylistName = stylist.StylistName;
                        objStylish.SalonName = stylist.SalonName;
                        objStylish.City = stylist.City;
                        objStylish.State = stylist.State;
                        objStylish.ZipCode = stylist.ZipCode;
                        objStylish.Website = stylist.Website;
                        objStylish.Email = stylist.Email;
                        objStylish.PhoneNumber = stylist.PhoneNumber;
                        objStylish.Address = stylist.Address;
                        objStylish.Instagram = stylist.Instagram;
                        objStylish.Facebook = stylist.Facebook;
                        objStylish.Background = stylist.Background;
                        objStylish.Notes = stylist.Notes;

                        _context.SaveChanges();
                    }

                    var newSpecialtyList = listSpecialty.Select(x => x.StylistSpecialtyId).ToArray();
                    var stylishCommonList = _context.StylishCommons.Where(x => x.StylistId == stylist.StylistId && x.IsActive == true).ToList();
                    var existSpecialtyList = stylishCommonList.Select(x => x.StylistSpecialtyId).ToArray();
                    var commonSpecilty = newSpecialtyList.Intersect(existSpecialtyList);


                    // Remove items
                    existSpecialtyList = existSpecialtyList.Except(commonSpecilty).ToArray();
                    var removeItems = stylishCommonList.Where(x => existSpecialtyList.Contains(x.StylistSpecialtyId)).ToList();
                    removeItems.ForEach(x => x.IsActive = false);
                    _context.StylishCommons.UpdateRange(removeItems);
                    _context.SaveChanges();

                    // Add new Items
                    newSpecialtyList = newSpecialtyList.Except(commonSpecilty).ToArray();

                    foreach (var spec in newSpecialtyList)
                    {
                        StylishCommon objcommon = new StylishCommon();
                        objcommon.StylistId = objStylish.StylistId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.StylistSpecialtyId = spec;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }

                    // Add Salon for Stylist
                    var ExistSalonStylist = _context.SalonsStylists.FirstOrDefault(x => x.StylistId == objStylish.StylistId && x.IsActive == true);
                    if (stylist.SalonId != null && stylist.SalonId != 0)
                    {
                        if (ExistSalonStylist == null)
                        {
                            SalonsStylist obj = new SalonsStylist
                            {
                                SalonId = stylist.SalonId,
                                StylistId = objStylish.StylistId,
                                IsActive = true
                            };
                            _context.Add(obj);
                            _context.SaveChanges();
                        }
                        else
                        {
                            if (ExistSalonStylist.SalonId != stylist.SalonId)
                            {
                                ExistSalonStylist.SalonId = stylist.SalonId;
                                _context.SalonsStylists.Update(ExistSalonStylist);
                                _context.SaveChanges();
                            }
                        }

                    }
                    else
                    {
                        if(ExistSalonStylist != null)
                        {
                            ExistSalonStylist.SalonId = stylist.SalonId;
                            _context.SalonsStylists.Update(ExistSalonStylist);
                            _context.SaveChanges();
                        }
                    }
                }
                else
                {
                    Stylist objStylist = new Stylist();
                    objStylist.StylistName = stylist.StylistName;
                    objStylist.SalonName = stylist.SalonName;
                    objStylist.City = stylist.City;
                    objStylist.State = stylist.State;
                    objStylist.ZipCode = stylist.ZipCode;
                    objStylist.Website = stylist.Website;
                    objStylist.Email = stylist.Email;
                    objStylist.PhoneNumber = stylist.PhoneNumber;
                    objStylist.Address = stylist.Address;
                    objStylist.Instagram = stylist.Instagram;
                    objStylist.Facebook = stylist.Facebook;
                    objStylist.Background = stylist.Background;
                    objStylist.Notes = stylist.Notes;
                    objStylist.IsActive = true;
                    objStylist.CreatedOn = DateTime.Now;

                    _context.Add(objStylist);
                    _context.SaveChanges();

                    foreach (var spec in listSpecialty)
                    {
                        StylishCommon objcommon = new StylishCommon();
                        objcommon.StylistId = objStylist.StylistId;
                        objcommon.IsActive = true;
                        objcommon.CreatedOn = DateTime.Now;
                        objcommon.StylistSpecialtyId = spec.StylistSpecialtyId;

                        _context.Add(objcommon);
                        _context.SaveChanges();
                    }

                    // Add Salon for Stylist
                    if (stylist.SalonId != null && stylist.SalonId != 0)
                    {
                        SalonsStylist obj = new SalonsStylist
                        {
                            SalonId = stylist.SalonId,
                            StylistId = objStylist.StylistId,
                            IsActive = true
                        };
                        _context.Add(obj);
                        _context.SaveChanges();
                    }
                }
                return stylist;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AddUpdateStylist, StylistId:" + stylist.StylistId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public List<StylistSpecialty> GetStylistSpecialty()
        {
            try
            {
                List<StylistSpecialty> stylistSpecialties = _context.StylistSpecialties.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
                return stylistSpecialties;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetStylistSpecialty, Error: " + ex.Message, ex);
                return null;
            }
        }

        public StylishAdminModel GetStylishAdmin(StylishAdminModel stylishAdminModel)
        {
            try
            {

                StylishAdminModel stylish = (from sty in _context.Stylists
                                             where sty.StylistId == stylishAdminModel.StylistId
                                             && sty.IsActive == true
                                             select new StylishAdminModel()
                                             {
                                                 StylistId = sty.StylistId,
                                                 StylistName = sty.StylistName,
                                                 SalonName = sty.SalonName,
                                                 City = sty.City,
                                                 State = sty.State,
                                                 ZipCode = sty.ZipCode,
                                                 Website = sty.Website,
                                                 Email = sty.Email,
                                                 PhoneNumber = sty.PhoneNumber,
                                                 Address = sty.Address,
                                                 Instagram = sty.Instagram,
                                                 Facebook = sty.Facebook,
                                                 Background = sty.Background,
                                                 Notes = sty.Notes,
                                                 IsActive = sty.IsActive,
                                                 CreatedOn = sty.CreatedOn,

                                                 stylishCommons = _context.StylishCommons.Where(x => x.StylistId == sty.StylistId && x.IsActive == true).ToList(),
                                                 SalonId = _context.SalonsStylists.FirstOrDefault(x => x.StylistId == sty.StylistId && x.IsActive == true) != null ? _context.SalonsStylists.FirstOrDefault(x => x.StylistId == sty.StylistId && x.IsActive == true).SalonId : null

                                             }).FirstOrDefault();

                return stylish;
            }
            catch(Exception ex)
            {
                _logger.LogError("Method: GetStylishAdmin, StylistId:" + stylishAdminModel.StylistId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<StylistListModel> GetStylistList()
        {
            List<StylistListModel> stylistListModels = new List<StylistListModel>();
            try
            {
                List<int> stylistIds = _context.StylishCommons.Select(x => x.StylistId).Distinct().ToList();

                foreach (var id in stylistIds)
                {
                    int i = 0;
                    int[] elements = new int[50];

                    List<int> stylistSpecialtyId = _context.StylishCommons.Where(x => x.StylistId == id).Select(x => x.StylistSpecialtyId).Distinct().ToList();

                    StylistListModel stylistListModel = new StylistListModel();

                    var objStylist = _context.Stylists.Where(x => x.StylistId == id && x.IsActive == true).FirstOrDefault();
                    if (objStylist != null)
                    {
                        stylistListModel.StylistId = objStylist.StylistId;
                        stylistListModel.StylistName = objStylist.StylistName;
                        stylistListModel.SalonName = objStylist.SalonName;
                        stylistListModel.City = objStylist.City;
                        stylistListModel.State = objStylist.State;
                        stylistListModel.ZipCode = objStylist.ZipCode;
                        stylistListModel.Website = objStylist.Website;
                        stylistListModel.Email = objStylist.Email;
                        stylistListModel.PhoneNumber = objStylist.PhoneNumber;
                        stylistListModel.Address = objStylist.Address;
                        stylistListModel.Instagram = objStylist.Instagram;
                        stylistListModel.Facebook = objStylist.Facebook;
                        stylistListModel.Background = objStylist.Background;
                        stylistListModel.Notes = objStylist.Notes;

                        var specialty = _context.StylistSpecialties.Where(x => stylistSpecialtyId.Contains(x.StylistSpecialtyId)).ToList();

                        List<StylistSpecialty> stylistSpecialties = new List<StylistSpecialty>();

                        foreach (var spec in specialty)
                        {
                            if (!elements.Contains(spec.StylistSpecialtyId))
                            {
                                elements[i] = spec.StylistSpecialtyId;
                                i++;
                                StylistSpecialty stylistSpecialty = new StylistSpecialty();
                                stylistSpecialty.StylistSpecialtyId = spec.StylistSpecialtyId;
                                stylistSpecialty.Description = spec.Description;
                                stylistSpecialty.IsActive = spec.IsActive;
                                stylistSpecialty.CreatedOn = spec.CreatedOn;

                                stylistSpecialties.Add(stylistSpecialty);
                            }
                        }

                        stylistListModel.stylistSpecialties = stylistSpecialties;

                        stylistListModels.Add(stylistListModel);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetStylistList, Error: " + ex.Message, ex);
            }
            return stylistListModels;
        }

        public IEnumerable<StylistListModel> AddStylistList(IEnumerable<StylistListModel> stylistData)
        {
            try
            {
                if (stylistData != null)
                {
                    foreach (var stylist in stylistData)
                    {
                        Stylist objStylist = new Stylist();
                        objStylist.StylistName = stylist.StylistName;
                        objStylist.SalonName = stylist.SalonName;
                        objStylist.City = stylist.City;
                        objStylist.State = stylist.State;
                        objStylist.ZipCode = stylist.ZipCode;
                        objStylist.Website = stylist.Website;
                        objStylist.Email = stylist.Email;
                        objStylist.PhoneNumber = stylist.PhoneNumber;
                        objStylist.Address = stylist.Address;
                        objStylist.Instagram = stylist.Instagram;
                        objStylist.Facebook = stylist.Facebook;
                        objStylist.Background = stylist.Background;
                        objStylist.Notes = stylist.Notes;
                        objStylist.IsActive = true;
                        objStylist.CreatedOn = DateTime.Now;

                        _context.Add(objStylist);
                        _context.SaveChanges();


                        if (stylist.stylistSpecialties != null)
                        {
                            foreach (var spec in stylist.stylistSpecialties)
                            {
                                StylishCommon objcommon = new StylishCommon();
                                objcommon.StylistId = objStylist.StylistId;
                                objcommon.IsActive = true;
                                objcommon.CreatedOn = DateTime.Now;

                                objcommon.StylistSpecialtyId = _context.StylistSpecialties.Where(x => x.Description.ToLower() == spec.Description.Trim().ToLower()).Select(x => x.StylistSpecialtyId).FirstOrDefault();
                                if (objcommon.StylistSpecialtyId == 0)
                                {
                                    StylistSpecialty style = new StylistSpecialty();
                                    style.Description = spec.Description;
                                    style.IsActive = true;
                                    style.CreatedOn = DateTime.Now;
                                    _context.Add(style);
                                    _context.SaveChanges();
                                    objcommon.StylistSpecialtyId = style.StylistSpecialtyId;
                                }
                                _context.Add(objcommon);
                                _context.SaveChanges();
                            }
                        }
                    }
                }

                //List<StylistListModel> stylistListModels =  GetStylistList();
                return stylistData;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: AddStylistList, Error: " + ex.Message, ex);
                return null;
            }
        }
    }
}
