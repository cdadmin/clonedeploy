using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using CloneDeploy_Services;
using HardDrive = CloneDeploy_Entities.DTOs.ImageSchemaFE.HardDrive;
using LogicalVolume = CloneDeploy_Entities.DTOs.ImageSchemaFE.LogicalVolume;
using Partition = CloneDeploy_Entities.DTOs.ImageSchemaFE.Partition;


namespace CloneDeploy_App.Controllers
{
    public class ImageSchemaAPI : GenericAPI<ImageSchemaGridView>
    {
        public ImageSchemaAPI(string resource):base(resource)
        {
		
        }
    

        [ImageAuth(Permission = "ImageRead")]
        public ImageSchemaGridView GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetImageSchema();
        }

        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<HardDrive> GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            return new ImageSchemaFEServices(schemaRequest).GetHardDrivesForGridView();
        }

        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<Partition> GetPartitions(ImageSchemaRequestDTO schemaRequest, string selectedHd)
        {
            return new ImageSchemaFEServices(schemaRequest).GetPartitionsForGridView(selectedHd);
        }

        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<LogicalVolume> GetLogicalVolumes(ImageSchemaRequestDTO schemaRequest, string selectedHd)
        {
            return new ImageSchemaFEServices(schemaRequest).GetLogicalVolumesForGridView(selectedHd);
        }

     
    }
}