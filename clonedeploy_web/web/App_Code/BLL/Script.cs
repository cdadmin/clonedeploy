using System.Collections.Generic;
using DAL;
using Helpers;

namespace BLL
{
    public class Script
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public Script()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool AddScript(Models.Script script)
        {
            if (_unitOfWork.ScriptRepository.Exists(s => s.Name == script.Name))
            {
                Message.Text = "A Script With This Name Already Exists";
                return false;
            }
            _unitOfWork.ScriptRepository.Insert(script);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created Script";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Script";
                return false;
            }
        }

        public string TotalCount()
        {
            return _unitOfWork.ScriptRepository.Count();
        }

        public bool DeleteScript(int scriptId)
        {
            _unitOfWork.ScriptRepository.Delete(scriptId);
            return _unitOfWork.Save();
        }

        public Models.Script GetScript(int scriptId)
        {
            return _unitOfWork.ScriptRepository.GetById(scriptId);
        }

        public List<Models.Script> SearchScripts(string searchString)
        {
            return _unitOfWork.ScriptRepository.Get(s => s.Name.Contains(searchString));
        }

        public void UpdateScript(Models.Script script)
        {
            _unitOfWork.ScriptRepository.Update(script, script.Id);
            if (_unitOfWork.Save())
                Message.Text = "Successfully Updated Script";
        }

      
    }
}