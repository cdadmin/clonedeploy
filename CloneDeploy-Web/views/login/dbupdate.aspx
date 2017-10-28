<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dbupdate.aspx.cs" Inherits="CloneDeploy_Web.views.login.dbupdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EDGE" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <title>Database Update</title>
    <link href="~/content/css/login.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="loginwrapper">

           <asp:Label runat="server" ID="lblVersion"></asp:Label>
            <asp:ScriptManager ID="ScriptManager" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="AsynUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_OnClick" Text="Update" ClientIDMode="Static" />
                    <br class="clear"/> 
                    <asp:Label runat="server" ID="lblResult" ></asp:Label>

                    <asp:UpdateProgress ID="UpdateProgress"
                        runat="server" AssociatedUpdatePanelID="AsynUpdatePanel"
                        DynamicLayout="False">
                        <ProgressTemplate>
                            Update in Progress...
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
                
            </asp:UpdatePanel>
           
        </div>
        <script type="text/javascript" language="javascript">
            var requestManager = Sys.WebForms.PageRequestManager.getInstance();
            requestManager.add_initializeRequest(CancelPostbackForSubsequentSubmitClicks);

            function CancelPostbackForSubsequentSubmitClicks(sender, args) {
                if (requestManager.get_isInAsyncPostBack() & 
            args.get_postBackElement().id == 'btnUpdate')
                {
                    args.set_cancel(true);                
                    alert('Database Update Still In Progress');
                }
            }
    </script>
    </form>

</body>
</html>
