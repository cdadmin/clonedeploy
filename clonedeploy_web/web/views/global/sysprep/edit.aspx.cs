using System;
using Models;

public partial class views_global_sysprep_edit : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var sysprepTag = new SysprepTag
        {
            Id = SysprepTag.Id,
            Name = txtName.Text,
            OpeningTag = txtOpenTag.Text,
            ClosingTag = txtCloseTag.Text,
            Description = txtSysprepDesc.Text,
            Contents = txtContent.Text        
        };
      
        BllSysprepTag.UpdateSysprepTag(sysprepTag);
    }

    protected void PopulateForm()
    {
        txtName.Text = SysprepTag.Name;
        txtOpenTag.Text = SysprepTag.OpeningTag;
        txtCloseTag.Text = SysprepTag.ClosingTag;
        txtSysprepDesc.Text = SysprepTag.Description;
        txtContent.Text = SysprepTag.Contents;
    }
}