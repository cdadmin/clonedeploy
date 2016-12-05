using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
    public class ApiObjectResponseDTO
    {
        public ApiObjectResponseDTO()
        {
            Success = false;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string ObjectJson { get; set; }
    }
}
