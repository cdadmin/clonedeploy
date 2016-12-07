using System.Collections.Generic;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class MunkiManifestTemplateAPI : GenericAPI<MunkiManifestTemplateEntity>
    {
        public MunkiManifestTemplateAPI(string resource) : base(resource)
        {

        }


        public IEnumerable<MunkiManifestCatalogEntity> GetManifestCatalogs(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManifestCatalogs/{1}", _resource,id);
            return new ApiRequest().Execute<List<MunkiManifestCatalogEntity>>(_request);
        }


        public string GetCatalogCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCatalogCount/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        public IEnumerable<MunkiManifestIncludedManifestEntity> GetManifestIncludedManifests(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManifestIncludedManifests/{1}", _resource, id);
            return new ApiRequest().Execute<List<MunkiManifestIncludedManifestEntity>>(_request);
        }


        public string GetIncludedManifestCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetIncludedManifestCount/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        public IEnumerable<MunkiManifestManagedInstallEntity> GetManifestManagedInstalls(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManifestManagedInstalls/{1}", _resource, id);
            return new ApiRequest().Execute<List<MunkiManifestManagedInstallEntity>>(_request);
        }


        public string GetManagedInstallCount(int id)
        {

            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManagedInstallCount/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;

        }


        public IEnumerable<MunkiManifestManagedUnInstallEntity> GetManifestManagedUninstalls(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManifestManagedUninstalls/{1}", _resource, id);
            return new ApiRequest().Execute<List<MunkiManifestManagedUnInstallEntity>>(_request);
        }


        public string GetManagedUninstallCount(int id)
        {

            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManagedUninstallCount/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;

        }

        public IEnumerable<MunkiManifestManagedUpdateEntity> GetManifestManagedUpdates(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManifestManagedUpdates/{1}", _resource, id);
            return new ApiRequest().Execute<List<MunkiManifestManagedUpdateEntity>>(_request);
        }


        public string GetManagedUpdateCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManagedUpdateCount/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        public IEnumerable<MunkiManifestOptionInstallEntity> GetManifestOptionalInstalls(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetManifestOptionalInstalls/{1}", _resource, id);
            return new ApiRequest().Execute<List<MunkiManifestOptionInstallEntity>>(_request);
        }


        public string GetOptionalInstallCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetOptionalInstallCount/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }

        public bool AddCatalogToTemplate(MunkiManifestCatalogEntity catalog)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/AddCatalogToTemplate/", _resource);
            _request.AddJsonBody(catalog);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }




        public bool DeleteCatalogsFromTemplate(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteCatalogsFromTemplate/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;


        }


        public bool AddManifestToTemplate(MunkiManifestIncludedManifestEntity manifest)
        {

            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/AddManifestToTemplate/", _resource);
            _request.AddJsonBody(manifest);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }




        public bool DeleteManifestsFromTemplate(int id)
        {

            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteManifestsFromTemplate/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;


        }


        public bool AddManagedInstallToTemplate(MunkiManifestManagedInstallEntity managedInstall)
        {

            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/AddManagedInstallToTemplate/", _resource);
            _request.AddJsonBody(managedInstall);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }




        public bool DeleteManagedInstallsFromTemplate(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteManagedInstallsFromTemplate/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;


        }


        public bool AddManagedUninstallsToTemplate(MunkiManifestManagedUnInstallEntity managedUninstall)
        {

            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/AddManagedUninstallsToTemplate/", _resource);
            _request.AddJsonBody(managedUninstall);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }



        public bool DeleteManageUninstallsFromTemplate(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteManageUninstallsFromTemplate/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;


        }


        public bool AddManagedUpdateToTemplate(MunkiManifestManagedUpdateEntity managedUpdate)
        {

            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/AddManagedUpdateToTemplate/", _resource);
            _request.AddJsonBody(managedUpdate);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }




        public bool RemoveManagedUpdatesFromTemplate(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/RemoveManagedUpdatesFromTemplate/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;



        }


        public bool AddOptionalInstallToTemplate(MunkiManifestOptionInstallEntity optionalInstall)
        {

            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/AddOptionalInstallToTemplate/", _resource);
            _request.AddJsonBody(optionalInstall);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;


        }



        public bool DeleteOptonalInstallsFromTemplate(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteOptonalInstallsFromTemplate/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;



        }

        public string GetEffectiveManifest(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }

        public MunkiUpdateConfirmDTO GetUpdateStats(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetUpdateStats/{1}", _resource, id);
            return new ApiRequest().Execute<MunkiUpdateConfirmDTO>(_request);
        }

        public int Apply(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Apply/{1}", _resource, id);
            return new ApiRequest().Execute<ApiIntResponseDTO>(_request).Value;
        }
    }
}