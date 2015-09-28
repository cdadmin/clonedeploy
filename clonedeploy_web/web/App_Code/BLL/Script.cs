using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;

namespace BLL
{
    public class Script
    {
        private readonly DAL.Script _da = new DAL.Script();

        public bool AddScript(Models.Script script)
        {
            if (_da.Exists(script.Name))
            {
                Message.Text = "A Script With This Name Already Exists";
                return false;
            }
            if (_da.Create(script))
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
            return _da.GetTotalCount();
        }

        public bool DeleteScript(int scriptId)
        {
            return _da.Delete(scriptId);
        }

        public Models.Script GetScript(int scriptId)
        {
            return _da.Read(scriptId);
        }

        public List<Models.Script> SearchScripts(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateScript(Models.Script script)
        {
            if (_da.Update(script))
                Message.Text = "Successfully Updated Script";
        }

      
    }
}