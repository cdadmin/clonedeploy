using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using CloneDeploy_Services;
using HardDrive = CloneDeploy_Entities.DTOs.ImageSchemaFE.HardDrive;
using LogicalVolume = CloneDeploy_Entities.DTOs.ImageSchemaFE.LogicalVolume;
using Partition = CloneDeploy_Entities.DTOs.ImageSchemaFE.Partition;


namespace CloneDeploy_App.Controllers
{
    public class ImageSchemaController: ApiController
    {

        [Authorize]
        [HttpPost]
        public ImageSchemaGridView GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetImageSchema();
        }

        [Authorize]
        [HttpPost]
        public IEnumerable<HardDrive> GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetHardDrivesForGridView();
        }

        [Authorize]
        [HttpPost]
        public List<Partition> GetPartitions(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetPartitionsForGridView(schemaRequest.selectedHd);
        }

        [Authorize]
        [HttpPost]
        public List<LogicalVolume> GetLogicalVolumes(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetLogicalVolumesForGridView(schemaRequest.selectedHd);
        }

     
    }
}