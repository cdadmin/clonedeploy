using System;
using System.Collections.Generic;
using System.IO;
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
    protected void btnGenerate_OnClick(object sender, EventArgs e)
    {
        var output = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                            Path.DirectorySeparatorChar + "client_iso" + Path.DirectorySeparatorChar + "output" +
                            Path.DirectorySeparatorChar;

        if (
            new BLL.Workflows.IsoGen(ddlBuildType.Text, ddlKernel.Text, ddlBootImage.Text, txtKernelArgs.Text).Generate())
            EndUserMessage = "Complete.  Output Located At " + Utility.EscapeFilePaths(output);
        else
        {
            EndUserMessage = "Could Not Create Client ISO Files";
        }
    }
}

