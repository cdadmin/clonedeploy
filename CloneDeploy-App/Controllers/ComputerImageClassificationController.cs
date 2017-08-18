using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class ComputerImageClassificationController : ApiController
    {
        private readonly ComputerImageClassificationServices _computerImageClassificationServices;

        public ComputerImageClassificationController()
        {
            _computerImageClassificationServices = new ComputerImageClassificationServices();
        }

        [Authorize]
        [HttpPost]
        public IEnumerable<ImageWithDate> FilterClassifications(FilterComputerClassificationDTO filterDto)
        {
            return _computerImageClassificationServices.FilterClassifications(filterDto.ComputerId, filterDto.ListImages);
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateComputer)]
        public ActionResultDTO Post(List<ComputerImageClassificationEntity> listOfClassifications)
        {
            return _computerImageClassificationServices.AddClassifications(listOfClassifications);
        }
    }
}