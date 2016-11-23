using System.Collections.Generic;

namespace BLL
{
    public static class ComputerMunki
    {
        //moved
        public static bool AddMunkiTemplates(List<CloneDeploy_Web.Models.ComputerMunki> listOfTemplates)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var template in listOfTemplates)
                    uow.ComputerMunkiRepository.Insert(template);

                return uow.Save();
            }
        }

        //moved
        public static bool DeleteMunkiTemplates(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerMunkiRepository.DeleteRange(x => x.ComputerId == computerId);
                return uow.Save();
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.ComputerMunki> Get(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerMunkiRepository.Get(x => x.ComputerId == computerId);
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.ComputerMunki> GetComputersForManifestTemplate(int templateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerMunkiRepository.Get(x => x.MunkiTemplateId == templateId);
            }
        }

       
    }
}