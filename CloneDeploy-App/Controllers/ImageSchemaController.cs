using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class ImageSchemaController: ApiController
    {
        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<Models.ImageSchema.ImageFileInfo> GetPartitionFileInfo(Models.Image image, string selectedHd, string selectedPartition)
        {

            return BLL.ImageSchema.GetPartitionImageFileInfoForGridView(image, selectedHd, selectedPartition);

        }

        [ImageAuth(Permission = "ImageRead")]
        public ApiDTO GetServerImageSize(string imageName, string hdNumber)
        {
            var apiDto = new ApiDTO();
            apiDto.Value =  BLL.ImageSchema.ImageSizeOnServerForGridView(imageName, hdNumber);
            return apiDto;

        }

        [ImageAuth(Permission = "ImageRead")]
        public ApiDTO GetMinimumClientSize(int profileId, int hdNumber)
        {
            var apiDto = new ApiDTO();
            apiDto.Value = BLL.ImageSchema.MinimumClientSizeForGridView(profileId, hdNumber);
            return apiDto;

        }

        [ImageAuth(Permission = "ImageRead")]
        public Models.ImageSchema.GridView.ImageSchemaGridView GetSchema(ImageSchemaRequestDTO schemaRequest)
        {
            return new BLL.ImageSchema(schemaRequest).GetImageSchema();
        }

        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<Models.ImageSchema.GridView.HardDrive> GetHardDrives(ImageSchemaRequestDTO schemaRequest)
        {
            return new BLL.ImageSchema(schemaRequest).GetHardDrivesForGridView();
        }

        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<Models.ImageSchema.GridView.Partition> GetPartitions(ImageSchemaRequestDTO schemaRequest, string selectedHd)
        {
            return new BLL.ImageSchema(schemaRequest).GetPartitionsForGridView(selectedHd);
        }

        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<Models.ImageSchema.GridView.LogicalVolume> GetLogiclVolumes(ImageSchemaRequestDTO schemaRequest, string selectedHd)
        {
            return new BLL.ImageSchema(schemaRequest).GetLogicalVolumesForGridView(selectedHd);
        }

     
    }
}