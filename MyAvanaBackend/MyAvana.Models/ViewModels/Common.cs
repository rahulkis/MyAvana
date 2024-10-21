using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class Common
    {
    }
    public enum CustomerTypeEnum
    {
        DigitalAnalysis = 1, HairKit = 2, HairKitPlus = 3, Legacy= 4, Registered= 5, SalonHarKit=6
    }
    public enum UserTypeEnum
    {
        Admin = 1, B2B = 2, DataEntry = 3
    }
    public enum SubscriptionTypeEnum
    {
        AnnualSubscription = 1, OneTime=2
    }
    public class ImagesModel
    {
        public string Name { get; set; }
        public List<IFormFile> Files { get; set; }
    }
    public enum HairAnalysisImageTypeEnum
    {
        TopLeft = 1, TopRight = 2, BottomLeft = 3, BottomRight = 4, Crown = 5
    }
    public class TLImagesModel
    {
        public List<IFormFile> Files { get; set; }
        public int HairAnalysisImageType { get; set; }
        public int HairProfileId { get; set; }
        public string UserId { get; set; }

        public int SalonId { get; set; }
    }
    public class HairStrandsImagesModel
    {
        public int HairAnalysisImageType { get; set; }
        public int HairProfileId { get; set; }
        public string HairStrand { get; set; }
        public string UserId { get; set; }
        public int SalonId { get; set; }

    }
    public class FileModel
    {
        public IFormFile FormFile { get; set; }
        public string FileName { get; set; }
        public string UniqueFileName { get; set; }

    }

}
