using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;

namespace MyAvana.CRM.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HairProfileController : ControllerBase
    {
        IHairProfileService hairProfileService;
        private readonly IBaseBusiness _baseBusiness;
        private IHostingEnvironment _env;
        private readonly string webThumbnailImagePath = Path.Combine(Environment.CurrentDirectory, "HairProfileAnalysisThumbnails");
        public HairProfileController(IHairProfileService _hairProfileService, IBaseBusiness baseBusiness, IHostingEnvironment environment)
        {
            hairProfileService = _hairProfileService;
            _baseBusiness = baseBusiness;
            _env = environment;
        }

        [HttpGet("GetHairProfile")]
        public IActionResult GetHairProfile(string token)
        {
            //var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string userId = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            //string decoded = token.Split('.').Take(2).Select(x => Encoding.UTF8.GetString(Convert.FromBase64String(
            //x.PadRight(x.Length + (x.Length % 4), '=')))).Aggregate((s1, s2) => s1 + Environment.NewLine + s2);

            //string str1 = decoded.Substring(decoded.IndexOf("sub") + 6);
            //string userId = str1.Substring(0, str1.IndexOf(",") - 1);

            var result = hairProfileService.GetHairProfile(userId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetHairProfile2")]
        public IActionResult GetHairProfile2(string token, int hairProfileId)
        {
            string userId = "";
            //var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (string.Equals(token, "Null", StringComparison.OrdinalIgnoreCase))
            {
                token = null;
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;

                userId = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            }


            //string decoded = token.Split('.').Take(2).Select(x => Encoding.UTF8.GetString(Convert.FromBase64String(
            //x.PadRight(x.Length + (x.Length % 4), '=')))).Aggregate((s1, s2) => s1 + Environment.NewLine + s2);

            //string str1 = decoded.Substring(decoded.IndexOf("sub") + 6);
            //string userId = str1.Substring(0, str1.IndexOf(",") - 1);

            var result = hairProfileService.GetHairProfile2(userId, hairProfileId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("CollaboratedDetail")]
        public IActionResult CollaboratedDetail(string hairProfileId)
        {

            var result = hairProfileService.CollaboratedDetails(hairProfileId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("CollaboratedDetailLocal")]
        public IActionResult CollaboratedDetailLocal(string hairProfileId)
        {

            var result = hairProfileService.CollaboratedDetailsLocal(hairProfileId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("RecommendedRegimens")]
        public IActionResult RecommendedRegimens(int regimenId)
        {

            var result = hairProfileService.RecommendedRegimens(regimenId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("No data to display") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("RecommendedProducts")]
        public IActionResult RecommendedProducts(int productId)
        {

            var result = hairProfileService.RecommendedProducts(productId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("No data to display") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("RecommendedProducts2")]
        public IActionResult RecommendedProducts2(int productId)
        {

            var result = hairProfileService.RecommendedProducts2(productId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("No data to display") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpPost("SaveProfile")]
        public JObject SaveProfile([FromForm] HairProfile hairProfile)
        {
            HairProfile result = hairProfileService.SaveProfile(hairProfile);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetHairProfileAdmin")]
        public JObject GetHairProfileAdmin(HairProfileAdminModel hairProfileModel)
        {
            var result = hairProfileService.GetHairProfileAdmin(hairProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetQuestionaireAnswer")]
        public JObject GetQuestionaireAnswer(QuestionaireSelectedAnswer hairProfileModel)
        {
            var result = hairProfileService.GetQuestionaireAnswer(hairProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetHairProfileCustomer")]
        public async Task<JObject> GetHairProfileCustomer(HairProfileCustomerModel hairProfileModel)
        {
            var result = await hairProfileService.GetHairProfileCustomer(hairProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetHairProfileCustomerAlexa")]
        public async Task<JObject> GetHairProfileCustomerAlexa(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string userId = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            var result = await hairProfileService.GetHairProfileCustomerAlexa(userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("GetQuestionaireDetails")]
        public JObject GetQuestionaireDetails(QuestionaireModel questionaire)
        {
            QuestionaireModel result = hairProfileService.GetQuestionaireDetails(questionaire);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", questionaire);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost]
        [Route("GetQuestionaireDetailsMobile")]
        public async Task<JObject> GetQuestionaireDetailsMobile()
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string userId = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            QuestionaireModel result = await hairProfileService.GetQuestionaireDetailsMobile(userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet]
        [Route("GetHairProfileCustomerList")]
        public JObject GetHairProfileCustomerList(int userId)
        {
            List<HairProfileCustomersModel> result = hairProfileService.GetHairProfileCustomerList(userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("CreateHHCPByDigitalAssessment")]
        public JObject CreateHHCPByDigitalAssessment(DigitalAssessmentModel digitalAssessmentModel)
        {
            DigitalAssessmentModel result = hairProfileService.CreateHHCPByDigitalAssessment(digitalAssessmentModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", digitalAssessmentModel);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost("CreateHHCPByDigitalAssessmentForMobile")]
        public JObject CreateHHCPByDigitalAssessmentForMobile(DigitalAssessmentModel digitalAssessmentModel)
        {
            DigitalAssessmentModel result = hairProfileService.CreateHHCPByDigitalAssessmentForMobile(digitalAssessmentModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", digitalAssessmentModel);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost("CreateHHCPHairKitUser")]
        public JObject CreateHHCPHairKitUser(DigitalAssessmentModel digitalAssessmentModel)
        {
            DigitalAssessmentModel result = hairProfileService.CreateHHCPHairKitUser(digitalAssessmentModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", digitalAssessmentModel);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetHHCPList")]
        public IActionResult GetHHCPList(string userId, bool? isRequestedFromCustomer)
        {
            List<HairProfileSelectModel> result = hairProfileService.GetHHCPList(userId, isRequestedFromCustomer);
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetMessageTempleteList")]
        public IActionResult GetMessageTempleteList()
        {
            List<MessageTemplateModel> result = hairProfileService.GetMessageTempleteList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveSalonNotes")]
        public JObject SaveSalonNotes(SalonNotesModel salonNotesModel)
        {
            SalonNotesModel result = hairProfileService.SaveSalonNotes(salonNotesModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        public void GenerateThumbnailAndSave(Image originalImage, string outputPath)
        {
            try
            {
                double scale = 0.3;
                int newWidth = (int)(originalImage.Width * scale);
                int newHeight = (int)(originalImage.Height * scale);
                using (Bitmap thumbnail = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(thumbnail))
                    {
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }
                    thumbnail.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            } 
            catch(Exception ex)
            {

            }
        }

        [HttpPost("UploadHairAnalysisImages")]
        public async Task<JObject> UploadHairAnalysisImages([FromForm] TLImagesModel imagesModel)
        {
            HairStrandsImagesModel hairProfile = new HairStrandsImagesModel();
            hairProfile.HairProfileId = imagesModel.HairProfileId;
            hairProfile.HairAnalysisImageType = imagesModel.HairAnalysisImageType;
            hairProfile.UserId = imagesModel.UserId;
            hairProfile.SalonId = imagesModel.SalonId;
            try
            {
                //if (Request.HasFormContentType)
                //{
                if (imagesModel.Files.Count > 0)
                {
                    List<FileModel> fileModelListing = new List<FileModel>();
                    const string UPLOAD_FOLDER = "HairProfileAnalysis";
                    foreach (var imageFile in imagesModel.Files)
                    {
                        fileModelListing.Add(new FileModel { FileName = imageFile.Name.Replace(" ", ""), FormFile = imageFile });
                    }
                    var path = string.Empty;
                    if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
                    {
                        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
                    }
                    //if folder not exist
                    if (!Directory.Exists(webThumbnailImagePath))
                    {
                        Directory.CreateDirectory(webThumbnailImagePath);
                    }
                    foreach (var fileDataModel in fileModelListing)
                    {
                        string fileName = fileDataModel.FormFile.FileName.Replace(" ", "");
                        string uniqueFileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + fileDataModel.FormFile.FileName.Replace(" ", "");
                        path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, uniqueFileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await fileDataModel.FormFile.CopyToAsync(stream);
                            if (string.IsNullOrEmpty(hairProfile.HairStrand))
                            {

                                hairProfile.HairStrand = uniqueFileName;
                            }
                            else
                            {

                                hairProfile.HairStrand += "," + uniqueFileName;
                            }

                        }
                        var thumbnailPath = Path.Combine(webThumbnailImagePath, uniqueFileName);
                        using (var originalImage = Image.FromFile(path))
                        {
                            GenerateThumbnailAndSave(originalImage, thumbnailPath);
                        }
                    }
                    HairStrandsImagesModel result = hairProfileService.UploadHairAnalysisImages(hairProfile);
                    if (result != null)
                        return _baseBusiness.AddDataOnJson("Success", "1", result);
                    else
                        return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

                }
                else
                {
                    return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
                }

                //}
                //else
                //{
                //    return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
                //}

            }
            catch (Exception ex)
            {
                return _baseBusiness.AddDataOnJson(ex.Message, "0", string.Empty);
            }

        }

        [HttpGet("GetHairAnalysisImage")]
        public IActionResult GetHairAnalysisImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                return BadRequest("Image name cannot be empty.");
            }
            var imagePath = Path.Combine(_env.ContentRootPath, "HairProfileAnalysis", imageName);
            if(System.IO.File.Exists(Path.Combine(_env.ContentRootPath, "HairProfileAnalysisThumbnails", imageName)))
            {
                imagePath = Path.Combine(_env.ContentRootPath, "HairProfileAnalysisThumbnails", imageName);
            }
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            return PhysicalFile(imagePath, "image/jpeg");
        }

        [HttpGet("GetHairAnalysisImageFull")]
        public IActionResult GetHairAnalysisImageFull(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                return BadRequest("Image name cannot be empty.");
            }
            var imagePath = Path.Combine(_env.ContentRootPath, "HairProfileAnalysis", imageName);
            
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            return PhysicalFile(imagePath, "image/jpeg");
        }

        [HttpPost]
        [Route("DeleteHairStrandImage")]
        public JObject DeleteHairStrandImage(HairStrandImageInfo strandImageInfo)
        {
            bool result = hairProfileService.DeleteHairStrandImage(strandImageInfo);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", strandImageInfo);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost("EnableDisableProfileView")]
        public JObject EnableDisableProfileView(EnableDisableProfileModel enableDisableProfileModel)
        {
            EnableDisableProfileModel result = hairProfileService.EnableDisableProfileView(enableDisableProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }


        [HttpPost("GetHairProfileCustomerTab2")]
        public async Task<JObject> GetHairProfileCustomerTab2(HairProfileCustomerModel hairProfileModel)
        {
            var result = await hairProfileService.GetHairProfileCustomerTab2(hairProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetHairProfileCustomerExceptTab2")]
        public async Task<JObject> GetHairProfileCustomerExceptTab2(HairProfileCustomerModel hairProfileModel)
        {
            var result = await hairProfileService.GetHairProfileCustomerExceptTab2(hairProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetHairStrandUploadNotificationList")]
        public IActionResult GetHairStrandUploadNotificationList()
        {
            List<HairStrandUploadNotificationModel> result = hairProfileService.GetHairStrandUploadNotificationList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost]
        [Route("UpdateNotificationAsRead")]
        public JObject UpdateNotificationAsRead(HairStrandUploadNotificationModel notification)
        {
            bool result = hairProfileService.UpdateNotificationAsRead(notification);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", notification);
            else
                return _baseBusiness.AddDataOnJson("Something went wrong!", "0", string.Empty);
        }
        [HttpPost("SaveCustomerAIResultForMobile")]
        public JObject SaveCustomerAIResultForMobile(DigitalAssessmentModel digitalAssessmentModel)
        {
            DigitalAssessmentModel result = hairProfileService.SaveCustomerAIResultForMobile(digitalAssessmentModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", digitalAssessmentModel);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetLatestCustomerAIResult")]
        public JObject GetLatestCustomerAIResult(Guid userId)
        {
            var result = hairProfileService.GetLatestCustomerAIResult(userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetLatestCustomerAIResultAdmin")]
        public JObject GetLatestCustomerAIResultAdmin(string userId)
        {
            Guid uId = new Guid(userId);
            var result = hairProfileService.GetLatestCustomerAIResultAdmin(uId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetHairDiarySubmitNotificationList")]
        public IActionResult GetHairDiarySubmitNotificationList()
        {
            List<DailyRoutineTrackerNotificationModel> result = hairProfileService.GetHairDiarySubmitNotificationList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [Route("UpdateNotificationHairDiaryAsRead")]
        public JObject UpdateNotificationHairDiaryAsRead(DailyRoutineTrackerNotificationModel notification)
        {
            bool result = hairProfileService.UpdateNotificationHairDiaryAsRead(notification);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", notification);
            else
                return _baseBusiness.AddDataOnJson("Something went wrong!", "0", string.Empty);
        }

        [HttpPost("SaveHairStylistNotes")]
        public JObject SaveHairStylistNotes(StylistNotesHHCPModel HairStylistNotes)
        {
            StylistNotesHHCPModel result = hairProfileService.SaveHairStylistNotes(HairStylistNotes);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("CreateHHCPUsingScalpAnalysis")]
        public JObject CreateHHCPUsingScalpAnalysis(DigitalAssessmentModelParameters digitalAssessmentModelParam)
        {
            DigitalAssessmentModelParameters result = hairProfileService.CreateHHCPUsingScalpAnalysis(digitalAssessmentModelParam);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost("IsQuestionaireExist")]
        public JObject IsQuestionaireExist(QuestionaireModelParameters QuestionaireModelParam)
        {
            QuestionaireModelParameters result = hairProfileService.IsQuestionaireExist(QuestionaireModelParam);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost("AutoGenerateHHCP")]
        public JObject AutoGenerateHHCP(Models.ViewModels.HairProfile hairProfile)
        {
            Models.Entities.HairProfile result = hairProfileService.AutoGenerateHHCP(hairProfile);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpPost("ShareEmailExist")]
        public JObject ShareEmailExist(string email, int hairProfileId, Guid sharedBy)
        {
            var result = hairProfileService.ShareEmailExist(email, hairProfileId, sharedBy);
            if (result == "1")
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else if (result == "This entered user does not exist")
                return _baseBusiness.AddDataOnJson("This entered user does not exist", "2", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetSharedHHCPList")]
        public IActionResult GetSharedHHCPList(Guid userId)
        {
            List<SharedHHCPModel> result = hairProfileService.GetSharedHHCPList(userId);
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost]
        [Route("RevokeAccess")]
        public JObject RevokeAccess(SharedHHCPModel sharedHHCP)
        {
            bool result = hairProfileService.RevokeAccess(sharedHHCP);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", sharedHHCP);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpGet("GetSharedWithMeHHCPList")]
        public IActionResult GetSharedWithMeHHCPList(Guid userId)
        {
            List<SharedHHCPModel> result = hairProfileService.GetSharedWithMeHHCPList(userId);
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost]
        [Route("GenerateThumbnails")]
        public async Task<IActionResult> GenerateThumbnails()
        {
            string rootPath = _env.ContentRootPath;


            string folderPath = Path.Combine(rootPath, "HairProfileAnalysis");
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                return BadRequest("Invalid folder path.");
            }
            string thumbnailDir = Path.Combine(rootPath, "HairProfileAnalysisThumbnails");

            // Create the thumbnail directory if it doesn't exist
            if (!Directory.Exists(thumbnailDir))
            {
                Directory.CreateDirectory(thumbnailDir);
            }

            // Process all images in the selected folder
            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                if (filePath.EndsWith(".jpg", System.StringComparison.OrdinalIgnoreCase) ||
                    filePath.EndsWith(".jpeg", System.StringComparison.OrdinalIgnoreCase) ||
                    filePath.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase) ||
                    filePath.EndsWith(".bmp", System.StringComparison.OrdinalIgnoreCase) ||
                    filePath.EndsWith(".gif", System.StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {   // Check if the file size is 0 KB
                        FileInfo fileInfo = new FileInfo(filePath);
                        if (fileInfo.Length == 0)
                        {
                            Console.WriteLine($"Skipping empty file: {filePath}");
                            continue; // Skip processing this file
                        }
                        // Generate the path for the thumbnail
                        string thumbnailPath = Path.Combine(thumbnailDir, Path.GetFileName(filePath));

                        if (System.IO.File.Exists(thumbnailPath))
                        {
                            Console.WriteLine($"Skipping file (thumbnail already exists): {filePath}");
                            continue; // Skip processing this file
                        }
                        // Load the original image
                        using (var originalImage = Image.FromFile(filePath))
                        {
                            string outputPath = Path.Combine(thumbnailDir, Path.GetFileName(filePath));
                            //GenerateThumbnailAndSave(originalImage, outputPath);

                            await Task.Run(() => GenerateThumbnailAndSave(originalImage, outputPath));
                        }
                    }
                    catch (OutOfMemoryException ex)
                    {
                        Console.WriteLine($"Skipping file due to out of memory error: {filePath}");
                        continue; // Skip this file and move to the next one
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
                        return StatusCode(500, $"Error processing file {filePath}: {ex.Message}");
                    }


                }
            }

            return Ok("Thumbnails generated successfully.");
        }

    }
}
