using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
    public class ValidationResultDTO
    {
        public ValidationResultDTO()
        {
            Success = false;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
