using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
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
    public class IngredientsController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public IngredientsController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }

        public IActionResult Ingredients()
        {
            return View();
        }

        public async Task<IActionResult> GetIngredients([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<IngredientsModel> filterIngredients = await MyavanaAdminApiClientFactory.Instance.GetIngredients();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<IngredientsModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Name);
                            filterIngredients =
                                sortDirection == "asc"
                                    ? filterIngredients.OrderBy(orderingFunctionString)
                                    : filterIngredients.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionString = (c => c.Type);
                            filterIngredients =
                                sortDirection == "asc"
                                    ? filterIngredients.OrderBy(orderingFunctionString)
                                    : filterIngredients.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 2:
                        {
                            orderingFunctionString = (c => c.Image);
                            filterIngredients =
                                sortDirection == "asc"
                                    ? filterIngredients.OrderBy(orderingFunctionString)
                                    : filterIngredients.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 3:
                        {
                            orderingFunctionString = (c => c.Description);
                            filterIngredients =
                                sortDirection == "asc"
                                    ? filterIngredients.OrderBy(orderingFunctionString)
                                    : filterIngredients.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 4:
                        {
                            orderingFunctionString = (c => c.Challenges);
                            filterIngredients =
                                sortDirection == "asc"
                                    ? filterIngredients.OrderBy(orderingFunctionString)
                                    : filterIngredients.OrderByDescending(orderingFunctionString);
                            break;
                        }

                }
            }
            try
            {
                IEnumerable<IngredientsModel> codes = filterIngredients.Select(e => new IngredientsModel
                {
                    IngedientsEntityId = e.IngedientsEntityId,
                    Name = e.Name,
                    Type = e.Type,
                    Image = e.Image,
                    ImageUrl = e.ImageUrl,
                    Description = e.Description,
                    Challenges = e.Challenges,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IActionResult> CreateIngredient(string id)
        {
            if (id != null)
            {
                IngredientsModel ingredientsModel = new IngredientsModel();
                ingredientsModel.IngedientsEntityId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetIngredientById(ingredientsModel);
                ingredientsModel = response.Data;
                ViewBag.UploadedFile = response.Data.Image;
                return View(ingredientsModel);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredient(IngredientEntityModel ingredientsModel, IFormFile File)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);

                    if (File != null)
                    {
                        string fileName = File.FileName;
                        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Ingredients")))
                        {
                            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Ingredients"));
                        }

                        //var dir = Directory.GetCurrentDirectory();
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Ingredients", fileName);


                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await File.CopyToAsync(stream);
                        }

                        byte[] data;
                        using (var br = new BinaryReader(File.OpenReadStream()))
                            data = br.ReadBytes((int)File.OpenReadStream().Length);

                        ByteArrayContent bytes = new ByteArrayContent(data);


                        MultipartFormDataContent multiContent = new MultipartFormDataContent();

                        multiContent.Add(bytes, "file", File.FileName);
                        multiContent.Add(new StringContent(ingredientsModel.IngedientsEntityId.ToString()), "IngedientsEntityId");
                        multiContent.Add(new StringContent(ingredientsModel.Name), "Name");
                        multiContent.Add(new StringContent(ingredientsModel.Type), "Type");
                        multiContent.Add(new StringContent(ingredientsModel.Description), "Description");
                        multiContent.Add(new StringContent(ingredientsModel.Challenges), "Challenges");

                        var result = client.PostAsync("Ingredients/SaveIngredients", multiContent).Result;
                        if ((int)result.StatusCode == 200)
                            return Content("1");
                        else
                            return Content("0");

                        //return StatusCode((int)result.StatusCode); //201 Created the request has been fulfilled, resulting in the creation of a new resource.

                    }
                    else
                    {
                        MultipartFormDataContent multiContent = new MultipartFormDataContent();

                        multiContent.Add(new StringContent(ingredientsModel.IngedientsEntityId.ToString()), "IngedientsEntityId");
                        multiContent.Add(new StringContent(ingredientsModel.Name), "Name");
                        multiContent.Add(new StringContent(ingredientsModel.Type), "Type");
                        multiContent.Add(new StringContent(ingredientsModel.Description), "Description");
                        multiContent.Add(new StringContent(ingredientsModel.Challenges), "Challenges");
                        multiContent.Add(new StringContent(ingredientsModel.Image), "Image");

                        var result = client.PostAsync("Ingredients/SaveIngredients", multiContent).Result;
                        if ((int)result.StatusCode == 200)
                            return Content("1");
                        else
                            return Content("0");
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500); // 500 is generic server error
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIngredient(IngredientsModel ingredientsModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteIngredient(ingredientsModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
    }
}