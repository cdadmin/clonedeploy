using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using Helpers;

public partial class views_admin_bootmenu_isogen : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlKernel.DataSource = Utility.GetKernels();
            ddlBootImage.DataSource = Utility.GetBootImages();
            ddlKernel.DataBind();
            ddlBootImage.DataBind();
          
        }
    }

    protected void btnSaveEditor_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}