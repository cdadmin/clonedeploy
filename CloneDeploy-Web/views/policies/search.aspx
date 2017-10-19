<%@ Page Title="" Language="C#" MasterPageFile="~/views/policies/policies.master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="CloneDeploy_Web.views.policies.search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubPageActionsRight" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AdditionalActions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Help" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageNavSub" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="SubContent" runat="server">
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
   #trigger-upload {
            color: white;
            background-color: #00ABC7;
            font-size: 14px;
            padding: 7px 20px;
            background-image: none;
        }

        #fine-uploader-manual-trigger .qq-upload-button {
            margin-right: 15px;
        }

        #fine-uploader-manual-trigger .buttons {
            width: 36%;
        }

        #fine-uploader-manual-trigger .qq-uploader .qq-total-progress-bar-container {
            width: 60%;
        }
          </style>
    
    
   

            <script type="text/javascript">
            $(document).ready(function () {

                var myToken = "<%= token %>";
                var myBaseUrl = "<%= baseurl %>";
              

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
                            subDirectory: "test"
                        }
                    },
          
                    retry: {
                        autoAttemptDelay:2,
                        enableAuto: true,
                        maxAutoAttempts:100
                    },
                    chunking: {
                        enabled: true,
                        partSize: 4096000
                    },
                    thumbnails: {
                        placeholders: {
                            waitingPath: '/clonedeploy/content/js/fineuploader/placeholders/waiting-generic.png',
                            notAvailablePath: '/clonedeploy/content/js/fineuploader/placeholders/not_available-generic.png'
                        }
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
