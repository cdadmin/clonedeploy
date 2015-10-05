using System.Collections.Generic;
using Helpers;

namespace BLL
{
    public class ImageProfilePartition
    {
        private readonly DAL.ImageProfilePartition _da = new DAL.ImageProfilePartition();

        public bool AddImageProfilePartition(Models.ImageProfilePartition imageProfilePartition)
        {
            if (_da.Create(imageProfilePartition))
            {
                Message.Text = "Successfully Added Partition Layout";
                return true;
            }
            else
            {
                Message.Text = "Could Not Add Partition Layout";
                return false;
            }
        }

        public bool DeleteImageProfilePartitions(int profileId)
        {
            return _da.Delete(profileId);
        }

        public List<Models.ImageProfilePartition> SearchImageProfilePartitions(int profileId)
        {
            return _da.Find(profileId);
        }
    }
}