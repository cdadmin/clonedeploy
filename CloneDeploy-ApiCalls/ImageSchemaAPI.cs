using System.Collections.Generic;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
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

        public IEnumerable<HardDrive> GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetHardDrives", Resource);
            Request.AddJsonBody(schemaRequest);
            return _apiRequest.Execute<List<HardDrive>>(Request);
        }

        public List<LogicalVolume> GetLogicalVolumes(ImageSchemaRequestDTO schemaRequest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetLogicalVolumes", Resource);
            Request.AddJsonBody(schemaRequest);
            return _apiRequest.Execute<List<LogicalVolume>>(Request);
        }


        public List<Partition> GetPartitions(ImageSchemaRequestDTO schemaRequest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetPartitions", Resource);
            Request.AddJsonBody(schemaRequest);
            return _apiRequest.Execute<List<Partition>>(Request);
        }


        public ImageSchemaGridView GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetSchema", Resource);
            Request.AddJsonBody(schemaRequest);
            return _apiRequest.Execute<ImageSchemaGridView>(Request);
        }
    }
}