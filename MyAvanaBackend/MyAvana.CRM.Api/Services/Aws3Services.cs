using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class Aws3Services : IAws3Services
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _awsS3Client;
        private readonly AvanaContext _context;
        private readonly Logger.Contract.ILogger _logger;
        //public Aws3Services(string awsAccessKeyId, string awsSecretAccessKey, string region, string bucketName)
        //{
        //    _bucketName = bucketName;
        //    _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.GetBySystemName(region));
        //}
        private readonly IConfiguration configuration;
        public Aws3Services(IConfiguration configuration, AvanaContext context, Logger.Contract.ILogger logger)
        {
            this.configuration = configuration;
            this._context = context;
            _logger = logger;
            _bucketName =  configuration.GetSection("BucketName").Value;
            _awsS3Client = new AmazonS3Client(configuration.GetSection("AwsAccessKey").Value, configuration.GetSection("AwsSecretAccessKey").Value, RegionEndpoint.GetBySystemName(configuration.GetSection("Region").Value));
        }
        public async Task<bool> UploadFileAsync(List<IFormFile> files,int ProductEntityId)
        {
            try
            {
                int num = 0;
                foreach (var file in files)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssmm") + '_' + num;
                    await UploadFile(file, fileName);
                    num++;
                }
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UploadFileAsync, ProductId:" + ProductEntityId + ", Error: " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> UploadFile(IFormFile file, string fileName)
        {
            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);
                    newMemoryStream.Position = 0;

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = fileName,
                        BucketName = _bucketName,
                        ContentType = file.ContentType
                    };

                    var fileTransferUtility = new TransferUtility(_awsS3Client);

                    await fileTransferUtility.UploadAsync(uploadRequest);
                    newMemoryStream.Dispose();
                    newMemoryStream.Flush();
                    return true;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UploadFile, Error: " + ex.Message, ex);
                return false;
            }
        }

        public bool DeleteFileAsync(string fileName, int id, string versionId = "")
        {
            try
            {
                var res = DeleteFile(fileName, versionId).GetAwaiter().GetResult();
                if (res == true)
                {
                    var productImage = _context.ProductImages.FirstOrDefault(x => x.Id == id);
                    if (productImage != null)
                    {
                        productImage.IsActive = false;
                        _context.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: DeleteFileAsync, Error: " + ex.Message, ex);
                return false;
            }
           
        }

        public async Task<bool> DeleteFile(string fileName, string versionId = "")
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };
                IsFileExists(fileName, null);

                if (!string.IsNullOrEmpty(versionId))
                    request.VersionId = versionId;

                await _awsS3Client.DeleteObjectAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: DeleteFile, Error: " + ex.Message, ex);
                return false;
            }

        }

        public bool IsFileExists(string fileName, string versionId)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };

                var response = _awsS3Client.GetObjectMetadataAsync(request).Result;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: IsFileExists, Error: " + ex.Message, ex);
                if (ex.InnerException != null && ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                        return false;

                    else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                        return false;
                }
                return false;
            }
        }

        #region Upload attachment documents
        public async Task<bool> UploadAttchmentFile(IFormFile file, string fileName)
        {
            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);
                    newMemoryStream.Position = 0;

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = fileName,
                        BucketName = _bucketName+"/attachment",
                        ContentType = file.ContentType
                    };

                    var fileTransferUtility = new TransferUtility(_awsS3Client);

                    await fileTransferUtility.UploadAsync(uploadRequest);
                    newMemoryStream.Dispose();
                    newMemoryStream.Flush();
                    return true;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UploadAttchmentFile, Error: " + ex.Message, ex);
                return false;
            }
        }
        #endregion

        public async Task<bool> UploadSalonLogo(IFormFile file, string fileName)
        {
            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);
                    newMemoryStream.Position = 0;

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = fileName,
                        BucketName = _bucketName + "/salonLogo",
                        ContentType = file.ContentType
                    };

                    var fileTransferUtility = new TransferUtility(_awsS3Client);

                    await fileTransferUtility.UploadAsync(uploadRequest);
                    newMemoryStream.Dispose();
                    newMemoryStream.Flush();
                    return true;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UploadSalonLogo, Error: " + ex.Message, ex);
                return false;
            }
        }
    }
}
