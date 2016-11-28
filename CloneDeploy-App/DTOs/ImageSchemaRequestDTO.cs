using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloneDeploy_App.DTOs
{
    public class ImageSchemaRequestDTO
    {
        public Models.ImageProfile imageProfile { get; set; }
        public string schemaType { get; set; }
        public Models.Image image { get; set; }
    }
}