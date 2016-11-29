using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloneDeploy_Entities;

namespace CloneDeploy_App.DTOs
{
    public class ImageSchemaRequestDTO
    {
        public ImageProfileEntity imageProfile { get; set; }
        public string schemaType { get; set; }
        public ImageEntity image { get; set; }
    }
}