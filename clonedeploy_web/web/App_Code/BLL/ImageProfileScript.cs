using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;

namespace BLL
{
    public class ImageProfileScript
    {
        private readonly DAL.ImageProfileScript _da = new DAL.ImageProfileScript();

        public bool AddImageProfileScript(Models.ImageProfileScript imageProfileScript)
        {
            if (_da.Create(imageProfileScript))
            {
                Message.Text = "Successfully Added Script";
                return true;
            }
            else
            {
                Message.Text = "Could Not Add Script";
                return false;
            }
        }

        public bool DeleteImageProfileScripts(int profileId)
        {
            return _da.Delete(profileId);
        }

        public List<Models.ImageProfileScript> SearchImageProfileScripts(int profileId)
        {
            return _da.Find(profileId);
        }
    }
}