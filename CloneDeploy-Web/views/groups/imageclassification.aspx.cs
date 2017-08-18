using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.groups
{
    public partial class imageclassification : Groups
    {
        protected void buttonUpdate_OnClick(object sender, EventArgs e)
        {
            var list = new List<GroupImageClassificationEntity>();
            foreach (GridViewRow row in gvClassifications.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvClassifications.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                var groupImageClassification = new GroupImageClassificationEntity
                {
                    GroupId = Group.Id,
                    ImageClassificationId = Convert.ToInt32(dataKey.Value)
                };
                list.Add(groupImageClassification);
            }

            Call.GroupApi.DeleteImageClassifications(Group.Id);
            EndUserMessage = Call.GroupImageClassificationApi.Post(list).Success
                ? "Successfully Updated Image Classifications"
                : "Could Not Update Image Classifications";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            var listOfGroupClassifications = Call.GroupApi.GetImageClassifications(Group.Id);

            gvClassifications.DataSource = Call.ImageClassificationApi.Get();
            gvClassifications.DataBind();

            foreach (GridViewRow row in gvClassifications.Rows)
            {
                var chkBox = (CheckBox) row.FindControl("chkSelector");
                var dataKey = gvClassifications.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                foreach (var classification in listOfGroupClassifications)
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