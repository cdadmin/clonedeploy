using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class ImageClassificationController : ApiController
    {
        private readonly ImageClassificationServices _imageClassificationServices;

        public ImageClassificationController()
        {
            _imageClassificationServices = new ImageClassificationServices();
        }

        [CustomAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _imageClassificationServices.DeleteImageClassification(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [Authorize]
        public ImageClassificationEntity Get(int id)
        {
            var result = _imageClassificationServices.GetImageClassification(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [Authorize]
        public IEnumerable<ImageClassificationEntity> Get()
        {
            return _imageClassificationServices.GetAll();
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _imageClassificationServices.TotalCount()};
        }

        [CustomAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(ImageClassificationEntity imageClassification)
        {
            return _imageClassificationServices.AddImageClassification(imageClassification);
        }

        [CustomAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, ImageClassificationEntity imageClassification)
        {
            imageClassification.Id = id;
            var result = _imageClassificationServices.UpdateImageClassification(imageClassification);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}