using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.Models;

public partial class views_global_partitions_edit : BasePages.Global
{
    public PartitionLayout Layout
    {
        get { return ReadProfile(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
        else
        {
            if (gvPartitions.Rows[0].Cells[0].Text == "No Results")
            {
                gvPartitions.Rows[0].Cells.Clear();
                gvPartitions.Rows[0].Cells.Add(new TableCell());
                gvPartitions.Rows[0].Cells[0].Text = "No Results";
            }
        }
    }

    protected void PopulateForm()
    {
        txtLayoutName.Text = Layout.Name;
        ddlLayoutType.Text = Layout.Table;
        ddlEnvironment.Text = Layout.ImageEnvironment;
        txtPriority.Text = Layout.Priority.ToString();
        BindGrid();
       
    }

    protected void BindGrid()
    {
        gvPartitions.DataSource = BLL.Partition.SearchPartitions(Layout.Id);
        gvPartitions.DataBind();

        if(gvPartitions.Rows.Count == 0)
        {
            var obj = new List<Partition>();
            obj.Add(new Partition());
            gvPartitions.DataSource = obj;
            gvPartitions.DataBind();
            
            // Bind the DataTable which contain a blank row to the GridVi
            int columnsCount = gvPartitions.Columns.Count;
            gvPartitions.Rows[0].Cells.Clear();// clear all the cells in the row
            gvPartitions.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
           // gvPartitions.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

            //You can set the styles here
            //gvPartitions.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            //gvPartitions.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            //gvPartitions.Rows[0].Cells[0].Font.Bold = true;
            //set No Results found to the new added cell
            gvPartitions.Rows[0].Cells[0].Text = "No Results";
            
        }
    }

    private PartitionLayout ReadProfile()
    {
        return BLL.PartitionLayout.GetPartitionLayout(Convert.ToInt32(Request.QueryString["layoutid"]));
    }

    protected void Insert(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        var partition = new Partition
        {
            LayoutId = Layout.Id,
            Type = ((DropDownList)gvRow.FindControl("ddlTypeAdd")).Text,
            Number = Convert.ToInt32(((DropDownList)gvRow.FindControl("ddlNumberAdd")).Text),
            FsType = ((DropDownList)gvRow.FindControl("ddlFsTypeAdd")).Text,
            Size = Convert.ToInt32(((TextBox)gvRow.FindControl("txtSizeAdd")).Text),
            Unit = ((DropDownList)gvRow.FindControl("ddlUnitAdd")).Text,
            Boot = Convert.ToInt32(((CheckBox)gvRow.FindControl("chkBootAdd")).Checked)
        };

        BLL.Partition.AddPartition(partition);

        this.BindGrid();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPartitions.EditIndex = e.NewEditIndex;
        BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow gvRow = gvPartitions.Rows[e.RowIndex];
        var partition = new Partition
        {
            Id = Convert.ToInt32(gvPartitions.DataKeys[e.RowIndex].Values[0]),
            LayoutId = Layout.Id,
            Type = ((DropDownList)gvRow.FindControl("ddlType")).Text,
            Number = Convert.ToInt32(((DropDownList)gvRow.FindControl("ddlNumber")).Text),
            FsType = ((DropDownList)gvRow.FindControl("ddlFsType")).Text,
            Size = Convert.ToInt32(((TextBox)gvRow.FindControl("txtSize")).Text),
            Unit = ((DropDownList)gvRow.FindControl("ddlUnit")).Text,
            Boot = Convert.ToInt32(((CheckBox)gvRow.FindControl("chkBoot")).Checked)
        };
        BLL.Partition.UpdatePartition(partition);

        gvPartitions.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        gvPartitions.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BLL.Partition.DeletePartition(Convert.ToInt32(gvPartitions.DataKeys[e.RowIndex].Values[0]));
        BindGrid();
    }

    protected void ddlTypeAdd_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        DropDownList ddlType = ((DropDownList)gvRow.FindControl("ddlTypeAdd"));
        DropDownList ddlNumber = ((DropDownList)gvRow.FindControl("ddlNumberAdd"));
        ddlNumber.Items.Clear();
        if (ddlType.Text == "Logical")
        {
            for (int i = 5; i <= 10; i++)
                ddlNumber.Items.Insert(i - 5, i.ToString());
        }
        else
        {
            for (int i = 1; i <= 4; i++)
                ddlNumber.Items.Insert(i - 1, i.ToString());
        }
    }

    protected void gvPartitions_OnDataBound(object sender, GridViewRowEventArgs e)
    {
        DropDownList partitionType = null;
        DropDownList partitionNumber = null;
        CheckBox boot = null;
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            partitionType = e.Row.FindControl("ddlTypeAdd") as DropDownList;
            partitionNumber = e.Row.FindControl("ddlNumberAdd") as DropDownList;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            partitionType = e.Row.FindControl("ddlType") as DropDownList;
            partitionNumber = e.Row.FindControl("ddlNumber") as DropDownList;
            boot = e.Row.FindControl("chkBoot") as CheckBox;
        }
        if (partitionNumber != null)
        {
            partitionNumber.Items.Clear();

            if (partitionType.Text == "Logical")
            {
                for (int i = 5; i <= 10; i++)
                    partitionNumber.Items.Insert(i - 5, i.ToString());
            }
            else
            {
                for (int i = 1; i <= 4; i++)
                    partitionNumber.Items.Insert(i - 1, i.ToString());
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                partitionNumber.SelectedValue = ((Partition)(e.Row.DataItem)).Number.ToString();
            }
        }
    }

    protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        DropDownList ddlType = ((DropDownList)gvRow.FindControl("ddlType"));
        DropDownList ddlNumber = ((DropDownList)gvRow.FindControl("ddlNumber"));
        ddlNumber.Items.Clear();
        if (ddlType.Text == "Logical")
        {
            for (int i = 5; i <= 10; i++)
                ddlNumber.Items.Insert(i - 5, i.ToString());
        }
        else
        {
            for (int i = 1; i <= 4; i++)
                ddlNumber.Items.Insert(i - 1, i.ToString());
        }
    }
}