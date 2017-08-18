using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class MunkiManifestTemplateAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public MunkiManifestTemplateAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public bool AddCatalogToTemplate(MunkiManifestCatalogEntity catalog)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/AddCatalogToTemplate/", Resource);
            Request.AddJsonBody(catalog);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool AddManagedInstallToTemplate(MunkiManifestManagedInstallEntity managedInstall)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/AddManagedInstallToTemplate/", Resource);
            Request.AddJsonBody(managedInstall);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool AddManagedUninstallsToTemplate(MunkiManifestManagedUnInstallEntity managedUninstall)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/AddManagedUninstallsToTemplate/", Resource);
            Request.AddJsonBody(managedUninstall);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool AddManagedUpdateToTemplate(MunkiManifestManagedUpdateEntity managedUpdate)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/AddManagedUpdateToTemplate/", Resource);
            Request.AddJsonBody(managedUpdate);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool AddManifestToTemplate(MunkiManifestIncludedManifestEntity manifest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/AddManifestToTemplate/", Resource);
            Request.AddJsonBody(manifest);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool AddOptionalInstallToTemplate(MunkiManifestOptionInstallEntity optionalInstall)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/AddOptionalInstallToTemplate/", Resource);
            Request.AddJsonBody(optionalInstall);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public int Apply(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Apply/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiIntResponseDTO>(Request);
            return response != null ? response.Value : 0;
        }

        public ActionResultDTO Delete(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/Delete/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public bool DeleteCatalogsFromTemplate(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteCatalogsFromTemplate/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool DeleteManagedInstallsFromTemplate(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteManagedInstallsFromTemplate/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool DeleteManageUninstallsFromTemplate(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteManageUninstallsFromTemplate/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool DeleteManifestsFromTemplate(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteManifestsFromTemplate/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool DeleteOptonalInstallsFromTemplate(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteOptonalInstallsFromTemplate/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public MunkiManifestTemplateEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<MunkiManifestTemplateEntity>(Request);
        }

        public List<MunkiManifestTemplateEntity> Get(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<MunkiManifestTemplateEntity>>(Request);
        }


        public string GetCatalogCount(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCatalogCount/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public string GetCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCount", Resource);
            var responseData = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return responseData != null ? responseData.Value : string.Empty;
        }

        public string GetEffectiveManifest(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }


        public string GetIncludedManifestCount(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetIncludedManifestCount/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }


        public string GetManagedInstallCount(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManagedInstallCount/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }


        public string GetManagedUninstallCount(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManagedUninstallCount/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }


        public string GetManagedUpdateCount(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManagedUpdateCount/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public IEnumerable<MunkiManifestCatalogEntity> GetManifestCatalogs(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManifestCatalogs/{1}", Resource, id);
            return _apiRequest.Execute<List<MunkiManifestCatalogEntity>>(Request);
        }


        public IEnumerable<MunkiManifestIncludedManifestEntity> GetManifestIncludedManifests(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManifestIncludedManifests/{1}", Resource, id);
            return _apiRequest.Execute<List<MunkiManifestIncludedManifestEntity>>(Request);
        }


        public IEnumerable<MunkiManifestManagedInstallEntity> GetManifestManagedInstalls(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManifestManagedInstalls/{1}", Resource, id);
            return _apiRequest.Execute<List<MunkiManifestManagedInstallEntity>>(Request);
        }


        public IEnumerable<MunkiManifestManagedUnInstallEntity> GetManifestManagedUninstalls(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManifestManagedUninstalls/{1}", Resource, id);
            return _apiRequest.Execute<List<MunkiManifestManagedUnInstallEntity>>(Request);
        }

        public IEnumerable<MunkiManifestManagedUpdateEntity> GetManifestManagedUpdates(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManifestManagedUpdates/{1}", Resource, id);
            return _apiRequest.Execute<List<MunkiManifestManagedUpdateEntity>>(Request);
        }


        public IEnumerable<MunkiManifestOptionInstallEntity> GetManifestOptionalInstalls(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetManifestOptionalInstalls/{1}", Resource, id);
            return _apiRequest.Execute<List<MunkiManifestOptionInstallEntity>>(Request);
        }


        public string GetOptionalInstallCount(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetOptionalInstallCount/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public MunkiUpdateConfirmDTO GetUpdateStats(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetUpdateStats/{1}", Resource, id);
            return _apiRequest.Execute<MunkiUpdateConfirmDTO>(Request);
        }

        public ActionResultDTO Post(MunkiManifestTemplateEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, MunkiManifestTemplateEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }


        public bool RemoveManagedUpdatesFromTemplate(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/RemoveManagedUpdatesFromTemplate/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }
    }
}