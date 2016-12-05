using System.Collections.Generic;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using RestSharp;
using HardDrive = CloneDeploy_Entities.DTOs.ImageSchemaFE.HardDrive;
using LogicalVolume = CloneDeploy_Entities.DTOs.ImageSchemaFE.LogicalVolume;
using Partition = CloneDeploy_Entities.DTOs.ImageSchemaFE.Partition;


namespace CloneDeploy_ApiCalls
{
    public class ImageSchemaAPI : GenericAPI<ImageSchemaGridView>
    {
        public ImageSchemaAPI(string resource):base(resource)
        {
		
        }
    

        public ImageSchemaGridView GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetSchema", _resource);
            _request.AddJsonBody(schemaRequest);
            return new ApiRequest().Execute<ImageSchemaGridView>(_request);
        }

        public IEnumerable<HardDrive> GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetHardDrives", _resource);
            _request.AddJsonBody(schemaRequest);
            return new ApiRequest().Execute<List<HardDrive>>(_request);
        }


        public IEnumerable<Partition> GetPartitions(ImageSchemaRequestDTO schemaRequest, string selectedHd)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetPartitions", _resource);
            _request.AddJsonBody(schemaRequest);
            _request.AddParameter("selectedHd", selectedHd);
            return new ApiRequest().Execute<List<Partition>>(_request);
        }

        public IEnumerable<LogicalVolume> GetLogicalVolumes(ImageSchemaRequestDTO schemaRequest, string selectedHd)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetLogicalVolumes", _resource);
            _request.AddJsonBody(schemaRequest);
            _request.AddParameter("selectedHd", selectedHd);
            return new ApiRequest().Execute<List<LogicalVolume>>(_request);
        }

     
    }
}