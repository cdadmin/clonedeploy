using System.Collections.Generic;
using Helpers;

namespace BLL
{
    public class Partition
    {
        private readonly DAL.Partition _da = new DAL.Partition();

        public bool AddPartition(Models.Partition partition)
        {
         
            if (_da.Create(partition))
            {
                Message.Text = "Successfully Created Partition";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Partition";
                return false;
            }
        }

        public string TotalCount()
        {
            return _da.GetTotalCount();
        }

      
        public bool DeletePartition(int partitionId)
        {
            return _da.Delete(partitionId);
        }

        public Models.Partition GetPartition(int partitionId)
        {
            return _da.Read(partitionId);
        }

      
        public List<Models.Partition> SearchPartitions(int layoutId)
        {
            return _da.Find(layoutId);
        }

        public void UpdatePartition(Models.Partition partition)
        {
            if (_da.Update(partition))
                Message.Text = "Successfully Updated Partition";
        }

     
    }
}