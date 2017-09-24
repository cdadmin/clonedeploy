using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using CloneDeploy_Services;
using Newtonsoft.Json;

namespace CloneDeploy_App.Controllers
{
    // The restsharp deserializer does not work when calling these methods.
    // Must have something to do with when LVM is used.
    // Workaround is to just return the string and deserialize using newtonsoft
    public class ImageSchemaController : ApiController
    {
        [Authorize]
        [HttpPost]
        public ApiStringResponseDTO GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            var hardDrives = new ImageSchemaFEServices(schemaRequest).GetHardDrivesForGridView();
            return new ApiStringResponseDTO(){Value=JsonConvert.SerializeObject(hardDrives)};
        }

        [Authorize]
        [HttpPost]
        public ApiStringResponseDTO GetLogicalVolumes(ImageSchemaRequestDTO schemaRequest)
        {
            var logicalVolumes = new ImageSchemaFEServices(schemaRequest).GetLogicalVolumesForGridView(schemaRequest.selectedHd);
            return new ApiStringResponseDTO() { Value = JsonConvert.SerializeObject(logicalVolumes) };
        }

        [Authorize]
        [HttpPost]
        public ApiStringResponseDTO GetPartitions(ImageSchemaRequestDTO schemaRequest)
        {
            var partitions = new ImageSchemaFEServices(schemaRequest).GetPartitionsForGridView(schemaRequest.selectedHd);
            return new ApiStringResponseDTO() { Value = JsonConvert.SerializeObject(partitions) };
        }

        [Authorize]
        [HttpPost]
        public ApiStringResponseDTO GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            var schema = new ImageSchemaFEServices(schemaRequest).GetImageSchema();
            return new ApiStringResponseDTO() { Value = JsonConvert.SerializeObject(schema) };
        }
    }
}