using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class RegimensController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public RegimensController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }

        public IActionResult Regimens()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetRegimensList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<RegimensModel> filteredCodes = await MyavanaAdminApiClientFactory.Instance.GetRegimensList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
            }
            try
            {
                IEnumerable<RegimensModel> codes = filteredCodes.Select(e => new RegimensModel
                {
                    RegimensId = e.RegimensId,
                    Name = e.Name,
                    RegimenStepsId = e.RegimenStepsId,
                    Step1Instruction = e.Step1Instruction,
                    Step1PhotoName = e.Step1PhotoName,
                    Step2Instruction = e.Step2Instruction,
                    Step2PhotoName = e.Step2PhotoName,
                    Step3Instruction = e.Step3Instruction,
                    Step3PhotoName = e.Step3PhotoName,
                    Step4Instruction = e.Step4Instruction,
                    Step4PhotoName = e.Step4PhotoName,
                    Step5Instruction = e.Step5Instruction,
                    Step5PhotoName = e.Step5PhotoName,
                    Step6Instruction = e.Step6Instruction,
                    Step6PhotoName = e.Step6PhotoName,
                    Step7Instruction = e.Step7Instruction,
                    Step7PhotoName = e.Step7PhotoName,
                    Step8Instruction = e.Step8Instruction,
                    Step8PhotoName = e.Step8PhotoName,
                    Step9Instruction = e.Step9Instruction,
                    Step9PhotoName = e.Step9PhotoName,
                    Step10Instruction = e.Step10Instruction,
                    Step10PhotoName = e.Step10PhotoName,
                    Step11Instruction = e.Step11Instruction,
                    Step11PhotoName = e.Step11PhotoName,
                    Step12Instruction = e.Step12Instruction,
                    Step12PhotoName = e.Step12PhotoName,
                    Step13Instruction = e.Step13Instruction,
                    Step13PhotoName = e.Step13PhotoName,
                    Step14Instruction = e.Step14Instruction,
                    Step14PhotoName = e.Step14PhotoName,
                    Step15Instruction = e.Step15Instruction,
                    Step15PhotoName = e.Step15PhotoName,
                    Step16Instruction = e.Step16Instruction,
                    Step16PhotoName = e.Step16PhotoName,
                    Step17Instruction = e.Step17Instruction,
                    Step17PhotoName = e.Step17PhotoName,
                    Step18Instruction = e.Step18Instruction,
                    Step18PhotoName = e.Step18PhotoName,
                    Step19Instruction = e.Step19Instruction,
                    Step19PhotoName = e.Step19PhotoName,
                    Step20Instruction = e.Step20Instruction,
                    Step20PhotoName = e.Step20PhotoName,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive,
                });
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<ByteArrayContent> AddRegimensImage(IFormFile formFile)
        {
            string fileName = formFile.FileName;
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Regimens")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Regimens"));
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Regimens", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            byte[] dataStep1Photo;
            using (var br = new BinaryReader((formFile).OpenReadStream()))
                dataStep1Photo = br.ReadBytes((int)(formFile).OpenReadStream().Length);
            ByteArrayContent bytesStep1Photo = new ByteArrayContent(dataStep1Photo);
            return bytesStep1Photo;

        }

        [HttpPost]
        public async Task<IActionResult> CreateRegimens(RegimensModel regimensModel)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    PropertyInfo[] regimeProperties = typeof(RegimensModel).GetProperties();

                    foreach (PropertyInfo prop in regimeProperties)
                    {
                        var propertyType = prop.PropertyType.FullName;
                        if (propertyType == "Microsoft.AspNetCore.Http.IFormFile")
                        {
                            IFormFile value = ((Microsoft.AspNetCore.Http.Internal.FormFile)prop.GetValue(regimensModel));

                            if (value != null)
                            {
                                ByteArrayContent response = await AddRegimensImage(value);

                                //multiContent.Add(response, value.Name, value.FileName);
                            }
                            else
                            {
                                multiContent.Add(new StringContent(string.Empty), prop.Name);
                            }

                        }
                        else
                        {
                            if (prop.GetValue(regimensModel) != null)
                            {
                                multiContent.Add(new StringContent(prop.GetValue(regimensModel).ToString()), prop.Name);
                            }
                            else
                            {
                                multiContent.Add(new StringContent(string.Empty), prop.Name);
                            }
                        }
                    }


                    var result = client.PostAsync("Regimens/SaveRegimens", multiContent).Result;

                    if ((int)result.StatusCode == 200)
                        return Content("1");
                    else
                        return Content("0");
                }
                catch (Exception ex)
                {
                    return StatusCode(500);
                }
            }
        }

        public async Task<IActionResult> CreateRegimens(string id)
        {
            if (id != null)
            {
                int count = 0;
                RegimensModel regimensModel = new RegimensModel();
                regimensModel.RegimensId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetRegimensById(regimensModel);
                regimensModel = response.Data;
                PropertyInfo[] regimeProperties = typeof(RegimensModel).GetProperties();

                foreach (PropertyInfo prop in regimeProperties)
                {
                    var propertyType = prop.PropertyType.FullName;
                    if (prop.Name.Contains("PhotoName") && prop.GetValue(regimensModel) != null)
                    {
                        if (prop.GetValue(regimensModel).ToString() != null)
                            count++;
                    }
                }
                ViewBag.Count = count;
                return View(regimensModel);
            }

            ViewBag.Count = 1;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegimens(RegimensModel regimensModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteRegimens(regimensModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
    }
}
