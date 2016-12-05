using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ImageAPI: GenericAPI<ImageEntity>
    {
        public ImageAPI(string resource):base(resource)
        {
		
        }
    
       

      

        [ImageAuth(Permission = "ImageSearch")]
        public IEnumerable<ImageEntity> Search(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _imageServices.SearchImages()
                : _imageServices.SearchImages(searchstring);
        }

       

        [ImageAuth]
        public ApiBoolResponseDTO SendImageApprovedMail(int id)
        {
            _imageServices.SendImageApprovedEmail(id);
            return new ApiBoolResponseDTO(){Value = true};
        }

     

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileEntity> GetImageProfiles(int imageId)
        {
            return _imageServices.SearchProfiles(imageId);
        }

        [HttpPost]
        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ImageProfileEntity SeedDefaultProfile(int id)
        {
            return _imageServices.SeedDefaultImageProfile(id);
        }

        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<ImageFileInfo> GetPartitionFileInfo(int id, string selectedHd, string selectedPartition)
        {

            return _imageServices.GetPartitionImageFileInfoForGridView(id, selectedHd, selectedPartition);

        }

        [ImageAuth(Permission = "ImageRead")]
        public ApiStringResponseDTO GetImageSizeOnServer(string imageName, string hdNumber)
        {

            return new ApiStringResponseDTO() {Value = _imageServices.ImageSizeOnServerForGridView(imageName, hdNumber)};
        }
    }
}