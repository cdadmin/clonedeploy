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

        [ImageAuth(Permission = "ImageRead")]
        [HttpPost]
        public ImageSchemaGridView GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetImageSchema();
        }

        [ImageAuth(Permission = "ImageRead")]
        [HttpPost]
        public IEnumerable<HardDrive> GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetHardDrivesForGridView();
        }

        [ImageAuth(Permission = "ImageRead")]
        [HttpPost]
        public List<Partition> GetPartitions(ImageSchemaRequestDTO schemaRequest, string selectedHd)
        {
            return new ImageSchemaFEServices(schemaRequest).GetPartitionsForGridView(selectedHd);
        }

        [ImageAuth(Permission = "ImageRead")]
        [HttpPost]
        public List<LogicalVolume> GetLogicalVolumes(ImageSchemaRequestDTO schemaRequest, string selectedHd)
        {
            return new ImageSchemaFEServices(schemaRequest).GetLogicalVolumesForGridView(selectedHd);
        }

     
    }
}