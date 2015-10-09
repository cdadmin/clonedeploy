using System;
using Models;

public partial class views_global_sysprep_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var sysPrepTag = new SysprepTag()
        {
            Name = txtName.Text,
            OpeningTag = txtOpenTag.Text,
            ClosingTag = txtCloseTag.Text,
            Description = txtSysprepDesc.Text,
            Contents = txtContent.Text
        };

        BLL.SysprepTag.AddSysprepTag(sysPrepTag);
            Response.Redirect("~/views/global/sysprep/edit.aspx?cat=sub1&syspreptagid=" + sysPrepTag.Id);
    }
}