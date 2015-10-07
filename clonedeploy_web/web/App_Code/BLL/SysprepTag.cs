using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace BLL
{
    public class SysprepTag
    {
        private DAL.UnitOfWork _unitOfWork;
        public SysprepTag()
        {
            _unitOfWork = new DAL.UnitOfWork();
        }
        public bool AddSysprepTag(Models.SysprepTag sysprepTag)
        {
            if (_unitOfWork.SysprepTagRepository.Exists(s => s.Name == sysprepTag.Name))
            {
                Message.Text = "A Sysprep Tag With This Name Already Exists";
                return false;
            }
            _unitOfWork.SysprepTagRepository.Insert(sysprepTag);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created Sysprep Tag";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Sysprep Tag";
                return false;
            }
        }

        public string TotalCount()
        {
            return _unitOfWork.SysprepTagRepository.Count();
        }

        public bool DeleteScript(int sysprepTagId)
        {
            _unitOfWork.SysprepTagRepository.Delete(sysprepTagId);
            return _unitOfWork.Save();
        }

        public Models.SysprepTag GetSysprepTag(int sysprepTagId)
        {
            return _unitOfWork.SysprepTagRepository.GetById(sysprepTagId);
        }

        public List<Models.SysprepTag> SearchSysprepTags(string searchString)
        {
            return
                _unitOfWork.SysprepTagRepository.Get(
                    s => s.Name.Contains(searchString) || s.OpeningTag.Contains(searchString),
                    orderBy: (q => q.OrderBy(s => s.OpeningTag)));
        }

        public void UpdateSysprepTag(Models.SysprepTag sysprepTag)
        {
            _unitOfWork.SysprepTagRepository.Update(sysprepTag,sysprepTag.Id);
            if (_unitOfWork.Save())
                Message.Text = "Successfully Updated Sysprep Tag";
        }

      
    }
}