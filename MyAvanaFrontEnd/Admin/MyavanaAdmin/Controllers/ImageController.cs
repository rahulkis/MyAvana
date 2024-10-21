using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using Newtonsoft.Json.Linq;

namespace MyavanaAdmin.Controllers
{
    // [Authorize(AuthenticationSchemes = "AdminCookies")]

    public class ImageController : Controller
    {
        private const string UPLOAD_FOLDER = "wwwroot/imageUpload";
        private const string IMAGE_FOLDER = "wwwroot/groupImage";
        private const string AUDIO_FOLDER = "wwwroot/groupAudio";
        private const string VIDEO_FOLDER = "wwwroot/groupVideo";
        private const string THUMBNAIL_FOLDER = "wwwroot/groupThumbnail";
        private const string PRODUCT_FOLDER = "wwwroot/routineProducts";
        private const string INGREDIENT_FOLDER = "wwwroot/routineIngredients";
        private const string REGIMEN_FOLDER = "wwwroot/routineRegimens";
        private const string ROUTINEIMAGE_FOLDER = "wwwroot/routineProfile";
        private readonly IOptions<AppSettingsModel> config;
        public ImageController(IOptions<AppSettingsModel> config)
        {
            ApplicationSettings.WebApiUrl = config.Value.WebApiBaseUrl;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ImageUpload([FromBody] Imagerequest imagerequest)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (imagerequest.fileData.Length == 0)
                return BadRequest();
            var bytes = Convert.FromBase64String(imagerequest.fileData);

            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
            }

            string name = Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ".jpg");
            string imageName = name.Substring(name.LastIndexOf("/") + 1);
            if (bytes.Length > 0)
            {
                using (var stream = new FileStream(name, FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
            }
            fileData file = new fileData();
            file.access_token = token;
            file.ImageURL = imageName;

            var response = await MyavanaAdminApiClientFactory.Instance.ImageUpload(file);
            if (response.message == "Success")
            {
                return Ok(new JsonResult(new Dictionary<string, object>{
                { "access_token" , response.Data.access_token },
                {"user_name",response.Data.user_name },
                {"Email",response.Data.Email },
                {"Name",response.Data.Name },
                {"AccountNo",response.Data.AccountNo },
                {"TwoFactor",response.Data.TwoFactor },
                {"hairType",response.Data.HairType },
                {"imageURL",response.Data.ImageURL } })
                { StatusCode = (int)HttpStatusCode.OK });
            }
            return BadRequest(new JsonResult("Something went wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePost([FromForm] GroupPostModelParam groupPost)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string imgName = null, audioName = null, videoName = null, thumbName = null;

            if (groupPost.Image != null)
            {
                imgName = groupPost.Image.FileName;
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), IMAGE_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, IMAGE_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), IMAGE_FOLDER, imgName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await groupPost.Image.CopyToAsync(stream);
                }

            }
            else if (groupPost.Audio != null)
            {
                audioName = groupPost.Audio.FileName;
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), AUDIO_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, AUDIO_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), AUDIO_FOLDER, audioName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await groupPost.Audio.CopyToAsync(stream);
                }

            }
            else if (groupPost.Video != null)
            {
                videoName = groupPost.Video.FileName;
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), VIDEO_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, VIDEO_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), VIDEO_FOLDER, videoName);
                Random generator = new Random();
                String random = generator.Next(0, 1000000).ToString("D6");
                thumbName = random + ".jpeg";
                var thumbPath = Path.Combine(Directory.GetCurrentDirectory(), THUMBNAIL_FOLDER, thumbName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await groupPost.Video.CopyToAsync(stream);

                }

                //try
                //{
                //    string fileargs = "-i " + path + " -ss 00:00:14.435 -vframes 1 " + thumbPath;
                //    Process process = new Process
                //    {
                //        StartInfo = new ProcessStartInfo
                //        {
                //            CreateNoWindow = true,
                //            UseShellExecute = false,
                //            FileName = ffmpegcore
                //            Arguments = fileargs
                //        }
                //    };
                //    process.Start();
                //    process.WaitForExit(10000);
                //}
                //catch(Exception ex)
                //{
                //    return BadRequest(new JsonResult(ex.Message + " " + ex.StackTrace) { StatusCode = (int)HttpStatusCode.BadRequest });
                //}

            }


            string imageUrl = "https://adminstaging.myavana.com/groupImage/" + imgName;
            string audioUrl = "https://adminstaging.myavana.com/groupAudio/" + audioName;
            string videoUrl = "https://adminstaging.myavana.com/groupVideo/" + videoName;
            string thumbNail = "https://adminstaging.myavana.com/groupThumbnail/" + thumbName;
            groupPost.ImageUrl = imgName != null ? imageUrl : null;
            groupPost.AudioUrl = audioName != null ? audioUrl : null;
            groupPost.VideoUrl = videoName != null ? videoUrl : null;
            groupPost.ThumbnailUrl = thumbName != null ? thumbNail : null;
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            groupPost.UserEmail = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            try
            {
                var response = await MyavanaAdminApiClientFactory.Instance.CreatePostForMobile(groupPost);
                if (response.message == "Success")
                {
                    return Ok(new JsonResult("Post created Successfully") { StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                var abc = ex.Message;
            }
            return BadRequest(new JsonResult("Something went wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpPost]
        [AllowAnonymous]
        public async
            Task<IActionResult> CreateThumbnail([FromBody] GroupPost groupPost)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string thumbName = null;
            if (groupPost.ThumbnailUrl != null)
            {
                var bytes = Convert.FromBase64String(groupPost.ThumbnailUrl);

                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, THUMBNAIL_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, THUMBNAIL_FOLDER));
                }

                string name = Path.Combine(Environment.CurrentDirectory, THUMBNAIL_FOLDER, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ".jpg");
                if (bytes.Length > 0)
                {
                    using (var stream = new FileStream(name, FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                }


            }

            string thumbNail = "http://adminstaging.myavana.com/groupThumbnail/" + thumbName;

            groupPost.ThumbnailUrl = thumbName != null ? thumbNail : null;
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            groupPost.UserEmail = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            try
            {
                var response = await MyavanaAdminApiClientFactory.Instance.CreatePost(groupPost);
                if (response.message == "Success")
                {
                    return Ok(new JsonResult("THumbnail added Successfully") { StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                var abc = ex.Message;
            }
            return BadRequest(new JsonResult("Something went wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        public Bitmap GetThumbnail(string video, string thumbnail)
        {
            var cmd = "ffmpeg  -itsoffset -1  -i " + '"' + video + '"' + " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 320x240 " + '"' + thumbnail + '"';

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + cmd
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            process.WaitForExit(5000);

            //var ms = new MemoryStream(File.ReadAllBytes(thumbnail));
            //return  (Bitmap)Image.FromStream(ms);
            return null; // LoadImage(thumbnail);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddSelectedRoutineItems([FromForm] UserRoutineHairCareModel hairRoutine)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string imgName, imgUrl = string.Empty;
            Random generator = new Random();
            String random = generator.Next(0, 1000000).ToString("D6");
            string imgExt = Path.GetExtension(hairRoutine.FormImage.FileName);
            imgName = hairRoutine.FormImage.FileName.Substring(0, hairRoutine.FormImage.FileName.IndexOf(".")) + "_" + random + imgExt;

            if (hairRoutine.IsProduct)
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), PRODUCT_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, PRODUCT_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), PRODUCT_FOLDER, imgName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await hairRoutine.FormImage.CopyToAsync(stream);
                    imgUrl = "http://admin.myavana.com/routineProducts/" + imgName;
                }

            }
            else if (hairRoutine.IsIngredient)
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), INGREDIENT_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, INGREDIENT_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), INGREDIENT_FOLDER, imgName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await hairRoutine.FormImage.CopyToAsync(stream);
                    imgUrl = "http://admin.myavana.com/routineIngredients/" + imgName;
                }

            }
            else if (hairRoutine.IsRegimen)
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), REGIMEN_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, REGIMEN_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), REGIMEN_FOLDER, imgName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await hairRoutine.FormImage.CopyToAsync(stream);
                    imgUrl = "http://admin.myavana.com/routineRegimens/" + imgName;
                }

            }

            hairRoutine.Image = imgUrl;
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            hairRoutine.UserName = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            try
            {
                hairRoutine.FormImage = null;
                var response = await MyavanaAdminApiClientFactory.Instance.AddSelectedRoutineItems(hairRoutine);
                if (response.message == "Success")
                {
                    return Ok(new JsonResult("Added Successfully") { StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                var abc = ex.Message;
            }
            return BadRequest(new JsonResult("Something went wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveRoutineProfileImage([FromForm] DailyRoutineTracker dailyRoutine)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string imgName = null;

            if (dailyRoutine.Image != null)
            {
                imgName = dailyRoutine.Image.FileName;
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), ROUTINEIMAGE_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, ROUTINEIMAGE_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), ROUTINEIMAGE_FOLDER, imgName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dailyRoutine.Image.CopyToAsync(stream);
                }

            }

            dailyRoutine.ProfileImage = "http://admin.myavana.com/routineProfile/" + imgName;
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            dailyRoutine.UserId = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            try
            {
                var response = await MyavanaAdminApiClientFactory.Instance.AddProfileImage(dailyRoutine);
                if (response.message == "Success")
                {
                    return Ok(new JsonResult("Image added Successfully") { StatusCode = (int)HttpStatusCode.OK, Value = dailyRoutine });
                }
            }
            catch (Exception ex)
            {
                var abc = ex.Message;
            }
            return BadRequest(new JsonResult("Something went wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost]
        //[Route("GenerateThumbnails")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateThumbnails()
        {
            string rootPath = Environment.CurrentDirectory;
            // Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", uniqueFileName);

            string folderPath = Path.Combine(rootPath, "wwwroot/HairProfile/");
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                return BadRequest("Invalid folder path.");
            }
            string thumbnailDir = Path.Combine(rootPath, "wwwroot/HairProfile/HairProfileThumbnails");

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

        public void GenerateThumbnailAndSave(Image originalImage, string outputPath)
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

    }


}