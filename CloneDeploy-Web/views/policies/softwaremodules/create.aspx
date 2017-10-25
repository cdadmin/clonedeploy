<%@ Page Title="" Language="C#" MasterPageFile="~/views/policies/softwaremodules/software.master" AutoEventWireup="true" CodeBehind="create.aspx.cs" Inherits="CloneDeploy_Web.views.policies.softwaremodules.create" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-dp.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_OnClick" Text="Add SoftwareModule" CssClass="btn btn-default" />
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Display Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="textbox" ClientIDMode="Static"></asp:TextBox>
    </div>
    <br class="clear"/>
      <div class="size-4 column">
        Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDescription" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <br class="clear"/>
     <div class="size-4 column">
        Additional Arguments:
    </div>
     <div class="size-5 column">
        <asp:TextBox ID="txtArguments" runat="server" CssClass="textbox" ClientIDMode="Static"></asp:TextBox>

    </div>
    <br class="clear"/>
     <div class="size-4 column">
        Timeout:
    </div>
     <div class="size-5 column">
        <asp:TextBox ID="txtTimeout" runat="server" CssClass="textbox" ClientIDMode="Static"></asp:TextBox>

    </div>
    <br class="clear" />
    <div class="size-4 column">
        MSI Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlType" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged">
            <asp:ListItem>Install</asp:ListItem>
            <asp:ListItem>Uninstall</asp:ListItem>
            <asp:ListItem>Patch</asp:ListItem>
            </asp:DropDownList>
    </div>
    <br class="clear"/>
   
     <script type="text/template" id="qq-template">
        <div class="qq-uploader-selector qq-uploader qq-gallery" qq-drop-area-text="Drop files here">
            <div class="qq-total-progress-bar-container-selector qq-total-progress-bar-container">
                <div role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" class="qq-total-progress-bar-selector qq-progress-bar qq-total-progress-bar"></div>
            </div>
            <div class="qq-upload-drop-area-selector qq-upload-drop-area" qq-hide-dropzone>
                <span class="qq-upload-drop-area-text-selector"></span>
            </div>
            <div class="qq-upload-button-selector qq-upload-button">
                <div>Select Files</div>

            </div>
            <button type="button" id="trigger-upload" class="btn btn-primary">
                    <i class="icon-upload icon-white"></i> Upload
                </button>
            <span class="qq-drop-processing-selector qq-drop-processing">
                <span>Processing dropped files...</span>
                <span class="qq-drop-processing-spinner-selector qq-drop-processing-spinner"></span>
            </span>
            <ul class="qq-upload-list-selector qq-upload-list" role="region" aria-live="polite" aria-relevant="additions removals">
                <li>
                    <span role="status" class="qq-upload-status-text-selector qq-upload-status-text"></span>
                    <div class="qq-progress-bar-container-selector qq-progress-bar-container">
                        <div role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" class="qq-progress-bar-selector qq-progress-bar"></div>
                    </div>
                    <span class="qq-upload-spinner-selector qq-upload-spinner"></span>
                    <div class="qq-thumbnail-wrapper">
                        <img class="qq-thumbnail-selector" qq-max-size="120" qq-server-scale>
                    </div>
                    <button type="button" class="qq-upload-cancel-selector qq-upload-cancel">X</button>
                    <button type="button" class="qq-upload-retry-selector qq-upload-retry">
                        <span class="qq-btn qq-retry-icon" aria-label="Retry"></span>
                        Retry
                    </button>

                    <div class="qq-file-info">
                        <div class="qq-file-name">
                            <span class="qq-upload-file-selector qq-upload-file"></span>
                            <span class="qq-edit-filename-icon-selector qq-btn qq-edit-filename-icon" aria-label="Edit filename"></span>
                        </div>
                        <input class="qq-edit-filename-selector qq-edit-filename" tabindex="0" type="text">
                        <span class="qq-upload-size-selector qq-upload-size"></span>
                        <button type="button" class="qq-btn qq-upload-delete-selector qq-upload-delete">
                            <span class="qq-btn qq-delete-icon" aria-label="Delete"></span>
                        </button>
                        <button type="button" class="qq-btn qq-upload-pause-selector qq-upload-pause">
                            <span class="qq-btn qq-pause-icon" aria-label="Pause"></span>
                        </button>
                        <button type="button" class="qq-btn qq-upload-continue-selector qq-upload-continue">
                            <span class="qq-btn qq-continue-icon" aria-label="Continue"></span>
                        </button>
                    </div>
                </li>
            </ul>

            <dialog class="qq-alert-dialog-selector">
                <div class="qq-dialog-message-selector"></div>
                <div class="qq-dialog-buttons">
                    <button type="button" class="qq-cancel-button-selector">Close</button>
                </div>
            </dialog>

            <dialog class="qq-confirm-dialog-selector">
                <div class="qq-dialog-message-selector"></div>
                <div class="qq-dialog-buttons">
                    <button type="button" class="qq-cancel-button-selector">No</button>
                    <button type="button" class="qq-ok-button-selector">Yes</button>
                </div>
            </dialog>

            <dialog class="qq-prompt-dialog-selector">
                <div class="qq-dialog-message-selector"></div>
                <input type="text">
                <div class="qq-dialog-buttons">
                    <button type="button" class="qq-cancel-button-selector">Cancel</button>
                    <button type="button" class="qq-ok-button-selector">Ok</button>
                </div>
            </dialog>
        </div>
    </script>
      <style>
  

      
          </style>
    
    
   

            <script type="text/javascript">
             

                $(document).ready(function () {

                    var myToken = "<%= token %>";
                    var myBaseUrl = "<%= baseurl %>";
                    var softwareGuid = "<%= softwareGuid %>";

                    var uploader = new qq.FineUploader({
                        debug: true,
                        element: document.getElementById('uploader'),
                        request: {
                            endpoint: myBaseUrl + 'api/Upload/UploadFile',
                            customHeaders: {
                                "Authorization": "Bearer " + myToken
                            },
                            params: {
                                uploadMethod: "standard",
                                module: "software",
                                subDirectory: softwareGuid
                            }
                        },

                        retry: {
                            autoAttemptDelay: 2,
                            enableAuto: true,
                            maxAutoAttempts: 100
                        },
                        chunking: {
                            enabled: true,
                            partSize: 4096000
                        },
                        thumbnails: {
                            placeholders: {
                                waitingPath: "<%= ResolveUrl("~/content/js/fineuploader/placeholders/waiting-generic.png") %>",
                                notAvailablePath: "<%= ResolveUrl("~/content/js/fineuploader/placeholders/not_available-generic.png") %>"
                            }
                        },
                        validation: {
                            allowedExtensions: ['msi', 'mst', 'msp'],
                            itemLimit: 10
                        
                        },
                        autoUpload: false


                    });

                    qq(document.getElementById("trigger-upload")).attach("click", function () {
                        uploader.uploadStoredFiles();
                    });

                 

                });
            </script>
    
      <div id="uploader"></div>


</asp:Content>
