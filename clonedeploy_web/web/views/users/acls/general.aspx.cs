using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_users_acl : BasePages.Users
{
    private List<CheckBox> _listCheckBoxes;

    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);

        _listCheckBoxes = new List<CheckBox> {ComputerCreate, ComputerRead, ComputerUpdate, 
            ComputerDelete,ComputerSearch, ImageCreate,ImageRead,ImageUpdate,ImageDelete,ImageSearch,GroupCreate,
            GroupUpdate,GroupRead,GroupDelete,GroupSearch,ProfileCreate,ProfileDelete,ProfileRead,ProfileUpdate,ProfileSearch,
            GlobalCreate,GlobalDelete,GlobalRead,GlobalUpdate,AdminUpdate,ImageTaskUpload,ImageTaskDeploy,
            ImageTaskMulticast,ApproveImage,AllowOnd,AllowDebug,SmartCreate,SmartUpdate};
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        var listOfRights = BLL.UserRight.Get(CloneDeployUser.Id);
        foreach (var right in listOfRights)
        {
            foreach (var box in _listCheckBoxes.Where(box => box.ID == right.Right))
                box.Checked = true;

        }
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        BLL.UserRight.DeleteUserRights(CloneDeployUser.Id);
        var listOfRights = _listCheckBoxes.Where(x => x.Checked).Select(box => new Models.UserRight {UserId = CloneDeployUser.Id, Right = box.ID}).ToList();
        EndUserMessage = BLL.UserRight.AddUserRights(listOfRights)
            ? "Successfully Updated User ACLs"
            : "Could Not Update User ACLs";
    }
}
