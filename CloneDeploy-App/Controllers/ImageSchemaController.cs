using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class ImageSchemaController : ApiController
    {
        [Authorize]
        [HttpPost]
        public IEnumerable<HardDrive> GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetHardDrivesForGridView();
        }

        [Authorize]
        [HttpPost]
        public List<LogicalVolume> GetLogicalVolumes(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetLogicalVolumesForGridView(schemaRequest.selectedHd);
        }

        [Authorize]
        [HttpPost]
        public List<Partition> GetPartitions(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetPartitionsForGridView(schemaRequest.selectedHd);
        }

        [Authorize]
        [HttpPost]
        public ImageSchemaGridView GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetImageSchema();
        }
    }
}