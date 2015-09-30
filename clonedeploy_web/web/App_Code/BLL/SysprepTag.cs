using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;

namespace BLL
{
    public class SysprepTag
    {
        private readonly DAL.SysprepTag _da = new DAL.SysprepTag();

        public bool AddSysprepTag(Models.SysprepTag sysprepTag)
        {
            if (_da.Exists(sysprepTag.Name))
            {
                Message.Text = "A Sysprep Tag With This Name Already Exists";
                return false;
            }
            if (_da.Create(sysprepTag))
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
            return _da.GetTotalCount();
        }

        public bool DeleteScript(int sysprepTagId)
        {
            return _da.Delete(sysprepTagId);
        }

        public Models.SysprepTag GetSysprepTag(int sysprepTagId)
        {
            return _da.Read(sysprepTagId);
        }

        public List<Models.SysprepTag> SearchSysprepTags(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateSysprepTag(Models.SysprepTag sysprepTag)
        {
            if (_da.Update(sysprepTag))
                Message.Text = "Successfully Updated Sysprep Tag";
        }

      
    }
}