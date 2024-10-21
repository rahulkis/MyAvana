using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.CRM.Api.Services;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AwsS3Controller : ControllerBase
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly IAws3Services _aws3Services;
        //private readonly IAws3Services _aws3Services;

        public AwsS3Controller(IAppConfiguration appConfiguration, IAws3Services aws3Services)
        {
            _aws3Services = aws3Services;
        }

        [HttpPost("UploadDocumentToS3")]
        public IActionResult UploadDocumentToS3([FromForm] ImagesModel imagesModel)
        {
            try
            {
                if (imagesModel.Files is null || imagesModel.Files.Count <= 0)
                return BadRequest(new JsonResult("file is required to upload") { StatusCode = (int)HttpStatusCode.BadRequest });

               //var _aws3Services = new Aws3Services(_appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.Region, _appConfiguration.BucketName);

                var result = _aws3Services.UploadFileAsync(imagesModel.Files, 0);

                return Ok(string.Empty);
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError });
            }
        }

        [HttpGet("DeleteImageS3")]
        public IActionResult DeletetDocumentFromS3(string documentName)
        {
            try
            {
                if (string.IsNullOrEmpty(documentName))
                    return BadRequest(new JsonResult("file is required to upload") { StatusCode = (int)HttpStatusCode.BadRequest });

               //var _aws3Services = new Aws3Services(_appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.Region, _appConfiguration.BucketName);


                _aws3Services.DeleteFileAsync(documentName,1);

                return Ok(string.Format("The document '{0}' is deleted successfully", documentName));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError });
            }
        }
    }
}
