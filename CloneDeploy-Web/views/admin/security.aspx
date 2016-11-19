<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="views_admin_security" Codebehind="security.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" runat="Server">
    <li>Security Settings</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
     <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/admin-security.aspx")%>" target="_blank">Help</a></li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Security Settings" OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default" />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#security').addClass("nav-current");
        });
    </script>

    <div class="size-4 column">
        Require Image Approval:
    </div>
    <div class="size-setting column">
        <asp:CheckBox runat="server" ID="chkImageApproval" />
    </div>
    <br class="clear" />
    <br />
    <div class="size-4 column">
        Force SSL:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlSSL" runat="server" CssClass="ddlist">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />
    <div class="size-4 column">
        On Demand Mode:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlOnd" runat="server" CssClass="ddlist">
            <asp:ListItem>Enabled</asp:ListItem>
            <asp:ListItem>Disabled</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />

    <div class="size-4 column">
        Debug Requires Login:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlDebugLogin" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="LoginsChanged">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />
    <div class="size-4 column">
        On Demand Requires Login:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlOndLogin" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="LoginsChanged">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />
    <div class="size-4 column">
        Add Computer Requires Login:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlRegisterLogin" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="LoginsChanged">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />
    <div class="size-4 column">
        Web Tasks Require Login:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlWebTasksLogin" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="LoginsChanged">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />
     <div class="size-4 column">
        Clobber Mode Requires Login:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlClobberLogin" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="LoginsChanged">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />
    <div id="universal" runat="server" visible="False">
        <div class="size-4 column">
            Universal Token:
    <asp:LinkButton ID="btnGenKey" runat="server" Text="Generate" OnClick="btnGenerate_Click" CssClass="btn btn-default right" />
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtToken" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
    </div>
    <br class="clear" />
    <div class="size-4 column">
        Enable LDAP Authentication:
    </div>
    <div class="size-setting column">
        <asp:CheckBox ID="chkldap" runat="server" AutoPostBack="True" OnCheckedChanged="chkldap_OnCheckedChanged"></asp:CheckBox>
    </div>
    <br class="clear" />
    <div id="ad" runat="server" Visible="False">
        <div class="size-4 column">
            LDAP Server:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtldapServer" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear" />
         <div class="size-4 column">
            LDAP Port:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtldapPort" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear" />
         <div class="size-4 column">
            LDAP Authentication Attribute:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtldapAuthAttribute" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear" />
         <div class="size-4 column">
            LDAP Base DN:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtldapbasedn" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear"/>
         <div class="size-4 column">
            LDAP Authentication Type:
        </div>
        <div class="size-setting column">
            <asp:DropDownList ID="ddlldapAuthType" runat="server" CssClass="ddlist">
                <asp:ListItem>Basic</asp:ListItem>
                <asp:ListItem>Secure</asp:ListItem>
                <asp:ListItem>SSL</asp:ListItem>
            </asp:DropDownList>
        </div>
        <br class="clear" />

    </div>


    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" CssClass="modaltitle"></asp:Label>
            </h4>

            <div class="confirm-box-btns">
                <asp:LinkButton ID="OkButton" OnClick="OkButton_Click" runat="server" Text="Yes" CssClass="confirm_yes" />
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no" />
                <br />
                <asp:Label ID="lblClientISO" runat="server" CssClass="smalltext"></asp:Label>

            </div>
        </div>
    </div>
    <div id="discouraged" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <p style="font-size: 18px; text-align: center;">
                <asp:Label ID="lblDiscouraged" runat="server" CssClass="modaltitle" ></asp:Label>
            </p>

            <div class="confirm-box-btns">
                <asp:LinkButton ID="lnkOk" runat="server" Text="OK" CssClass="confirm_no" />

            </div>
        </div>

    </div>

</asp:Content>

