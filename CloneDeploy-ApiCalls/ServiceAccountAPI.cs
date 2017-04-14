using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class ServiceAccountAPI:BaseAPI
    {
        public ServiceAccountAPI(string resource):base (resource)
        {
          
        }

        public ServiceAccountAPI(string resource, CustomApiCallDTO cApiDto)
            : base(resource, cApiDto)
        {
           
        }

        public bool Test()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Test", _resource);
            var response = _cApiDto != null ? new ApiRequest(_cApiDto.Token, _cApiDto.BaseUrl).Execute<ApiBoolResponseDTO>(_request) : new ApiRequest().Execute<ApiBoolResponseDTO>(_request);
            return response != null && response.Value;

        }

        public bool CancelAllImagingTasks()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/CancelAllImagingTasks/", _resource);
            return _cApiDto != null ? new ApiRequest(_cApiDto.Token, _cApiDto.BaseUrl).Execute<ApiBoolResponseDTO>(_request).Value : new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public ServerRoleDTO GetServerRoles()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetServerRoles/", _resource);
            return _cApiDto != null ? new ApiRequest(_cApiDto.Token, _cApiDto.BaseUrl).Execute<ServerRoleDTO>(_request) : new ApiRequest().Execute<ServerRoleDTO>(_request);
        }

      

        public bool WriteTftpFile(TftpFileDTO tftpFile)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/WriteTftpFile/", _resource);
            _request.AddJsonBody(tftpFile);
            return _cApiDto != null ? new ApiRequest(_cApiDto.Token, _cApiDto.BaseUrl).Execute<ApiBoolResponseDTO>(_request).Value : new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public bool DeleteTftpFile(string path)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/DeleteTftpFile/", _resource);
            _request.AddParameter("path", path);
            return _cApiDto != null ? new ApiRequest(_cApiDto.Token, _cApiDto.BaseUrl).Execute<ApiBoolResponseDTO>(_request).Value : new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public string GetTftpServer()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetTftpServer/", _resource);
            return _cApiDto != null ? new ApiRequest(_cApiDto.Token, _cApiDto.BaseUrl).Execute<ApiStringResponseDTO>(_request).Value : new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }

        public int GetMulticastSenderArgs(MulticastArgsDTO multicastArgs)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/GetMulticastSenderArgs/", _resource);
            _request.AddJsonBody(multicastArgs);
            return _cApiDto != null ? new ApiRequest(_cApiDto.Token, _cApiDto.BaseUrl).Execute<ApiIntResponseDTO>(_request).Value : new ApiRequest().Execute<ApiIntResponseDTO>(_request).Value;
        }
    }
}