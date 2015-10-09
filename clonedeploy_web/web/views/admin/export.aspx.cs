using System;
using BasePages;
using BLL;

namespace views.admin
{
    public partial class AdminExport : Admin
    {
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Setting.ExportDatabase();
        }
    }
}