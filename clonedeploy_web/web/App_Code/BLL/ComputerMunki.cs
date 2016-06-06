using System.Collections.Generic;

namespace BLL
{
    public static class ComputerMunki
    {
        public static bool AddMunkiTemplates(List<Models.ComputerMunki> listOfTemplates)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var template in listOfTemplates)
                    uow.ComputerMunkiRepository.Insert(template);

                return uow.Save();
            }
        }

        public static bool DeleteMunkiTemplates(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerMunkiRepository.DeleteRange(x => x.ComputerId == computerId);
                return uow.Save();
            }
        }

        public static List<Models.ComputerMunki> Get(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerMunkiRepository.Get(x => x.ComputerId == computerId);
            }
        }

        public static List<Models.ComputerMunki> GetComputersForManifestTemplate(int templateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerMunkiRepository.Get(x => x.MunkiTemplateId == templateId);
            }
        }

       
    }
}