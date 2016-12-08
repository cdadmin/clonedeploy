using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class MunkiManifestTemplateController: ApiController
    {
         private readonly MunkiManifestTemplateServices _munkiManifestTemplateServices;

        public MunkiManifestTemplateController()
        {
            _munkiManifestTemplateServices = new MunkiManifestTemplateServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestTemplateEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _munkiManifestTemplateServices.SearchManifests()
                : _munkiManifestTemplateServices.SearchManifests(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO() {Value = _munkiManifestTemplateServices.TotalCount()};
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestTemplateEntity Get(int id)
        {
            var result = _munkiManifestTemplateServices.GetManifest(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(MunkiManifestTemplateEntity manifest)
        {
            return _munkiManifestTemplateServices.AddManifest(manifest);
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, MunkiManifestTemplateEntity manifest)
        {
            manifest.Id = id;
            var result = _munkiManifestTemplateServices.UpdateManifest(manifest);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _munkiManifestTemplateServices.DeleteManifest(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
           
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestCatalogEntity> GetManifestCatalogs(int id)
        {
            return _munkiManifestTemplateServices.GetAllCatalogsForMt(id);
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCatalogCount(int id)
        {
            return new ApiStringResponseDTO() {Value = _munkiManifestTemplateServices.GetCatalogTotalCount(id)};
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestIncludedManifestEntity> GetManifestIncludedManifests(int id)
        {
            return _munkiManifestTemplateServices.GetAllIncludedManifestsForMt(id);
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetIncludedManifestCount(int id)
        {
            return new ApiStringResponseDTO() {Value = _munkiManifestTemplateServices.GetIncludedManifestTotalCount(id)};
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestManagedInstallEntity> GetManifestManagedInstalls(int id)
        {
             return _munkiManifestTemplateServices.GetAllManagedInstallsForMt(id);
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetManagedInstallCount(int id)
        {

            return new ApiStringResponseDTO() {Value = _munkiManifestTemplateServices.GetManagedInstallTotalCount(id)};

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestManagedUnInstallEntity> GetManifestManagedUninstalls(int id)
        {
            return _munkiManifestTemplateServices.GetAllManagedUnInstallsForMt(id);
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetManagedUninstallCount(int id)
        {

            return new ApiStringResponseDTO() {Value = _munkiManifestTemplateServices.GetManagedUninstallTotalCount(id)};

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestManagedUpdateEntity> GetManifestManagedUpdates(int id)
        {
            return _munkiManifestTemplateServices.GetAllManagedUpdatesForMt(id);
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetManagedUpdateCount(int id)
        {
            return new ApiStringResponseDTO() {Value = _munkiManifestTemplateServices.GetManagedUpdateTotalCount(id)};
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestOptionInstallEntity> GetManifestOptionalInstalls(int id)
        {
            return _munkiManifestTemplateServices.GetAllOptionalInstallsForMt(id);
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetOptionalInstallCount(int id)
        {
            return new ApiStringResponseDTO() {Value = _munkiManifestTemplateServices.GetOptionalInstallTotalCount(id)};
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddCatalogToTemplate(MunkiManifestCatalogEntity catalog)
        {
            return new ApiBoolResponseDTO() {Value = _munkiManifestTemplateServices.AddCatalogToTemplate(catalog)};
        }



        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteCatalogsFromTemplate(int id)
        {

            return new ApiBoolResponseDTO() {Value = _munkiManifestTemplateServices.DeleteCatalogFromTemplate(id)};

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddManifestToTemplate(MunkiManifestIncludedManifestEntity manifest)
            {

                return new ApiBoolResponseDTO()
                {
                    Value = _munkiManifestTemplateServices.AddIncludedManifestToTemplate(manifest)
                };

            }



        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteManifestsFromTemplate(int id)
        {

            return new ApiBoolResponseDTO()
            {
                Value = _munkiManifestTemplateServices.DeleteIncludedManifestFromTemplate(id)
            };

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddManagedInstallToTemplate(MunkiManifestManagedInstallEntity managedInstall)
        {

            return new ApiBoolResponseDTO()
            {
                Value = _munkiManifestTemplateServices.AddManagedInstallToTemplate(managedInstall)
            };

        }



        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteManagedInstallsFromTemplate(int id)
        {

            return new ApiBoolResponseDTO()
            {
                Value = _munkiManifestTemplateServices.DeleteManagedInstallFromTemplate(id)
            };

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddManagedUninstallsToTemplate(MunkiManifestManagedUnInstallEntity managedUninstall)
        {

            return new ApiBoolResponseDTO()
            {
                Value = _munkiManifestTemplateServices.AddManagedUnInstallToTemplate(managedUninstall)
            };

        }



        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteManageUninstallsFromTemplate(int id)
        {

            return new ApiBoolResponseDTO()
            {
                Value = _munkiManifestTemplateServices.DeleteManagedUnInstallFromTemplate(id)
            };

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddManagedUpdateToTemplate(MunkiManifestManagedUpdateEntity managedUpdate)
        {

            return new ApiBoolResponseDTO()
            {
                Value = _munkiManifestTemplateServices.AddManagedUpdateToTemplate(managedUpdate)
            };

        }



        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO RemoveManagedUpdatesFromTemplate(int id)
        {

            return new ApiBoolResponseDTO() {Value = _munkiManifestTemplateServices.DeleteManagedUpdateFromTemplate(id)};

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddOptionalInstallToTemplate(MunkiManifestOptionInstallEntity optionalInstall)
        {

            return new ApiBoolResponseDTO()
            {
                Value = _munkiManifestTemplateServices.AddOptionalInstallToTemplate(optionalInstall)
            };


        }



        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteOptonalInstallsFromTemplate(int id)
            {

                return new ApiBoolResponseDTO()
                {
                    Value = _munkiManifestTemplateServices.DeleteOptionalInstallFromTemplate(id)
                };

            }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetEffectiveManifest(int id)
        {
            var effectiveManifest = new BLL.Workflows.EffectiveMunkiTemplate().MunkiTemplate(id);
            return new ApiStringResponseDTO() { Value = Encoding.UTF8.GetString(effectiveManifest.ToArray()) };
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiUpdateConfirmDTO GetUpdateStats(int id)
        {
            return new BLL.Workflows.EffectiveMunkiTemplate().GetUpdateStats(id);
        }

        [GlobalAuth(Permission = "GlobalRead")]
        [HttpGet]
        public ApiIntResponseDTO Apply(int id)
        {
            return new ApiIntResponseDTO() {Value = new BLL.Workflows.EffectiveMunkiTemplate().Apply(id)};
        }
    }
}