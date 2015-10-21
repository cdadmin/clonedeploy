using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_groups_properties : BasePages.Groups
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        if (ddlHostImage.Text == "Select Image") return;
        PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
    }
}