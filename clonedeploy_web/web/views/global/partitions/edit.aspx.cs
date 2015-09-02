using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;

public partial class views_global_partitions_edit : System.Web.UI.Page
{
    public PartitionLayout Layout { get { return ReadProfile(); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }
    protected void PopulateForm()
    {
        txtLayoutName.Text = Layout.Name;
        ddlLayoutType.Text = Layout.Table;
        ddlEnvironment.Text = Layout.ImageEnvironment;

        var partitions = new Models.Partition();
        gvPartitions.DataSource = partitions.Search(Layout.Id);
        gvPartitions.DataBind();
    }

    private PartitionLayout ReadProfile()
    {
        var tmpLayout = new PartitionLayout { Id = Convert.ToInt32(Request.QueryString["layoutid"]) };
        tmpLayout.Read();
        return tmpLayout;
    }



    protected void AddPartition(object sender, EventArgs e)
    {

       

    }

    protected void DeletePartition(object sender, EventArgs e)
    {



    }



    protected void EditCustomer(object sender, GridViewEditEventArgs e)
    {

      

    }

    protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
    {


    }

    protected void UpdateCustomer(object sender, GridViewUpdateEventArgs e)
    {

       
    }



    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {

   
    }


}