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
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class MunkiManifestTemplateAPI : GenericAPI<MunkiManifestTemplateEntity>
    {
        public MunkiManifestTemplateAPI(string resource):base(resource)
        {
		
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
    }
}