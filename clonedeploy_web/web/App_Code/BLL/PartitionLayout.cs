using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;

namespace BLL
{
    public class PartitionLayout
    {
        private readonly DAL.PartitionLayout _da = new DAL.PartitionLayout();

        public bool AddPartitionLayout(Models.PartitionLayout partitionLayout)
        {

            if (_da.Create(partitionLayout))
            {
                Message.Text = "Successfully Created Partition Layout";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Partition Layout";
                return false;
            }
        }

        public string TotalCount()
        {
            return _da.GetTotalCount();
        }


        public bool DeletePartitionLayout(int partitionLayoutId)
        {
            return _da.Delete(partitionLayoutId);
        }

        public Models.PartitionLayout GetPartitionLayout(int partitionLayoutId)
        {
            return _da.Read(partitionLayoutId);
        }


        public List<Models.PartitionLayout> SearchPartitionLayouts(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdatePartitionLayout(Models.PartitionLayout partitionLayout)
        {
            if (_da.Update(partitionLayout))
                Message.Text = "Successfully Updated Partition Layout";
        }
    }
}