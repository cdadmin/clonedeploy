using System;
using CloneDeploy_Web.Models;

public partial class views_global_partitions_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var layout = new PartitionLayout
        {
           Name = txtLayoutName.Text,
           Table = ddlLayoutType.Text,
           ImageEnvironment = ddlEnvironment.Text,
           Priority = Convert.ToInt32(txtPriority.Text)
           
        };

        BLL.PartitionLayout.AddPartitionLayout(layout);
     
    }
}