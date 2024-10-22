using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;

namespace MyavanaAdmin.Controllers
{
    public class ToolsController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public ToolsController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }

        public IActionResult Toolslist()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<ToolsModel> filterTools = await MyavanaAdminApiClientFactory.Instance.GetTools();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<ToolsModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.ToolName);
                            filterTools =
                                sortDirection == "asc"
                                    ? filterTools.OrderBy(orderingFunctionString)
                                    : filterTools.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionString = (c => c.ActualName);
                            filterTools =
                                sortDirection == "asc"
                                    ? filterTools.OrderBy(orderingFunctionString)
                                    : filterTools.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 2:
                        {
                            orderingFunctionString = (c => c.BrandName);
                            filterTools =
                                sortDirection == "asc"
                                    ? filterTools.OrderBy(orderingFunctionString)
                                    : filterTools.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 3:
                        {
                            orderingFunctionString = (c => c.Image);
                            filterTools =
                                sortDirection == "asc"
                                    ? filterTools.OrderBy(orderingFunctionString)
                                    : filterTools.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 4:
                        {
                            orderingFunctionString = (c => c.ToolLink);
                            filterTools =
                                sortDirection == "asc"
                                    ? filterTools.OrderBy(orderingFunctionString)
                                    : filterTools.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 5:
                        {
                            orderingFunctionString = (c => c.ToolDetails);
                            filterTools =
                                sortDirection == "asc"
                                    ? filterTools.OrderBy(orderingFunctionString)
                                    : filterTools.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<ToolsModel> codes = filterTools.Select(x => new ToolsModel
                {
                    Id = x.Id,
                    ToolName = x.ToolName,
                    ActualName = x.ActualName,
                    BrandName = x.BrandName,
                    Image = x.Image,
                    ToolLink = x.ToolLink,
                    ToolDetails = x.ToolDetails,
                    CreatedOn = x.CreatedOn,
                    Price = x.Price
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IActionResult> CreateTool(string id)
        {
            if (id != null)
            {
                ToolsModel toolModel = new ToolsModel();
                toolModel.Id = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetToolsById(toolModel);
                toolModel = response.Data;
                return View(toolModel);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTool(ToolsModel toolsModel, IFormFile File)
        {
            if (File != null)
            {
                Random generator = new Random();
                String random = generator.Next(0, 1000000).ToString("D6");
                string imgExt = Path.GetExtension(File.FileName);
                string fileName = File.FileName.Substring(0, File.FileName.IndexOf(".")) + "_" + random + imgExt;

                //string fileName = File.FileName;
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "tools")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "tools"));
                }

                //var dir = Directory.GetCurrentDirectory();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tools", fileName);


                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                    toolsModel.Image = "http://admin.test.com/tools/" + fileName;
                }
            }
            var response = await MyavanaAdminApiClientFactory.Instance.SaveTools(toolsModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTool(ToolsModel toolsModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteTool(toolsModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

    }
}