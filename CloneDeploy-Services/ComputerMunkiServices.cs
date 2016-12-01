using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{
    public class ComputerMunkiServices
    {
        private readonly UnitOfWork _uow;

        public ComputerMunkiServices()
        {
            _uow = new UnitOfWork();

        }
        public  ActionResultDTO AddMunkiTemplates(ComputerMunkiEntity computerMunki)
        {
            var actionResult = new ActionResultDTO();
           
                _uow.ComputerMunkiRepository.Insert(computerMunki);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = computerMunki.Id;

                return actionResult;
            
        }

        

       

        public  List<ComputerMunkiEntity> GetComputersForManifestTemplate(int templateId)
        {
            
                return _uow.ComputerMunkiRepository.Get(x => x.MunkiTemplateId == templateId);
            
        }

       
    }
}