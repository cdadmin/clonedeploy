using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Services.Workflows;

namespace CloneDeploy_App.Controllers
{
    public class MunkiManifestTemplateController : ApiController
    {
        private readonly MunkiManifestTemplateServices _munkiManifestTemplateServices;

        public MunkiManifestTemplateController()
        {
            _munkiManifestTemplateServices = new MunkiManifestTemplateServices();
        }

        [CustomAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddCatalogToTemplate(MunkiManifestCatalogEntity catalog)
        {
            return new ApiBoolResponseDTO {Value = _munkiManifestTemplateServices.AddCatalogToTemplate(catalog)};
        }

        [CustomAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddManagedInstallToTemplate(MunkiManifestManagedInstallEntity managedInstall)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.AddManagedInstallToTemplate(managedInstall)
            };
        }

        [CustomAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddManagedUninstallsToTemplate(MunkiManifestManagedUnInstallEntity managedUninstall)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.AddManagedUnInstallToTemplate(managedUninstall)
            };
        }

        [CustomAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddManagedUpdateToTemplate(MunkiManifestManagedUpdateEntity managedUpdate)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.AddManagedUpdateToTemplate(managedUpdate)
            };
        }

        [CustomAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddManifestToTemplate(MunkiManifestIncludedManifestEntity manifest)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.AddIncludedManifestToTemplate(manifest)
            };
        }

        [CustomAuth(Permission = "GlobalCreate")]
        [HttpPost]
        public ApiBoolResponseDTO AddOptionalInstallToTemplate(MunkiManifestOptionInstallEntity optionalInstall)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.AddOptionalInstallToTemplate(optionalInstall)
            };
        }

        [CustomAuth(Permission = "GlobalRead")]
        [HttpGet]
        public ApiIntResponseDTO Apply(int id)
        {
            return new ApiIntResponseDTO {Value = new EffectiveMunkiTemplate().Apply(id)};
        }

        [CustomAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _munkiManifestTemplateServices.DeleteManifest(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }


        [CustomAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteCatalogsFromTemplate(int id)
        {
            return new ApiBoolResponseDTO {Value = _munkiManifestTemplateServices.DeleteCatalogFromTemplate(id)};
        }


        [CustomAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteManagedInstallsFromTemplate(int id)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.DeleteManagedInstallFromTemplate(id)
            };
        }


        [CustomAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteManageUninstallsFromTemplate(int id)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.DeleteManagedUnInstallFromTemplate(id)
            };
        }


        [CustomAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteManifestsFromTemplate(int id)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.DeleteIncludedManifestFromTemplate(id)
            };
        }


        [CustomAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO DeleteOptonalInstallsFromTemplate(int id)
        {
            return new ApiBoolResponseDTO
            {
                Value = _munkiManifestTemplateServices.DeleteOptionalInstallFromTemplate(id)
            };
        }

        [CustomAuth(Permission = "GlobalRead")]
        public MunkiManifestTemplateEntity Get(int id)
        {
            var result = _munkiManifestTemplateServices.GetManifest(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestTemplateEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _munkiManifestTemplateServices.SearchManifests()
                : _munkiManifestTemplateServices.SearchManifests(searchstring);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCatalogCount(int id)
        {
            return new ApiStringResponseDTO {Value = _munkiManifestTemplateServices.GetCatalogTotalCount(id)};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _munkiManifestTemplateServices.TotalCount()};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetEffectiveManifest(int id)
        {
            var effectiveManifest = new EffectiveMunkiTemplate().MunkiTemplate(id);
            return new ApiStringResponseDTO {Value = Encoding.UTF8.GetString(effectiveManifest.ToArray())};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetIncludedManifestCount(int id)
        {
            return new ApiStringResponseDTO {Value = _munkiManifestTemplateServices.GetIncludedManifestTotalCount(id)};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetManagedInstallCount(int id)
        {
            return new ApiStringResponseDTO {Value = _munkiManifestTemplateServices.GetManagedInstallTotalCount(id)};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetManagedUninstallCount(int id)
        {
            return new ApiStringResponseDTO {Value = _munkiManifestTemplateServices.GetManagedUninstallTotalCount(id)};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetManagedUpdateCount(int id)
        {
            return new ApiStringResponseDTO {Value = _munkiManifestTemplateServices.GetManagedUpdateTotalCount(id)};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestCatalogEntity> GetManifestCatalogs(int id)
        {
            return _munkiManifestTemplateServices.GetAllCatalogsForMt(id);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestIncludedManifestEntity> GetManifestIncludedManifests(int id)
        {
            return _munkiManifestTemplateServices.GetAllIncludedManifestsForMt(id);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestManagedInstallEntity> GetManifestManagedInstalls(int id)
        {
            return _munkiManifestTemplateServices.GetAllManagedInstallsForMt(id);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestManagedUnInstallEntity> GetManifestManagedUninstalls(int id)
        {
            return _munkiManifestTemplateServices.GetAllManagedUnInstallsForMt(id);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestManagedUpdateEntity> GetManifestManagedUpdates(int id)
        {
            return _munkiManifestTemplateServices.GetAllManagedUpdatesForMt(id);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<MunkiManifestOptionInstallEntity> GetManifestOptionalInstalls(int id)
        {
            return _munkiManifestTemplateServices.GetAllOptionalInstallsForMt(id);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetOptionalInstallCount(int id)
        {
            return new ApiStringResponseDTO {Value = _munkiManifestTemplateServices.GetOptionalInstallTotalCount(id)};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public MunkiUpdateConfirmDTO GetUpdateStats(int id)
        {
            return new EffectiveMunkiTemplate().GetUpdateStats(id);
        }

        [CustomAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(MunkiManifestTemplateEntity manifest)
        {
            return _munkiManifestTemplateServices.AddManifest(manifest);
        }

        [CustomAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, MunkiManifestTemplateEntity manifest)
        {
            manifest.Id = id;
            var result = _munkiManifestTemplateServices.UpdateManifest(manifest);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }


        [CustomAuth(Permission = "GlobalDelete")]
        public ApiBoolResponseDTO RemoveManagedUpdatesFromTemplate(int id)
        {
            return new ApiBoolResponseDTO {Value = _munkiManifestTemplateServices.DeleteManagedUpdateFromTemplate(id)};
        }
    }
}