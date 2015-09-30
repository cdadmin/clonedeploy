using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_global_sysprep_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var sysPrepTag = new Models.SysprepTag()
        {
            Name = txtName.Text,
            OpeningTag = txtOpenTag.Text,
            ClosingTag = txtCloseTag.Text,
            Description = txtSysprepDesc.Text,
            Contents = txtContent.Text
        };
     
        if (new BLL.SysprepTag().AddSysprepTag(sysPrepTag))
            Response.Redirect("~/views/global/sysprep/edit.aspx?cat=sub1&syspreptagid=" + sysPrepTag.Id);
    }
}