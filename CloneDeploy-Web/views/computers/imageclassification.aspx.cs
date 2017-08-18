using CloneDeploy_Common;
using CloneDeploy_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloneDeploy_Web.views.computers
{
    public partial class imageclassification : BasePages.Computers
    {
        protected void buttonUpdate_OnClick(object sender, EventArgs e)
        {
          
            var list = new List<ComputerImageClassificationEntity>();
            foreach (GridViewRow row in gvClassifications.Rows)
            {
                var cb = (CheckBox)row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvClassifications.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                var computerImageClassification = new ComputerImageClassificationEntity
                {
                    ComputerId = Computer.Id,
                    ImageClassificationId = Convert.ToInt32(dataKey.Value)
                };
                list.Add(computerImageClassification);
            }

            Call.ComputerApi.DeleteImageClassifications(Computer.Id);
            EndUserMessage = Call.ComputerImageClassificationApi.Post(list).Success
                ? "Successfully Updated Image Classifications"
                : "Could Not Update Image Classifications";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            var listOfComputerClassifications = Call.ComputerApi.GetImageClassifications(Computer.Id);

            gvClassifications.DataSource = Call.ImageClassificationApi.Get();
            gvClassifications.DataBind();

            foreach (GridViewRow row in gvClassifications.Rows)
            {
                var chkBox = (CheckBox)row.FindControl("chkSelector");
                var dataKey = gvClassifications.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                foreach (var classification in listOfComputerClassifications)
                {
                    if (classification.ImageClassificationId == Convert.ToInt32(dataKey.Value))
                    {
                        chkBox.Checked = true;
                    }
                }
            }
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvClassifications);
        }
    }
}