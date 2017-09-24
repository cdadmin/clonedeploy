using System.Collections.Generic;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using Newtonsoft.Json;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ImageSchemaAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ImageSchemaAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        //The restsharp deserializer does not work properly with the image schema.
        //Workaround is to deserialize the string manually with newtonsoft
        public IEnumerable<HardDrive> GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetHardDrives", Resource);
            Request.AddJsonBody(schemaRequest);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return JsonConvert.DeserializeObject<List<HardDrive>>(response.Value);
        }

        public List<LogicalVolume> GetLogicalVolumes(ImageSchemaRequestDTO schemaRequest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetLogicalVolumes", Resource);
            Request.AddJsonBody(schemaRequest);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return JsonConvert.DeserializeObject<List<LogicalVolume>>(response.Value);
        }

        public List<Partition> GetPartitions(ImageSchemaRequestDTO schemaRequest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetPartitions", Resource);
            Request.AddJsonBody(schemaRequest);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return JsonConvert.DeserializeObject<List<Partition>>(response.Value);
        }

        public ImageSchemaGridView GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetSchema", Resource);
            Request.AddJsonBody(schemaRequest);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return JsonConvert.DeserializeObject<ImageSchemaGridView>(response.Value);
        }
    }
}