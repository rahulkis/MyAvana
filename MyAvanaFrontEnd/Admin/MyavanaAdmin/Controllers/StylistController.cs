using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class StylistController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public StylistController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
		}

        public IActionResult Stylist()
        {
            return View();
        }

        public async Task<IActionResult> AddStylist(string id)
        {
            StylistModel model = new StylistModel();
            if (id != null)
            {
                model.StylistId = Convert.ToInt32(id);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateStylist(StylistModel stylistModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.AddUpdateStylist(stylistModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> GetStylishAdmin(StylishAdminModel stylishAdminModel)
        {
            if (stylishAdminModel != null)
            {
                var response = await MyavanaAdminApiClientFactory.Instance.GetStylishAdmin(stylishAdminModel);
                if (response != null)
                {
                    return Json(response.Data);
                }
                return Content("0");
            }
            return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStylist(StylistModel stylistModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteStylist(stylistModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

		[HttpPost]
		public async Task<IActionResult> UploadExcelsheet(StylistData fileModel)
		{

			string fileName = fileModel.file.FileName;
			FileStream stream = null;
			if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "StylistDocs")))
			{
				Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "StylistDocs"));
			}
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "StylistDocs", fileName);
			using (stream = new FileStream(path, FileMode.Create))
			{
				await fileModel.file.CopyToAsync(stream);
				
			}

			var styleModel = new List<StylistListModel>();
			try
			{
				System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

				using (var mStream = System.IO.File.Open("wwwroot/StylistDocs/" + fileName, FileMode.Open, FileAccess.Read))
				{
					using (var reader = ExcelReaderFactory.CreateReader(mStream))
					{
						var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
						{
							ConfigureDataTable = _ => new ExcelDataTableConfiguration
							{
								UseHeaderRow = true // To set First Row As Column Names  
							}
						});

						if (dataSet.Tables.Count > 0)
						{
							var dataTable = dataSet.Tables[0];
							int i = 2;
							foreach (DataRow objDataRow in dataTable.Rows)
							{
								if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;

								StylistListModel stylistListModel = new StylistListModel();

								if (objDataRow["StylistName"] == null || objDataRow["StylistName"] is System.DBNull)
									return Content("StylistName is Empty at Row no : " + i);
								else
									stylistListModel.StylistName = objDataRow["StylistName"].ToString();

								if (objDataRow["SalonName"] == null || objDataRow["SalonName"] is System.DBNull)
									return Content("SalonName is Empty at Row no : " + i);
								else
									stylistListModel.SalonName = objDataRow["SalonName"].ToString();

								if (objDataRow["City"] == null || objDataRow["City"] is System.DBNull)
									return Content("City is Empty at Row no : " + i);
								else
									stylistListModel.City = objDataRow["City"].ToString();

								if (objDataRow["State"] == null || objDataRow["State"] is System.DBNull)
									return Content("State is Empty at Row no : " + i);
								else
									stylistListModel.State = objDataRow["State"].ToString();

								stylistListModel.ZipCode = objDataRow["ZipCode"] is System.DBNull ? null : objDataRow["ZipCode"].ToString();
								
								stylistListModel.Website = objDataRow["Website"] is System.DBNull ? null : objDataRow["Website"].ToString();

								if (objDataRow["Email"] == null || objDataRow["Email"] is System.DBNull)
									return Content("Email is Empty at Row no : " + i);
								else
								{
									bool isValid = IsValidEmail(objDataRow["Email"].ToString());
									if(isValid)
										stylistListModel.Email = objDataRow["Email"].ToString();
									else
										return Content("Invalid Email at Row no : " + i);
								}

								if (objDataRow["PhoneNumber"] == null || objDataRow["PhoneNumber"] is System.DBNull)
									return Content("PhoneNumber at Empty in Row no : " + i);
								else
									stylistListModel.PhoneNumber = objDataRow["PhoneNumber"].ToString();
									

								if (objDataRow["Address"] == null || objDataRow["Address"] is System.DBNull)
									return Content("Address is Empty at Row no : " + i);
								else
									stylistListModel.Address = objDataRow["Address"].ToString();
									

								stylistListModel.Instagram = objDataRow["Instagram"] is System.DBNull  ? null : objDataRow["Instagram"].ToString();
								stylistListModel.Facebook = objDataRow["Facebook"] is System.DBNull ? null : objDataRow["Facebook"].ToString();


								if (objDataRow["Background"] == null || objDataRow["Background"] is System.DBNull)
									return Content("Background is Empty at Row no : " + i);
								else
									stylistListModel.Background = objDataRow["Background"].ToString();


								if (objDataRow["Notes"] == null || objDataRow["Notes"] is System.DBNull)
									return Content("Notes is Empty at Row no : " + i);
								else
									stylistListModel.Notes = objDataRow["Notes"].ToString();


								List<StylistSpecialtyModel> stylistSpecialtyModels = new List<StylistSpecialtyModel>();

								if (objDataRow["StylistSpecialty"] == null || objDataRow["StylistSpecialty"] is System.DBNull)
									return Content("StylistSpecialty is Empty at Row no : " + i);
								else
								{
									List<string> StylistSpecialty = objDataRow["StylistSpecialty"].ToString().Split(',').ToList();
									try
									{
										foreach (var Specialty in StylistSpecialty)
										{
											StylistSpecialtyModel stylistSpecialtyModel = new StylistSpecialtyModel() { Description = Specialty };
											stylistSpecialtyModels.Add(stylistSpecialtyModel);
										}
										stylistListModel.stylistSpecialties = stylistSpecialtyModels;

									}
									catch (Exception ex) {
										return Content("Please seperate Speciality with ',' at Row no : " + i);
									}
								}
									

								styleModel.Add(stylistListModel);
								i++;
							}
							
						}

					}
				}
			}
			catch (Exception ex)
			{
				return Content(ex.Message);
			}


			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
				
				var response = await MyavanaAdminApiClientFactory.Instance.AddStylistList(styleModel);
				if (response != null)
					return Content("1");
			}
			return Content("0"); 

		}

		bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

	}



	public class StylistData
	{
		public IFormFile file { get; set; }
	}
}
